using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WikipediaConsoleApp;

// Consumes from one queue, distributes to multiple output queues
public class EditDistributor
{
    private readonly BlockingCollection<WikipediaEdit> _inputQueue;
    private readonly List<BlockingCollection<WikipediaEdit>> _outputQueues;
    private readonly HashSet<WikipediaEdit> _seenEdits = new HashSet<WikipediaEdit>(); 

    public EditDistributor(BlockingCollection<WikipediaEdit> inputQueue, List<BlockingCollection<WikipediaEdit>> outputQueues)
    {
        _inputQueue = inputQueue ?? throw new ArgumentNullException(nameof(inputQueue));
        _outputQueues = outputQueues ?? throw new ArgumentNullException(nameof(outputQueues));
        if (_outputQueues.Count == 0)
        {
            throw new ArgumentException("Output queues list cannot be empty.", nameof(outputQueues));
        }
    }

    public Task RunAsync(CancellationToken token)
    {
        Console.WriteLine("[Distributor] Starting...");
        return Task.Run(() =>
        {
            try
            {
                // Consume items from the input queue
                foreach (var edit in _inputQueue.GetConsumingEnumerable(token))
                {
                    // Check if the edit is already seen
                    if (_seenEdits.Add(edit))
                    {
                        // Add the item to *each* output queue
                        foreach (var outputQueue in _outputQueues)
                        {
                            if (token.IsCancellationRequested) break; // Check cancellation often
                            if (!outputQueue.IsAddingCompleted)
                            {
                                try
                                {   
                                    outputQueue.Add(edit, token);
                                    Console.WriteLine($"[Distributor] Distributing edit: {edit.Title} to output queue.");
                                }
                                catch (OperationCanceledException) { break; } // Stop if cancelled during Add
                                catch (InvalidOperationException) { /* Output queue completed, ignore */ }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[Distributor] Duplicate edit skipped: {edit.Title}");
                    }
                    if (token.IsCancellationRequested) break; // Check after distributing
                }
            }
            catch (OperationCanceledException) { Console.WriteLine("[Distributor] Cancellation requested."); }
            catch (InvalidOperationException) { Console.WriteLine("[Distributor] Input queue completed."); }
            catch (Exception ex) { Console.WriteLine($"[Distributor] Unhandled exception: {ex}"); }
            finally
            {
                // Crucial: Signal completion on ALL output queues when the input is done
                foreach (var outputQueue in _outputQueues)
                {
                     if (!outputQueue.IsAddingCompleted)
                     {
                        try { outputQueue.CompleteAdding(); } catch (InvalidOperationException) { /* Ignore */ }
                     }
                }
                Console.WriteLine("[Distributor] Stopped and signaled output queue completion.");
            }
        }, token);
    }
}

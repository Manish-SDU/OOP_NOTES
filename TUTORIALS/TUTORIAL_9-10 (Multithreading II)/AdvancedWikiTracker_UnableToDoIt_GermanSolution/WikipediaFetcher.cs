using System;
using System.Collections.Concurrent; // ADDED
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

namespace WikipediaConsoleApp;

public class WikipediaFetcher
{
    // Task 1: Removed internal list and lock object fields

    // Task 1: Added field for the output queue
    private readonly BlockingCollection<WikipediaEdit> _outputQueue;

    // Fields for timer/cancellation/task management
    private PeriodicTimer? _timer;
    private Task? _fetchingTask;
    private CancellationTokenSource? _cts;

    // Task 1: Modified Constructor
    public WikipediaFetcher(BlockingCollection<WikipediaEdit> outputQueue)
    {
         _outputQueue = outputQueue ?? throw new ArgumentNullException(nameof(outputQueue));
    }

    // Task 1: Implement StartFetching
    public void StartFetching(PeriodicTimer timer)
    {
        // 1. Prevent starting if already running
        if (_fetchingTask != null && !_fetchingTask.IsCompleted)
        {
             Console.WriteLine("[Info] Fetcher already running.");
             return;
        }
        Console.WriteLine("[Info] Starting fetcher...");
        // 2. Store the timer.
        _timer = timer ?? throw new ArgumentNullException(nameof(timer));
        // 3. Create and store a new CancellationTokenSource (_cts).
        _cts = new CancellationTokenSource();
        // 4. Get the token from _cts.
        var token = _cts.Token;
        // 5. Start FetchEditsLoopAsync on a background thread using Task.Run, passing the token and storing the task in _fetchingTask.
        _fetchingTask = Task.Run(() => FetchEditsLoopAsync(token), token);
        // 6. Print confirmation (done above).
    }

    // Task 1: Implement StopFetching (and signal completion)
    public void StopFetching()
    {
        // 1. Prevent stopping if already stopped/stopping
        if (_cts == null || _cts.IsCancellationRequested)
        {
            Console.WriteLine("[Info] Fetcher not running or already stopping.");
            return;
        }
        // 2. Print message.
        Console.WriteLine("[Info] Requesting fetcher stop...");
        // 3. Dispose the timer
        _timer?.Dispose();
        // 4. Request cancellation
        _cts.Cancel();
        // 5. Signal queue completion
        try
        {
            // This allows consumers to finish processing items already in the queue
            if (!_outputQueue.IsAddingCompleted)
            {
                _outputQueue.CompleteAdding();
            }
        }
        catch (InvalidOperationException) { /* Ignore if already completed */ }
        // 6. Dispose the _cts
        _cts?.Dispose();
        // 7. Nullify fields
        _cts = null;
        _timer = null;
        // 8. Print confirmation
        Console.WriteLine("[Info] Fetcher stop requested. Output queue signaled for completion.");
    }

    // Task 1: Implement/Modify FetchEditsLoopAsync
    private async Task FetchEditsLoopAsync(CancellationToken token)
    {
        // 1. Add Console message "Fetcher started". Add try-catch-finally block.
        Console.WriteLine("[Fetcher] Fetching loop started.");
        try
        {
            // 2. Check if _timer is null
            if (_timer == null) return;
            // 3. Start `while (await _timer.WaitForNextTickAsync(token))` loop.
            while (await _timer.WaitForNextTickAsync(token))
            {
                // 4. Inside loop: Print message, call `WikipediaHttpRequest.FetchEditsAsync(token)`.
                Console.WriteLine("[Fetcher] Fetching batch...");
                JsonElement.ArrayEnumerator? results = await WikipediaHttpRequest.FetchEditsAsync(token);

                // 5. Check if results has value.
                if (results.HasValue)
                {
                    int countInBatch = 0;
                    // 6. If yes, iterate through results (`foreach`).
                    foreach (var resultElement in results.Value)
                    {
                        // 7. Inside foreach: Check cancellation, call `JsonConverter.Convert`.
                        if (token.IsCancellationRequested) break;
                        WikipediaEdit? edit = JsonConverter.Convert(resultElement);

                        // 8. If edit is not null: Call `_outputQueue.Add(edit, token)`. Wrap this Add in its own try-catch for InvalidOperationException/OperationCanceledException and break the inner foreach if caught.
                        if (edit != null)
                        {
                            try
                            {
                                 _outputQueue.Add(edit, token);
                                 countInBatch++;
                            }
                            catch(InvalidOperationException) {
                                 Console.WriteLine("[Fetcher] Output queue completed while trying to add. Stopping loop.");
                                 break; // Exit foreach loop
                            }
                            // OperationCanceledException will be caught by the outer handler
                        }
                    } // End foreach
                    if (token.IsCancellationRequested) break; // Check after processing results
                    if (countInBatch > 0) Console.WriteLine($"[Fetcher] Added {countInBatch} edits to queue.");
                }
                else { Console.WriteLine("[Fetcher] No results in batch or API error."); }

                if (token.IsCancellationRequested) break; // Check after processing batch
            } // End while
        }
        // 9. Handle OperationCanceledException for the outer loop.
        catch (OperationCanceledException) { Console.WriteLine("[Fetcher] Loop cancelled."); }
        catch (Exception ex) { Console.WriteLine($"[Fetcher] Error in loop: {ex.Message}"); }
        finally
        {
            // 10. In `finally` block, print message and ensure `_outputQueue.CompleteAdding()` is called if not already completed.
            Console.WriteLine("[Fetcher] Fetching loop stopped.");
            if (!_outputQueue.IsAddingCompleted)
            {
               try { _outputQueue.CompleteAdding(); } catch (InvalidOperationException) { /* Ignore */ }
            }
        }
    }

    // Task 1: Removed GetEditsSnapshot method
}

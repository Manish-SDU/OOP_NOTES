using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json; // For JsonElement
using System.Linq; // For ToList(), Any() etc.

namespace WikipediaConsoleApp;

public class WikipediaFetcher
{
    // Task 4: Define _edits list field
    private readonly List<WikipediaEdit> _edits = new List<WikipediaEdit>();
    // Task 5: Define _lockObject field
    private readonly object _lockObject = new object();
    private PeriodicTimer? _timer;
    private Task? _fetchingTask;
    private CancellationTokenSource? _cts;


    public WikipediaFetcher()
    {
        
    }

    // Task 3: Implement StartFetching
    public void StartFetching(PeriodicTimer timer)
    {
        // 1. Check if already running
        if (_fetchingTask != null && !_fetchingTask.IsCompleted)
        {
             Console.WriteLine("[Info] Fetcher already running.");
             return;
        }
         Console.WriteLine("[Info] Starting fetcher...");
        // 2. Store timer
        _timer = timer ?? throw new ArgumentNullException(nameof(timer));
        // 3. Create CTS
        _cts = new CancellationTokenSource();
        // 4. Get token
        var token = _cts.Token;
        // 5. Start background task
        _fetchingTask = Task.Run(() => FetchEditsLoopAsync(token), token);
    }

    // Task 3: Implement StopFetching
    public void StopFetching()
    {
        // 1. Check if already stopped
        if (_cts == null || _cts.IsCancellationRequested)
        {
            Console.WriteLine("[Info] Fetcher not running or already stopping.");
            return;
        }
         Console.WriteLine("[Info] Requesting fetcher stop...");
        // 2. Dispose timer
        _timer?.Dispose();
        // 3. Request cancellation
        _cts.Cancel();
        // 4. Dispose CTS
        _cts?.Dispose();
        // 5. Mark as stopped
        _cts = null;
        // 6. Mark as stopped
        _timer = null;
    }

    // Task 3, 4, 5: Implement the FetchEditsLoopAsync method
    private async Task FetchEditsLoopAsync(CancellationToken token)
    {
        Console.WriteLine("[Fetcher] Fetching loop started.");
        try
        {
            if (_timer == null) return;

            while (await _timer.WaitForNextTickAsync(token))
            {
                Console.WriteLine("[Fetcher] Fetching batch...");
                // 1. Call helper
                JsonElement.ArrayEnumerator? results = await WikipediaHttpRequest.FetchEditsAsync(token);

                // 2. Check results
                if (results.HasValue)
                {
                    int countInBatch = 0;
                    // 3. Iterate results
                    foreach (var resultElement in results.Value)
                    {
                        // 4. Check cancellation
                        if (token.IsCancellationRequested) break;
                        // 5. Convert JSON
                        WikipediaEdit? edit = JsonConverter.Convert(resultElement);
                        // 6. Check if edit is valid
                        if (edit != null)
                        {
                            // --- Task 5 Implementation ---
                            lock (_lockObject) // Lock the shared resource
                            {
                                _edits.Add(edit); // Add the edit safely
                            }
                            countInBatch++; // Increment counter
                            // --- End Task 5 ---
                        }
                    } // End foreach

                    if (token.IsCancellationRequested) break; 

                    if(countInBatch > 0)
                        Console.WriteLine($"[Fetcher] Added {countInBatch} edits from batch.");

                } 
                else { Console.WriteLine("[Fetcher] No results in batch or API error."); }

                 if (token.IsCancellationRequested) break; 

            } 
        }
        catch (OperationCanceledException) { Console.WriteLine("[Fetcher] Loop cancelled."); }
        catch (Exception ex) { Console.WriteLine($"[Fetcher] Error in loop: {ex.Message}"); }
        finally { Console.WriteLine("[Fetcher] Fetching loop stopped."); }
    }


    // Task 5: Implement GetEdits safely
    public List<WikipediaEdit> GetEditsSnapshot()
    {
        List<WikipediaEdit> snapshot;
        // 1. Use lock
        lock (_lockObject)
        {
            // 2. Create copy and return
            snapshot = _edits.ToList(); // Requires using System.Linq
        }
        return snapshot;
    }
}
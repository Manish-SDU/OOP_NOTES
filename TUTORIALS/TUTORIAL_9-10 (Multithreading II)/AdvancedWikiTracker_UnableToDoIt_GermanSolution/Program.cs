using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WikipediaConsoleApp;

class Program
{
    private const int FetchingPeriodInSeconds = 3;

    // Task 4: Define BlockingCollections
    private static BlockingCollection<WikipediaEdit> s_rawEditsQueue = null!;
    private static BlockingCollection<WikipediaEdit> s_categoryQueue = null!;
    private static BlockingCollection<WikipediaEdit> s_userStatsQueue = null!;

    // Task 4: Define component instances
    private static WikipediaFetcher? s_fetcher;
    private static EditDistributor? s_distributor;
    private static CategoryCounter? s_categoryCounter;
    private static UserChangeTracker? s_userTracker;

    // Task Management
    private static CancellationTokenSource? s_cancellationTokenSource;
    private static List<Task> s_runningTasks = new List<Task>();
    private static PeriodicTimer? s_fetchTimer;


    static async Task Main(string[] args) // Make Main async
    {
        Console.WriteLine("Wikipedia Edit Processing Pipeline");
        bool isRunning = true;

        while (isRunning)
        {
            DisplayMenu();
            var input = Console.ReadLine();
            Console.Clear();

            switch (input)
            {
                case "1": // Start Pipeline
                    // Task 4: Implement StartProcessingAsync
                    await StartProcessingAsync();
                    PauseForUser();
                    break;

                case "2": // Stop Pipeline
                    // Task 4: Implement StopProcessingAsync
                    await StopProcessingAsync();
                    PauseForUser();
                    break;

                case "3": // Show Category Counts
                    // Task 4: Implement Category Display
                    if (s_categoryCounter == null)
                    {
                        Console.WriteLine("Pipeline not running.");
                    }
                    else
                    {
                        Console.WriteLine("\nCategory Counts (Occurrences > 1)");
                        Console.WriteLine("=====================================");
                        var counts = s_categoryCounter.GetCategoryCounts();
                        var sortedCounts = counts
                            .Where(kvp => kvp.Value > 1)
                            .OrderByDescending(kvp => kvp.Value)
                            .ThenBy(kvp => kvp.Key)
                            .Take(25) // Limit output
                            .ToList();

                        if (!sortedCounts.Any())
                        {
                            Console.WriteLine("(No categories with count > 1 yet)");
                        }
                        else
                        {
                            foreach (var kvp in sortedCounts)
                            {
                                Console.WriteLine($"- {kvp.Key}: {kvp.Value}");
                            }
                             Console.WriteLine($"\n(Showing Top {sortedCounts.Count} categories with count > 1)");
                        }
                    }
                    PauseForUser();
                    break;

                case "4": // Show User Stats
                    // Task 4: Implement User Stats Display
                    if (s_userTracker == null)
                    {
                        Console.WriteLine("Pipeline not running.");
                    }
                    else
                    {
                        Console.WriteLine("\nUser Stats (Total Absolute Line Changes)");
                        Console.WriteLine("==========================================");
                        var stats = s_userTracker.GetUserStats();
                        var sortedStats = stats
                            .OrderByDescending(kvp => kvp.Value)
                            .ThenBy(kvp => kvp.Key)
                            .Take(25) // Limit output
                            .ToList();

                        if (!sortedStats.Any())
                        {
                            Console.WriteLine("(No user stats tracked yet)");
                        }
                        else
                        {
                            foreach (var kvp in sortedStats)
                            {
                                Console.WriteLine($"- {kvp.Key}: {kvp.Value:N0} lines");
                            }
                             Console.WriteLine($"\n(Showing Top {sortedStats.Count} users)");
                        }
                    }
                    PauseForUser();
                    break;

                case "5": // Exit
                    // Task 4: Implement Exit
                    Console.WriteLine("Exiting...");
                    await StopProcessingAsync(); // Ensure graceful shutdown
                    isRunning = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    PauseForUser();
                    break;
            }
        }
        Console.WriteLine("\nApplication exited. Press Enter to close window.");
        Console.ReadLine();
    }

    // Task 4: Implement StartProcessingAsync
    static async Task StartProcessingAsync()
    {
        // 1. Check if already running
        if (s_cancellationTokenSource != null && !s_cancellationTokenSource.IsCancellationRequested)
        {
            Console.WriteLine("Pipeline already running.");
            return;
        }
        // 2. Print message
        Console.WriteLine("Starting pipeline...");
        // 3. Create CTS and token
        s_cancellationTokenSource = new CancellationTokenSource();
        var token = s_cancellationTokenSource.Token;

        // 4. Create queues (using ConcurrentQueue for fair FIFO behaviour)
        s_rawEditsQueue = new BlockingCollection<WikipediaEdit>(new ConcurrentQueue<WikipediaEdit>());
        s_categoryQueue = new BlockingCollection<WikipediaEdit>(new ConcurrentQueue<WikipediaEdit>());
        s_userStatsQueue = new BlockingCollection<WikipediaEdit>(new ConcurrentQueue<WikipediaEdit>());

        // 5. Instantiate Fetcher
        s_fetcher = new WikipediaFetcher(s_rawEditsQueue);
        // 6. Instantiate Distributor
        s_distributor = new EditDistributor(s_rawEditsQueue, new List<BlockingCollection<WikipediaEdit>> { s_categoryQueue, s_userStatsQueue });
        // 7. Instantiate CategoryCounter
        s_categoryCounter = new CategoryCounter(s_categoryQueue);
        // 8. Instantiate UserTracker
        s_userTracker = new UserChangeTracker(s_userStatsQueue);
        // 9. Create Timer
        s_fetchTimer = new PeriodicTimer(TimeSpan.FromSeconds(FetchingPeriodInSeconds));

        // 10. Clear and prepare task list
        s_runningTasks.Clear();

        // 11. Start tasks and add to list
        s_runningTasks.Add(s_distributor.RunAsync(token));
        s_runningTasks.Add(s_categoryCounter.RunAsync(token));
        s_runningTasks.Add(s_userTracker.RunAsync(token));
        s_fetcher.StartFetching(s_fetchTimer); // This starts the internal fetcher task

        // 12. Print confirmation
        Console.WriteLine($"Processing pipeline started (fetching every {FetchingPeriodInSeconds} seconds).");

        await Task.CompletedTask; // Keep method async if needed elsewhere, otherwise can be void
    }

    // Task 4: Implement StopProcessingAsync
    static async Task StopProcessingAsync()
    {
        // 1. Check if already stopped
        if (s_cancellationTokenSource == null)
        {
            Console.WriteLine("Pipeline not running.");
            return;
        }
        // 2. Print message
        Console.WriteLine("Stopping pipeline...");
        // 3. Dispose timer
        s_fetchTimer?.Dispose();
        s_fetchTimer = null;
        // 4. Signal cancellation
        if (!s_cancellationTokenSource.IsCancellationRequested)
        {
            s_cancellationTokenSource.Cancel();
        }
        // 5. Signal fetcher completion (which completes raw queue)
        s_fetcher?.StopFetching(); // Call StopFetching which now includes CompleteAdding

        // 6. Wait for all tasks
        Console.WriteLine("Waiting for tasks to complete...");
        try
        {
           // Combine fetcher's internal task if accessible, otherwise rely on queue completion
           // Note: Fetcher's internal task isn't directly exposed here, but it should stop due to cancellation/timer disposal.
           // Distributor and Consumers stop when their input queues are completed & empty.
           await Task.WhenAll(s_runningTasks).WaitAsync(TimeSpan.FromSeconds(15)); // Wait for distributor and consumers
           Console.WriteLine("All processing tasks completed gracefully.");
        }
        catch (TimeoutException) { Console.WriteLine("Warning: Timeout waiting for tasks to complete."); }
        catch (OperationCanceledException) { Console.WriteLine("Tasks cancelled."); } // Expected if cancelled during wait
        catch (Exception ex) { Console.WriteLine($"Error during shutdown wait: {ex.Message}"); }

        // 7. Dispose CTS, clear list, nullify components
        s_cancellationTokenSource?.Dispose();
        s_cancellationTokenSource = null;
        s_runningTasks.Clear();
        s_fetcher = null;
        s_distributor = null;
        s_categoryCounter = null;
        s_userTracker = null;
        s_rawEditsQueue?.Dispose(); // Dispose queues
        s_categoryQueue?.Dispose();
        s_userStatsQueue?.Dispose();


        // 8. Print confirmation
        Console.WriteLine("Processing pipeline stopped.");
    }

    // Task 4: Update DisplayMenu
    static void DisplayMenu()
    {
        Console.WriteLine("\n========== WIKIPEDIA PIPELINE ==========");
        string status = (s_cancellationTokenSource != null && !s_cancellationTokenSource.IsCancellationRequested) ? "Running" : "Stopped";
        Console.WriteLine($"Status: {status}");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1. Start Pipeline");
        Console.WriteLine("2. Stop Pipeline");
        Console.WriteLine("3. Show Category Counts (>1)");
        Console.WriteLine("4. Show User Stats");
        Console.WriteLine("5. Exit");
        Console.Write("Enter choice: ");
    }

    static void PauseForUser()
    {
        Console.WriteLine("\nPress Enter to return to menu...");
        Console.ReadLine();
        Console.Clear();
    }
}
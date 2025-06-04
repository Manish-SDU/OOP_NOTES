using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace WikipediaConsoleApp;

public class UserChangeTracker
{
    private readonly BlockingCollection<WikipediaEdit> _inputQueue;
    private readonly ConcurrentDictionary<string, long> _userLineChanges = new ConcurrentDictionary<string, long>();

    // Task 3: Implement Constructor
    public UserChangeTracker(BlockingCollection<WikipediaEdit> userStatsQueue)
    {
        _inputQueue = userStatsQueue ?? throw new ArgumentNullException(nameof(userStatsQueue));
    }

    // Task 3: Implement RunAsync
    public Task RunAsync(CancellationToken token)
    {
        Console.WriteLine("[UserTracker] Starting...");
        return Task.Run(() =>
        {
            try
            {
                // 1. Use GetConsumingEnumerable
                foreach (var edit in _inputQueue.GetConsumingEnumerable(token))
                {
                    // 2. Get current edit
                    // 3. Check if user is valid
                    if (!string.IsNullOrWhiteSpace(edit.User))
                    {
                        // 4. Calculate absolute change and update dictionary
                        long absChange = Math.Abs((long)edit.SizeChange);
                        _userLineChanges.AddOrUpdate(edit.User, absChange, (key, currentTotal) => currentTotal + absChange);
                    }
                    Console.WriteLine($"[UserTracker] User: {edit.User}, SizeChange: {edit.SizeChange}");
                }
            }
            catch (OperationCanceledException) { Console.WriteLine("[UserTracker] Cancellation requested."); }
            catch (InvalidOperationException) { Console.WriteLine("[UserTracker] Input queue completed."); }
            catch (Exception ex) { Console.WriteLine($"[UserTracker] Unhandled exception: {ex}"); }
            finally { Console.WriteLine("[UserTracker] Stopped."); }
        }, token);
    }

    // Task 3: Implement GetUserStats
    public IReadOnlyDictionary<string, long> GetUserStats()
    {
        // Return the dictionary
        return _userLineChanges;
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace WikipediaConsoleApp;

public class CategoryCounter
{
    private readonly BlockingCollection<WikipediaEdit> _inputQueue;
    private readonly ConcurrentDictionary<string, int> _categoryCounts = new ConcurrentDictionary<string, int>();

    // Task 2: Implement Constructor
    public CategoryCounter(BlockingCollection<WikipediaEdit> categoryQueue)
    {
         _inputQueue = categoryQueue ?? throw new ArgumentNullException(nameof(categoryQueue));
    }

    // Task 2: Implement RunAsync
    public Task RunAsync(CancellationToken token)
    {
        Console.WriteLine("[CategoryCounter] Starting...");
        return Task.Run(() =>
        {
            try
            {
                // 1. Use GetConsumingEnumerable
                foreach (var edit in _inputQueue.GetConsumingEnumerable(token))
                {
                    // 2. Get current edit
                    // 3. Check if Categories is not null/empty
                    if (edit.Categories != null && edit.Categories.Any())
                    {
                        // 4. Loop through categories
                        foreach (var category in edit.Categories)
                        {
                            // 5. Check if category is valid
                            if (!string.IsNullOrWhiteSpace(category) &&
                                category != "Article not found" &&
                                category != "Error fetching categories" &&
                                category != "No categories property found" &&
                                category != "Invalid title" &&
                                category != "Cancelled" &&
                                category != "Error")
                            {
                                // 6. Update dictionary atomically
                                _categoryCounts.AddOrUpdate(category, 1, (key, currentCount) => currentCount + 1);
                                Console.WriteLine($"[CategoryCounter] Category: {category}, Count: {_categoryCounts.GetValueOrDefault(category)}");
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException) { Console.WriteLine("[CategoryCounter] Cancellation requested."); }
            catch (InvalidOperationException) { Console.WriteLine("[CategoryCounter] Input queue completed."); }
            catch (Exception ex) { Console.WriteLine($"[CategoryCounter] Unhandled exception: {ex}"); }
            finally { Console.WriteLine("[CategoryCounter] Stopped."); }
        }, token);
    }

    // Task 2: Implement GetCategoryCounts
    public IReadOnlyDictionary<string, int> GetCategoryCounts()
    {
        // Return the dictionary (reads are generally safe)
        return _categoryCounts;
    }
}
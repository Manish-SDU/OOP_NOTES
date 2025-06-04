using System;
using System.Collections.Generic; 
using System.Linq;            
using System.Threading;       
using System.Threading.Tasks;  

namespace WikipediaConsoleApp;

class Program
{
    private const int FetchingPeriodInSeconds = 10; 

    private static readonly WikipediaFetcher s_fetcher = new WikipediaFetcher();

    private static PeriodicTimer? s_timer; 

    static void Main(string[] args) 
    {
        Console.WriteLine("Simple Wikipedia Edit Viewer");
        bool isRunning = true;

        while (isRunning)
        {
            DisplayMenu();
            var input = Console.ReadLine();
            Console.Clear();

            switch (input)
            {
                case "1": 
                    // Task 3: Start Fetching implementation
                    if (s_timer != null)
                    {
                        Console.WriteLine("Already fetching.");
                    }
                    else
                    {
                        Console.WriteLine("Starting fetch...");
                        s_timer = new PeriodicTimer(TimeSpan.FromSeconds(FetchingPeriodInSeconds));
                        s_fetcher.StartFetching(s_timer);
                        Console.WriteLine($"Fetching started (every {FetchingPeriodInSeconds} seconds).");
                    }
                    PauseForUser();
                    break;

                case "2": 
                     // Task 3: Stop Fetching implementation
                     if (s_timer == null)
                     {
                         Console.WriteLine("Not currently fetching.");
                     }
                     else
                     {
                         Console.WriteLine("Stopping fetch...");
                         s_fetcher.StopFetching();
                         s_timer?.Dispose();       
                         s_timer = null;          
                         Console.WriteLine("Fetching stopped.");
                     }
                     PauseForUser();
                    break;

                case "3": 
                   // Task 4 & 5: Print Current Edits implementation
                   Console.WriteLine("Current Edits Log:");
                   Console.WriteLine("====================");
                   List<WikipediaEdit> editsSnapshot = s_fetcher.GetEditsSnapshot(); 

                   if (!editsSnapshot.Any()) 
                   {
                       Console.WriteLine("(No edits fetched yet)");
                   }
                   else
                   {
                       foreach (var edit in editsSnapshot.AsEnumerable().Reverse().Take(50))
                       {
                           Console.WriteLine(edit); 
                       }
                       Console.WriteLine("--------------------");
                       Console.WriteLine($"Total edits stored: {editsSnapshot.Count} (Displaying latest 50)");
                   }
                   PauseForUser();
                    break;

                case "4": // Exit
                    Console.WriteLine("Exiting...");
                    if (s_timer != null)
                    {
                         Console.WriteLine("Ensuring fetcher is stopped...");
                         s_fetcher.StopFetching();
                         s_timer?.Dispose();
                         s_timer = null;
                    }
                    isRunning = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    PauseForUser();
                    break;
            }
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("\n========== MENU ==========");
        string status = (s_timer == null) ? "Stopped" : "Running";
        Console.WriteLine($"Status: {status}");
        Console.WriteLine("--------------------------");
        Console.WriteLine("1. Start Fetching");
        Console.WriteLine("2. Stop Fetching");
        Console.WriteLine("3. Print Current Edits");
        Console.WriteLine("4. Exit");
        Console.Write("Enter choice: ");
    }

    static void PauseForUser()
    {
        Console.WriteLine("\nPress Enter to return to menu...");
        Console.ReadLine();
        Console.Clear(); 
    }
}

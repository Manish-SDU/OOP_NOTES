using System;
using System.Linq;

namespace WikipediaConsoleApp;

public class WikipediaEdit
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public string? User { get; set; }
    public int SizeChange { get; set; }
    public string? Timestamp { get; set; }

    // Task 1: Implement ToString()
    public override string ToString()
    {
        // Implemented using string interpolation and null-coalescing operator
        return $"Title: {Title ?? "N/A"} | User: {User ?? "N/A"} | Change: {SizeChange} | Time: {Timestamp ?? "N/A"}";
    }
}

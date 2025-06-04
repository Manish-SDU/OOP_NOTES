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
    public string[]? Categories { get; set; } // Populated by CategoryFetcherConsumer

    public override string ToString()
    {
        string baseInfo = $"Title: {Title ?? "N/A"} | User: {User ?? "N/A"} | Change: {SizeChange} | Time: {Timestamp ?? "N/A"}";
        // Include categories if they exist and are meaningful
        if (Categories != null && Categories.Any())
        {
            var displayCategories = Categories
                .Where(c => c != "Article not found" && c != "Error fetching categories" && c != "No categories property found" && c != "Invalid title" && c != "Cancelled" && c != "Error");
            if (displayCategories.Any())
            {
                return $"{baseInfo} | Categories: [{string.Join(", ", displayCategories)}]";
            }
            else if(Categories.Any()) // Show placeholder if only placeholders existed
            {
                 return $"{baseInfo} | Categories: ({Categories[0]})";
            }
        }
        return baseInfo;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        WikipediaEdit other = (WikipediaEdit)obj;
        return Timestamp == other.Timestamp && Title == other.Title && User == other.User && SizeChange == other.SizeChange;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Timestamp, Title, User, SizeChange);
    }
}
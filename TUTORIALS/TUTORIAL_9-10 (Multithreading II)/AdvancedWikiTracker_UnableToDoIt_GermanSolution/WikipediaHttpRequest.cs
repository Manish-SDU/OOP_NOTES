using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace WikipediaConsoleApp;

public static class WikipediaHttpRequest
{
    private static readonly HttpClient _httpClient = new HttpClient();

    static WikipediaHttpRequest()
    {
       _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WikipediaPipelineConsoleApp/1.0 (Tutorial; contact@example.com)");
    }

    public static async Task<JsonElement.ArrayEnumerator?> FetchEditsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            string url = "https://en.wikipedia.org/w/api.php?action=query&format=json&list=recentchanges&rcprop=title|user|timestamp|sizes";
            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            var jsonStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var json = await JsonSerializer.DeserializeAsync<JsonElement>(jsonStream, cancellationToken: cancellationToken);

            if (json.TryGetProperty("query", out var queryElement) &&
                queryElement.TryGetProperty("recentchanges", out var recentChangesElement) &&
                recentChangesElement.ValueKind == JsonValueKind.Array)
            {
                return recentChangesElement.EnumerateArray();
            }
            Console.WriteLine("\n[Error] Could not find 'query.recentchanges' array in API response.\n");
        }
         catch (HttpRequestException ex) { Console.WriteLine($"\n[Error] Network error fetching edits: {ex.Message}\n"); }
         catch (JsonException ex) { Console.WriteLine($"\n[Error] JSON error parsing edits: {ex.Message}\n"); }
         catch (OperationCanceledException) { Console.WriteLine("\n[Info] Edit fetching cancelled.\n"); }
         catch (Exception ex) { Console.WriteLine($"\n[Error] Unexpected error fetching edits: {ex.Message}\n"); }
        return null;
    }

    // This method IS needed again for the CategoryFetcherConsumer
    public static async Task<string[]> FetchCategoriesAsync(string title, CancellationToken cancellationToken = default)
    {
         if (string.IsNullOrWhiteSpace(title)) return ["Invalid title"];
        try
        {
            string categoryUrl = $"https://en.wikipedia.org/w/api.php?action=query&format=json&prop=categories&cllimit=max&titles={Uri.EscapeDataString(title)}";
            using var response = await _httpClient.GetAsync(categoryUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            var jsonStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var json = await JsonSerializer.DeserializeAsync<JsonElement>(jsonStream, cancellationToken: cancellationToken);

            if (json.TryGetProperty("query", out var queryElement) &&
                queryElement.TryGetProperty("pages", out var pagesElement))
            {
                foreach (var page in pagesElement.EnumerateObject())
                {
                    if (page.Value.ValueKind == JsonValueKind.Object &&
                        page.Value.TryGetProperty("categories", out var categories) &&
                        categories.ValueKind == JsonValueKind.Array)
                    {
                        return categories.EnumerateArray()
                            .Select(cat => cat.TryGetProperty("title", out var catTitle) ? catTitle.GetString()?.Replace("Category:", "") ?? "Unknown" : "Unknown")
                            .ToArray();
                    }
                    if (page.Value.TryGetProperty("missing", out _)) { return ["Article not found"]; }
                }
                 return ["No categories property found"];
            }
        }
         catch (HttpRequestException ex) { Console.WriteLine($"⚠️ Network error fetching categories for {title}: {ex.Message}"); }
         catch (JsonException ex) { Console.WriteLine($"⚠️ JSON error parsing categories for {title}: {ex.Message}"); }
         catch (OperationCanceledException) { Console.WriteLine($"Category fetch cancelled for {title}."); return ["Cancelled"]; } // Indicate cancellation
        catch (Exception ex) { Console.WriteLine($"⚠️ Unexpected error fetching categories for {title}: {ex.Message}"); }
        return ["Error fetching categories"];
    }
}

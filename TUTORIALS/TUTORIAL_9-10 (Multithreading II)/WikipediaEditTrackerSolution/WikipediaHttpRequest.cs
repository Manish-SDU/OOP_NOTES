using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Threading; // For CancellationToken

namespace WikipediaConsoleApp; // Ensure namespace matches others

public static class WikipediaHttpRequest
{
    // HttpClient is intended to be instantiated once and re-used.
    private static readonly HttpClient _httpClient = new HttpClient();

    static WikipediaHttpRequest()
    {
        // Set a default User-Agent (good practice for APIs)
       _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("SimpleWikipediaConsoleApp/1.0 (Beginner Tutorial; contact@example.com)");
    }


    // Fetches recent changes list
    public static async Task<JsonElement.ArrayEnumerator?> FetchEditsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            string url = "https://en.wikipedia.org/w/api.php?action=query&format=json&list=recentchanges&rcprop=title|user|timestamp|sizes";
            // Use cancellationToken if needed by GetAsync or ReadAsStreamAsync
            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            var jsonStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            // Ensure correct deserialization options if needed, but defaults are often fine
            var json = await JsonSerializer.DeserializeAsync<JsonElement>(jsonStream, cancellationToken: cancellationToken);

            // Navigate the JSON structure safely
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
}

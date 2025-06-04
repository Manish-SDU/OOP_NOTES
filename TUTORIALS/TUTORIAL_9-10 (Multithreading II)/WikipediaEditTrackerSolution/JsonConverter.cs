using System;
using System.Text.Json;

namespace WikipediaConsoleApp;

public static class JsonConverter
{
    public static WikipediaEdit? Convert(JsonElement element)
    {
        // This method takes a JsonElement representing one edit from the API
        // and should return a WikipediaEdit object.
        try
        {
            // 1. Create a new instance
            var edit = new WikipediaEdit();

            // 2. Extract basic properties 
            edit.Type = element.TryGetProperty("type", out var typeEl) ? typeEl.GetString() : null;
            edit.Title = element.TryGetProperty("title", out var titleEl) ? titleEl.GetString() : null;
            edit.Timestamp = element.TryGetProperty("timestamp", out var tsEl) ? tsEl.GetString() : null;

            // 3. Calculate SizeChange 
            int newLen = element.TryGetProperty("newlen", out var newLenEl) && newLenEl.TryGetInt32(out int nl) ? nl : 0;
            int oldLen = element.TryGetProperty("oldlen", out var oldLenEl) && oldLenEl.TryGetInt32(out int ol) ? ol : 0;
            edit.SizeChange = newLen - oldLen;

            // 4. Determine User: Handle 'anon' property
            // Task 2: Implement the 'anon' check logic.
            if (element.TryGetProperty("anon", out _)) // Check if "anon" property exists
            {
                edit.User = "anon";
            }
            else // Otherwise (if "anon" doesn't exist)
            {
                // Try to get the "user" property
                if (element.TryGetProperty("user", out var userEl))
                {
                    edit.User = userEl.GetString(); 
                }
                else
                {
                    edit.User = "Unknown"; 
                }
            }

            return edit;
        }
        catch (Exception ex)
        {
             Console.WriteLine($"\n[Error] Failed to convert JSON element: {ex.Message}\n");
             return null;
        }
    }
}

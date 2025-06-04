using System;
using System.Text.Json;

namespace WikipediaConsoleApp;

public static class JsonConverter
{
    public static WikipediaEdit? Convert(JsonElement element)
    {
        try
        {
            var edit = new WikipediaEdit();
            edit.Type = element.TryGetProperty("type", out var typeEl) ? typeEl.GetString() : null;
            edit.Title = element.TryGetProperty("title", out var titleEl) ? titleEl.GetString() : null;
            edit.Timestamp = element.TryGetProperty("timestamp", out var tsEl) ? tsEl.GetString() : null;

            int newLen = element.TryGetProperty("newlen", out var newLenEl) && newLenEl.TryGetInt32(out int nl) ? nl : 0;
            int oldLen = element.TryGetProperty("oldlen", out var oldLenEl) && oldLenEl.TryGetInt32(out int ol) ? ol : 0;
            edit.SizeChange = newLen - oldLen;

            // Task 2 from previous tutorial should be done here:
            if (element.TryGetProperty("anon", out _))
            { edit.User = "anon"; }
            else if (element.TryGetProperty("user", out var userEl))
            { edit.User = userEl.GetString(); }
            else
            { edit.User = "Unknown"; }

            return edit;
        }
        catch (Exception ex)
        {
             Console.WriteLine($"\n[Error] Failed to convert JSON element: {ex.Message}\n");
             return null;
        }
    }
}

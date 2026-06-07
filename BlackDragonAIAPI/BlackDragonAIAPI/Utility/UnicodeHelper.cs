using System.Text.Encodings.Web;
using System.Text.Json;

namespace BlackDragonAIAPI.Utility;

public static class UnicodeHelper
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Encoder = JavaScriptEncoder.Default
    };

    public static string EncodeWithEncoder(string input)
    {
        return JsonSerializer.Serialize(input, Options);
    }

    public static string DecodeWithEncoder(string encodedString)
    {
        return JsonSerializer.Deserialize<string>(encodedString, Options);
    }
}
using System.Text.Json.Serialization;

namespace MicrosoftStoreBadge.Models;

public record ShieldsEndpointResponse(string Label, string Message, Color Color = Color.BrightGreen, bool IsError = false, int SchemaVersion = 1)
{
    public static ShieldsEndpointResponse Ok(string label, string message, Color color = Color.BrightGreen)
    {
        return new ShieldsEndpointResponse(label, message, color);
    }

    public static ShieldsEndpointResponse Error(string errorMessage)
    {
        return new ShieldsEndpointResponse("Error", errorMessage, Color.Red, true);
    }
}

public enum Color
{
    [JsonPropertyName("brightgreen")]
    BrightGreen,

    [JsonPropertyName("green")]
    Green,

    [JsonPropertyName("yellowgreen")]
    YellowGreen,

    [JsonPropertyName("yellow")]
    Yellow,

    [JsonPropertyName("orange")]
    Orange,

    [JsonPropertyName("red")]
    Red,

    [JsonPropertyName("blue")]
    Blue,

    [JsonPropertyName("lightgrey")]
    LightGrey
}
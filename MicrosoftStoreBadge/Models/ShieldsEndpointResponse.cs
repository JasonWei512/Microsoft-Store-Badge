using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MicrosoftStoreBadge.Models;

public class ShieldsEndpointResponse
{
    public string Label { get; set; }
    public string Message { get; set; }

    public Color Color { get; set; } = Color.Blue;
    public bool IsError { get; set; } = false;

    public int SchemaVersion { get; set; } = 1;

    public ShieldsEndpointResponse(string label, string message, Color color = Color.BrightGreen, bool isError = false)
    {
        this.Label = label;
        this.Message = message;
        this.Color = color;
        this.IsError = isError;
    }

    public static ShieldsEndpointResponse Ok(string label, string errorMessage, Color color = Color.BrightGreen)
    {
        return new ShieldsEndpointResponse(label, errorMessage, color);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrosoftStoreBadge.Models;

public record ShieldsEndpointResponse(
    string Label,
    string Message,

    Color Color = Color.Blue,
    bool IsError = false,

    int SchemaVersion = 1
);

public enum Color
{
    BrightGreen,
    Green,
    YellowGreen,
    Yellow,
    Orange,
    Red,
    Blue,
    LightGrey
}
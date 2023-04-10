using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MicrosoftStoreBadge.Models;
using MicrosoftStoreBadge.Services;
using StoreLib.Services;

namespace MicrosoftStoreBadge.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingController : ControllerBase
{
    private readonly ILogger<RatingController> logger;
    private readonly MicrosoftStoreService microsoftStoreService;

    public RatingController(ILogger<RatingController> logger, MicrosoftStoreService microsoftStoreService)
    {
        this.logger = logger;
        this.microsoftStoreService = microsoftStoreService;
    }

    [HttpGet]
    [OutputCache(PolicyName = "Expire12Hours")]
    public async Task<ShieldsEndpointResponse> GetMicrosoftStoreRating([FromQuery] string? storeId, [FromQuery] string? market = null)
    {
        if (string.IsNullOrEmpty(storeId))
        {
            return ShieldsEndpointResponse.Error(@"You need to specify a ""storeId"" in query");
        }

        Market? marketEnum = null;

        if (!string.IsNullOrEmpty(market))
        {
            if (Enum.TryParse<Market>(market, true, out Market marketParseResult))
            {
                marketEnum = marketParseResult;
            }
            else
            {
                return ShieldsEndpointResponse.Error($@"Invalid market ""{market}""");
            }
        }

        MicrosoftStoreService.AppRating? appRating = await microsoftStoreService.GetAppRating(storeId, marketEnum);

        if (appRating is not null)
        {
            string averageRating = double.IsNaN(appRating.AverageRating) ?
                "~" :   // If no one rates the app, appRating.AverageRating will be NaN
                appRating.AverageRating.ToString("F1"); // 3.1415926 -> 3.1

            string ratingCount = ((double)appRating.RatingCount).ToMetric(decimals: 1); // 12345 -> 12.3k

            return ShieldsEndpointResponse.Ok("rating", $"{averageRating}/5 ({ratingCount})");
        }
        else
        {
            return ShieldsEndpointResponse.Error(@$"App with ID ""{storeId}"" not found");
        }
    }
}

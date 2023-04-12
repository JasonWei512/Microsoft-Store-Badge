using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
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
    private readonly IDistributedCache cache;

    public RatingController(ILogger<RatingController> logger, MicrosoftStoreService microsoftStoreService, IDistributedCache cache)
    {
        this.logger = logger;
        this.microsoftStoreService = microsoftStoreService;
        this.cache = cache;
    }

    [HttpGet]
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

        string cacheKey = $"rating:{storeId}";

        if (await cache.GetStringAsync(cacheKey) is string cachedMessage)
        {
            return ShieldsEndpointResponse.Ok("rating", cachedMessage);
        }

        MicrosoftStoreService.AppRating? appRating = await microsoftStoreService.GetAppRating(storeId, marketEnum);

        if (appRating is null)
        {
            return ShieldsEndpointResponse.Error(@$"App with ID ""{storeId}"" not found");
        }

        string averageRating = double.IsNaN(appRating.AverageRating) ?
            "~" :   // If no one rates the app, appRating.AverageRating will be NaN
            appRating.AverageRating.ToString("F1"); // 3.1415926 -> 3.1

        string ratingCount = ((double)appRating.RatingCount).ToMetric(decimals: 1); // 12345 -> 12.3k

        string message = $"{averageRating}/5 ({ratingCount})";

        _ = cache.SetStringAsync(cacheKey, message, new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
        });

        return ShieldsEndpointResponse.Ok("rating", message);
    }
}

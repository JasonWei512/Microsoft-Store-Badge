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

    /// <summary>
    /// Get the rating of a Microsoft Store app with <paramref name="storeId"/>.
    /// </summary>
    ///
    /// <remarks>
    /// The API response will be consumed by <a href="https://shields.io">Shields.IO</a> to generate an <a href="https://shields.io/endpoint">endpoint badge</a>.
    /// </remarks>
    ///
    /// <param name="storeId">
    /// The store ID of the Microsoft Store app. <br/>
    /// How to get the store ID: <br />
    /// 1. Open Microsoft Store and go to the app page. <br/>
    /// 2. Click the share button and select "copy link". <br />
    /// 3. A url like <strong>https://www.microsoft.com/store/productId/9NF7JTB3B17P</strong> will be copied to clipboard. <br/>
    /// 4. The last segment <strong>9NF7JTB3B17P</strong> is the store ID. <br/>
    /// </param>
    /// <param name="market">(Optional) The default value is "US".</param>
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MicrosoftStoreBadge.Models;
using MicrosoftStoreBadge.Services;
using StoreLib.Models;
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
            string averageRating = ((double)appRating.AverageRating).ToString("F1");

            return ShieldsEndpointResponse.Ok("rating", $"{averageRating}/5 ({appRating.RatingCount})");
        }
        else
        {
            return ShieldsEndpointResponse.Error(@$"App with ID ""{storeId}"" not found");
        }
    }
}

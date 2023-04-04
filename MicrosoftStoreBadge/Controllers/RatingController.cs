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
    public async Task<ShieldsEndpointResponse> GetMicrosoftStoreRating([FromQuery] string storeId, [FromQuery] string? market = null)
    {
        Market? marketEnum = null;

        if (!string.IsNullOrEmpty(market))
        {
            if (Enum.TryParse<Market>(market, true, out Market marketParseResult))
            {
                marketEnum = marketParseResult;
            }
            else
            {
                return new ShieldsEndpointResponse(
                    "Error",
                    $@"Invalid market ""{market}""",
                    IsError: true
                );
            }
        }

        double? rating = await microsoftStoreService.GetAppRating(storeId, marketEnum);

        if (rating is not null)
        {
            return new ShieldsEndpointResponse(
                "Microsoft Store",
                $"Rating {((double)rating).ToString("F2")}"
            );
        }
        else
        {
            return new ShieldsEndpointResponse(
                "Error",
                @$"App with ID ""{storeId}"" not found",
                IsError: true
            );
        }
    }
}

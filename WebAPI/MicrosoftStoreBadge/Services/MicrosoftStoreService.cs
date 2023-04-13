using StoreLib.Models;
using StoreLib.Services;

namespace MicrosoftStoreBadge.Services;

public class MicrosoftStoreService
{
    private readonly ILogger<MicrosoftStoreService> logger;

    public record AppRating(double AverageRating, long RatingCount);

    public MicrosoftStoreService(ILogger<MicrosoftStoreService> logger)
    {
        this.logger = logger;
    }

    public async Task<AppRating?> GetAppRating(string storeId, Market? market = null)
    {
        DisplayCatalogHandler dcatHandler = new(DCatEndpoint.Production, new Locale(market ?? Market.US, Lang.en, true));

        await dcatHandler.QueryDCATAsync(storeId);

        if (!dcatHandler.IsFound)
        {
            return null;
        }

        logger.LogInformation("Getting store rating for {appName} ({appId})", dcatHandler.ProductListing.Product.LocalizedProperties.FirstOrDefault()?.ProductTitle, storeId);

        List<(double averageRating, long ratingCount)> ratings = dcatHandler.ProductListing.Product.MarketProperties
            .Select(
                market => market.UsageData.FirstOrDefault(usage => usage.AggregateTimeSpan == "AllTime")
            )
            .Where(usage => usage is not null)
            .Select(usage => (usage!.AverageRating, usage!.RatingCount))
            .ToList();

        double totalRating = ratings.Sum(r => r.averageRating * r.ratingCount);
        long ratingCount = ratings.Sum(r => r.ratingCount);

        double averageRating = totalRating / ratingCount;

        return new AppRating(averageRating, ratingCount);
    }
}

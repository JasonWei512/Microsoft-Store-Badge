using StoreLib.Models;
using StoreLib.Services;

namespace MicrosoftStoreBadge.Services;

public class MicrosoftStoreService
{
    public async Task<double?> GetAppRating(string storeId, Market? market = null)
    {
        DisplayCatalogHandler dcatHandler = new(DCatEndpoint.Production, new Locale(market ?? Market.US, Lang.en, true));

        await dcatHandler.QueryDCATAsync(storeId);

        if (!dcatHandler.IsFound)
        {
            return null;
        }

        List<(double averageRating, long ratingCount)> ratings = dcatHandler.ProductListing.Product.MarketProperties
            .SelectMany(x => x.UsageData)
            .Select(x => (x.AverageRating, x.RatingCount))
            .ToList();

        double averageRating =
            ratings.Sum(x => x.averageRating * x.ratingCount)
            /
            ratings.Sum(x => x.ratingCount);

        return averageRating;
    }
}

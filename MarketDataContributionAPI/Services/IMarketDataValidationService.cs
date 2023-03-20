using MarketDataContributionAPI.Models;

namespace MarketDataContributionAPI.Services;

public interface IMarketDataValidationService
{
    MarketDataValidationResult ValidateMarketData(FxQuote marketData);
}
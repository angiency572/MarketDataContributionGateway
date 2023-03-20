namespace MarketDataContributionAPI.Services;

using MarketDataContributionAPI.Models;

public class MarketDataValidationService : IMarketDataValidationService
{
    public MarketDataValidationResult ValidateMarketData(FxQuote marketData)
    {
        var result = new MarketDataValidationResult();

        // Validate the market data
        if (marketData.CurrencyPair == null || marketData.CurrencyPair.Length != 6)
        {
            result.IsValid = false;
            return result;
        }

        if (marketData.Bid < 0 || marketData.Ask < 0)
        {
            result.IsValid = false;
            return result;
        }

        if (marketData.Bid > marketData.Ask)
        {
            result.IsValid = false;
            return result;
        }

        // If all validations pass, set the result to valid and generate a unique identifier
        result.IsValid = true;
        result.Identifier = Guid.NewGuid().ToString();

        return result;
    }
}
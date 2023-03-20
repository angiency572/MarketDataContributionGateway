using Amazon.DynamoDBv2.DataModel;

namespace MarketDataContributionAPI.Models;

public class FxQuote
{
    public string CurrencyPair { get; set; }
    public decimal Bid { get; set; }
    public decimal Ask { get; set; }
}

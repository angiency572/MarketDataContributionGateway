namespace MarketDataContributionAPI.Models;


public class FxQuoteResponse
{
    public string Identifier { get; set; }
    public FxQuoteStatus Status { get; set; }
}

public enum FxQuoteStatus
{
    Success,
    Failure
}

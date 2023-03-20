using MarketDataContributionAPI.Models;
using MarketDataContributionAPI.Services;

namespace MarketDataContributionAPI.Tests;

[TestClass]
public class MarketDataValidationServiceTests
{
    private MarketDataValidationService _service;

    [TestInitialize]
    public void Setup()
    {
        _service = new MarketDataValidationService();
    }

    [TestMethod]
    public void TestValidMarketData()
    {
        // Arrange
        var marketData = new FxQuote
        {
            CurrencyPair = "EURUSD",
            Bid = 1.1m,
            Ask = 1.2m
        };

        // Act
        var result = _service.ValidateMarketData(marketData);

        // Assert
        Assert.IsTrue(result.IsValid);
        Assert.IsNotNull(result.Identifier);
    }

    [TestMethod]
    public void TestInvalidCurrencyPair()
    {
        // Arrange
        var marketData = new FxQuote
        {
            CurrencyPair = "EUR",
            Bid = 1.1m,
            Ask = 1.2m
        };

        // Act
        var result = _service.ValidateMarketData(marketData);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsNull(result.Identifier);
    }

    [TestMethod]
    public void TestNegativeBid()
    {
        // Arrange
        var marketData = new FxQuote
        {
            CurrencyPair = "EURUSD",
            Bid = -1.1m,
            Ask = 1.2m
        };

        // Act
        var result = _service.ValidateMarketData(marketData);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsNull(result.Identifier);
    }

    [TestMethod]
    public void TestNegativeAsk()
    {
        // Arrange
        var marketData = new FxQuote
        {
            CurrencyPair = "EURUSD",
            Bid = 1.1m,
            Ask = -1.2m
        };

        // Act
        var result = _service.ValidateMarketData(marketData);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsNull(result.Identifier);
    }

    [TestMethod]
    public void TestBidGreaterThanAsk()
    {
        // Arrange
        var marketData = new FxQuote
        {
            CurrencyPair = "EURUSD",
            Bid = 1.2m,
            Ask = 1.1m
        };

        // Act
        var result = _service.ValidateMarketData(marketData);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsNull(result.Identifier);
    }
}
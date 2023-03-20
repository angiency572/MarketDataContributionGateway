using Microsoft.AspNetCore.Mvc;
using MarketDataContributionAPI.Models;
using Amazon.DynamoDBv2.DataModel;
using MarketDataContributionAPI.DataAccess;
using Amazon.DynamoDBv2.Model;
using MarketDataContributionAPI.Helpers;
using MarketDataContributionAPI.Services;

namespace MarketDataContributionAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MarketDataController : ControllerBase
{
    private readonly IMarketDataValidationService _marketDataValidationService;
    private readonly LoggerHelper _logger;
    public MarketDataController(IMarketDataValidationService marketDataValidationService, LoggerHelper logger)
    {
        _marketDataValidationService = marketDataValidationService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitMarketData(FxQuote marketData)
    {
        try
        {
            // Validate the market data
            if (!_marketDataValidationService.ValidateMarketData(marketData).IsValid)
            {
                return BadRequest("Invalid market data");
            }

            // Get the authenticated user's API key from the request header
            var apiKey = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var dbClient = await AmazonDynamoDB.GetClientAsync();

            // Verify the API key against database   
            if (!await ApiKeyHelper.VerifyApiKey(dbClient,apiKey))
            {
                return Unauthorized();
            }

            var dbContext = new DynamoDBContext(dbClient);

            // Create a new FxQuote object to save to DynamoDB
            var fxQuote = new FxQuote
            {
                CurrencyPair = marketData.CurrencyPair,
                Bid = marketData.Bid,
                Ask = marketData.Ask
            };

            // Save the FxQuote object to DynamoDB
            dbContext.SaveAsync(fxQuote).Wait();

            // Return a unique identifier and success status
            var response = new FxQuoteResponse
            {
                Identifier = Guid.NewGuid().ToString(),
                Status = FxQuoteStatus.Success
            };

            _logger.Info("Market data submitted successfully");

            return Ok(response);

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to submit market data");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{currencyPair}")]
    public async Task<IActionResult> GetMarketDataAsync(string currencyPair)
    {
        try
        {
            // Get the authenticated user's API key from the request header
            var apiKey = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var dbClient = await AmazonDynamoDB.GetClientAsync();

            // Verify the API key or token against database      
            if (!await ApiKeyHelper.VerifyApiKey(dbClient,apiKey))
            {
                return Unauthorized();
            }

            var queryRequest = new QueryRequest
            {
                TableName = "FxQuote",
                KeyConditionExpression = "CurrencyPair = :cp",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":cp", new AttributeValue { S = currencyPair } }
                }
            };

            var response = await dbClient.QueryAsync(queryRequest);

            var fxQuotes = response.Items.Select(item => new FxQuote
            {
                CurrencyPair = item["CurrencyPair"].S,
                Bid = decimal.Parse(item["Bid"].N),
                Ask = decimal.Parse(item["Ask"].N)
            });

            _logger.Info("Market data retrieved successfully");

            return Ok(fxQuotes);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to retrieve market data");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}

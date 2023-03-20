namespace MarketDataContributionAPI.Helpers;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Threading.Tasks;

public static class ApiKeyHelper
{
    public static async Task<bool> VerifyApiKey(AmazonDynamoDBClient dynamoDBClient,string apiKey)
    {
        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfiguration configuration = configBuilder.Build();
        string aesKey = configuration.GetSection("Encryption")["Key"];
        string aesIV = configuration.GetSection("Encryption")["IV"];

        string decryptedApiKey = EncryptionHelper.Decrypt(aesKey, aesIV, apiKey);

        var table = Table.LoadTable(dynamoDBClient, "ApiKeys");
        var search = table.Query(new QueryFilter("ApiKey", QueryOperator.Equal, decryptedApiKey));
        var document = await search.GetNextSetAsync();

        return document.Count > 0;
    }
}
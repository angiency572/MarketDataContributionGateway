namespace MarketDataContributionAPI.DataAccess;

using Amazon.DynamoDBv2;
using Amazon.Runtime;
using MarketDataContributionAPI.Helpers;
using Microsoft.Extensions.Configuration;
using NLog;
using System.Threading.Tasks;

public static class AmazonDynamoDB
{
    public static async Task<AmazonDynamoDBClient> GetClientAsync()
    {
        (string accessKey, string secretKey) = await AwsKeyHelper.GetSecret();

        var config = new AmazonDynamoDBConfig
        {
            RegionEndpoint = Amazon.RegionEndpoint.EUWest2
        };

        var credentials = new BasicAWSCredentials(accessKey, secretKey);

        return new AmazonDynamoDBClient(credentials, config);
    }
}

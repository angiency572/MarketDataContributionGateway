using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;

namespace MarketDataContributionAPI.Helpers;

public static class AwsKeyHelper
{
    public static async Task<(string accessKey, string secretKey)> GetSecret()
    {
        
        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfiguration configuration = configBuilder.Build();
        string secretName = configuration.GetSection("AWS")["SecretName"];
        string region = configuration.GetSection("AWS")["Region"];

        IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

        GetSecretValueRequest request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
        };

        GetSecretValueResponse response;

        try
        {
            response = await client.GetSecretValueAsync(request);
        }
        catch (Exception e)
        {
            // For a list of the exceptions thrown, see
            // https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
            throw e;
        }

        var secret = response.SecretString;
        var secrets = JsonConvert.DeserializeObject<Dictionary<string, string>>(secret);
        var accessKey = secrets["AccessKeyId"];
        var secretKey = secrets["SecretAccessKey"];

        return (accessKey, secretKey);
    }
}

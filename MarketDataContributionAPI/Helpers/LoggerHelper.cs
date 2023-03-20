using NLog;
using NLog.AWS.Logger;
using NLog.Config;

namespace MarketDataContributionAPI.Helpers;

public class LoggerHelper
{
    private readonly Logger _logger;

    public LoggerHelper()
    {
        _logger = LogManager.GetCurrentClassLogger();
        SetupLogger().Wait();
    }

    private async Task SetupLogger()
    {
        (string accessKey, string secretKey) = await AwsKeyHelper.GetSecret();

        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfiguration configuration = configBuilder.Build();
        string secretName = configuration.GetSection("AWS")["LogGroup"];
        string region = configuration.GetSection("AWS")["Region"];

        var config = new LoggingConfiguration();
        var awsTarget = new AWSTarget()
        {
            LogGroup = secretName,
            Region = region,
            Credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey)
        };
        config.AddTarget("aws", awsTarget);
        config.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, awsTarget));
        LogManager.Configuration = config;
    }

    public void Info(object message)
    {
        _logger.Info(message);
    }

    public void Error(Exception exception, string message)
    {
        _logger.Error(exception, message);
    }
}
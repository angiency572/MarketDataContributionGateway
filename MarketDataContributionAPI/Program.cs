using Amazon.Runtime;
using MarketDataContributionAPI.Helpers;
using MarketDataContributionAPI.Services;
using NLog;
using NLog.AWS.Logger;
using NLog.Config;

namespace MarketDataContributionAPI
{
    public class Program
    {
        private static readonly LoggerHelper _logger = new LoggerHelper();

        public static void Main(string[] args)
        {
            try
            {
                _logger.Info("Application starting up...");

                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddTransient<IMarketDataValidationService, MarketDataValidationService>();
                builder.Services.AddSingleton(_logger);

                builder.Services.AddControllers();

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                app.Run();

                _logger.Info("Application shutting down...");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An unhandled exception occurred");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
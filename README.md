# Overview
The solution, built using .NET 6 and the ASP.NET Core framework, consists of two main components:
- MarketDataContributionAPI project: A RESTful API that validates market data for foreign exchange (FX) quotes.
- MarketDataContributionAPI.Test project: A test project of the MarketDataContributionAPI project.

# Approach
- The project follows a test-driven development (TDD) approach. The MarketDataValidationServiceTests class in the MarketDataContributionAPI.Test project contains several unit tests that validate different scenarios for market data. 
- To validate market data, this project uses a service-oriented architecture. Specifically, there is a MarketDataValidationService class that contains the business logic for validating market data. This service is registered with the DI container and injected into the controller using constructor injection.
- The validation logic in the MarketDataValidationService class checks the market data against a set of rules to ensure that it is valid. If the market data is invalid, the service returns an error response. If the market data is valid, the service generates a unique identifier for the data and returns a success response with the identifier.


# Assumptions
- The API only validates FX quotes.
- The API expects market data to be in a specific format (i.e. an FxQuote object with CurrencyPair, Bid, and Ask properties).

# Other Considerations
- Unit tests were implemented using Microsoft's MSTest framework.
- Postman was used to test the API.
- The project uses NLog for logging, and logs to AWS CloudWatch using the NLog.AWS.Logger package. The logging configuration is set up in the appsettings.json file, which contains the AWS access key ID, secret access key, and region.
- The project is designed to be deployed to AWS, and assumes that the necessary AWS infrastructure (e.g. EC2 instances, VPCs, security groups) has already been set up. The project can be deployed using AWS Elastic Beanstalk, which is an easy-to-use service that can deploy .NET applications to the cloud.

# Overview
The solution, built using .NET 6 and the ASP.NET Core framework, consists of two main components:
- MarketDataContributionAPI project: A RESTful API allows users to submit and validate foreign exchange (FX) market data.
- MarketDataContributionAPI.Test project: A unit test project of the MarketDataContributionAPI project.

# Features
The Market Data Contribution API provides the following features:
- Submit FX market data (e.g. currency pair, bid and ask prices)
- Validate submitted market data based on business rules
- Store and retrieve market data from AWS DynamoDB
- Monitor API usage and errors using NLog and AWS CloudWatch

# Approach
- The project follows a test-driven development (TDD) approach. The MarketDataValidationServiceTests class in the MarketDataContributionAPI.Test project contains several unit tests that validate different scenarios for market data. 
- To validate market data, this project uses a service-oriented architecture. Specifically, there is a MarketDataValidationService class that contains the business logic for validating market data. This service is registered with the DI container and injected into the controller using constructor injection.
- The validation logic in the MarketDataValidationService class checks the market data against a set of rules to ensure that it is valid. If the market data is invalid, the service returns an error response. If the market data is valid, the service generates a unique identifier for the data and returns a success response with the identifier.

# Assumptions
- The API is intended for use by authorized users only. The HTTP requests require the authorization header which contains an encrypted API key.
- The API only validates FX quotes.
- The API expects market data to be in a specific format (i.e. an FxQuote object with CurrencyPair, Bid, and Ask properties).

# Usage
To use the Market Data Contribution API, follow these steps:
- Send an HTTP POST request to the /MarketData endpoint with an FXQuote object in the request body. The API will validate the data and return a response with a unique identifier for the FXQuote data.
- To retrieve the FXQuote data, send an HTTP GET request to the /MarketData/{currencyPair} endpoint
- The API can also be tested using Postman or another HTTP client. Import the provided Postman collection and run the tests to submit and retrieve FXQuote data.

# Other Considerations
- Unit tests were implemented using Microsoft's MSTest framework.
- The project uses NLog for logging, and logs to AWS CloudWatch using the NLog.AWS.Logger package. The logging configuration is set up in the appsettings.json file, which contains the AWS access key ID, secret access key, and region retrieved from AWS Secrets Manager.
- The project is designed to be deployed to AWS, and assumes that the necessary AWS infrastructure (e.g. EC2 instances, VPCs, security groups) has already been set up. The project can be deployed using AWS Elastic Beanstalk, which is an easy-to-use service that can deploy .NET applications to the cloud.

# Future Improvements
The following improvements could be made to the Market Data Contribution API:
- Enhance the validation logic to support additional market data validation rules.
- Add support for batch submissions and retrievals of FXQuote data.

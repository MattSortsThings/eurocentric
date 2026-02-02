# Eurocentric

**A web API for modelling and analysing the Eurovision Song Contest, 2016-present.**

## Documentation

- The [docs/launch_spec](docs/launch_spec/README.md) subdirectory contains the launch specification documents for *Eurocentric*.
- The [docs/scraped_data](docs/scraped_data/README.md) subdirectory contains country and contest data scraped from the official Eurovision website.

## AI declaration

**No AI-generated source code or test code has been used in this project.**

I have made the following uses of Microsoft Copilot:

1. Proofreading the launch specification documents for spelling and formatting errors and inconsistencies between documents.
2. Providing step-by-step instructions for deploying the web application to the Azure Container Apps service using a GitHub action.
3. Asking for an elaboration and example of the [logging-to-database background service](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-10.0#no-asynchronous-logger-methods) recommended in Microsoft's ASP.NET core documentation.
4. Generating the scraped data [JSON schemas](docs/scraped_data/README.md).

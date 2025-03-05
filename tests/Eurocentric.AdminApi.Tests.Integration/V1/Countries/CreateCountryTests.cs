using ErrorOr;
using Eurocentric.AdminApi.Tests.Integration.Utils;
using Eurocentric.AdminApi.Tests.Integration.Utils.Assertions;
using Eurocentric.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.AdminApi.V1.Countries.Models;

namespace Eurocentric.AdminApi.Tests.Integration.V1.Countries;

public static class CreateCountryTests
{
    private static void ShouldBeCorrectlyCreatedFrom(this Country country, CreateCountryCommand command)
    {
        Assert.Equal(command.CountryCode, country.CountryCode);
        Assert.Equal(command.CountryName, country.CountryName);
        Assert.Equal(command.CountryType, country.CountryType);
        Assert.Empty(country.ContestIds);
    }

    public sealed class AppPipeline(CleanWebAppFixture fixture) : IntegrationTest(fixture)
    {
        private static CreateCountryCommand CreateCountryCommand => new()
        {
            CountryCode = "AA", CountryName = "CountryName", CountryType = CountryType.Real
        };

        [Fact]
        public async Task Should_return_created_country_given_valid_real_country_command()
        {
            // Arrange
            CreateCountryCommand command = new()
            {
                CountryCode = "GB", CountryName = "United Kingdom", CountryType = CountryType.Real
            };

            // Act
            ErrorOr<CreateCountryResult> errorsOrResult = await SendAsync(command);

            // Assert
            (bool isError, CreateCountryResult result) = errorsOrResult.ParseAsSuccess();

            isError.ShouldBeFalse();

            result.Country.ShouldBeCorrectlyCreatedFrom(command);
        }

        [Fact]
        public async Task Should_return_created_country_given_valid_pseudo_country_command()
        {
            // Arrange
            CreateCountryCommand command = new()
            {
                CountryCode = "XX", CountryName = "Rest of the World", CountryType = CountryType.Pseudo
            };

            // Act
            ErrorOr<CreateCountryResult> errorsOrResult = await SendAsync(command);

            // Assert
            (bool isError, CreateCountryResult result) = errorsOrResult.ParseAsSuccess();

            isError.ShouldBeFalse();

            result.Country.ShouldBeCorrectlyCreatedFrom(command);
        }

        [Fact]
        public async Task Should_return_errors_given_command_with_duplicate_country_code()
        {
            // Arrange
            const string sharedCountryCode = "XX";

            await CreateCountryWithCountryCodeAsync(sharedCountryCode);

            CreateCountryCommand command = CreateCountryCommand with { CountryCode = sharedCountryCode };

            // Act
            ErrorOr<CreateCountryResult> errorsOrResult = await SendAsync(command);

            // Assert
            var (isError, firstError) = errorsOrResult.ParseAsError();

            isError.ShouldBeTrue();

            firstError.ShouldHaveConflictErrorType();
            firstError.ShouldHaveCode("Country code conflict");
            firstError.ShouldHaveDescription("A country already exists with the specified country code value.");
            firstError.ShouldHaveMetadata("countryCode", sharedCountryCode);
        }

        [Fact]
        public async Task Should_return_errors_given_command_with_invalid_country_code()
        {
            // Arrange
            const string invalidCountryCode = "";

            CreateCountryCommand command = CreateCountryCommand with { CountryCode = invalidCountryCode };

            // Act
            ErrorOr<CreateCountryResult> errorsOrResult = await SendAsync(command);

            // Assert
            var (isError, firstError) = errorsOrResult.ParseAsError();

            isError.ShouldBeTrue();

            firstError.ShouldHaveFailureErrorType();
            firstError.ShouldHaveCode("Invalid country code");
            firstError.ShouldHaveDescription("Country code value must be a string of 2 upper-case letters.");
            firstError.ShouldHaveMetadata("countryCode", invalidCountryCode);
        }

        [Fact]
        public async Task Should_return_errors_given_command_with_invalid_country_name()
        {
            // Arrange
            const string invalidCountryName = "";

            CreateCountryCommand command = CreateCountryCommand with { CountryName = invalidCountryName };

            // Act
            ErrorOr<CreateCountryResult> errorsOrResult = await SendAsync(command);

            // Assert
            var (isError, firstError) = errorsOrResult.ParseAsError();

            isError.ShouldBeTrue();

            firstError.ShouldHaveFailureErrorType();
            firstError.ShouldHaveCode("Invalid country name");
            firstError.ShouldHaveDescription("Country name value must be a non-empty, non-white-space string " +
                                             "of no more than 200 characters.");
            firstError.ShouldHaveMetadata("countryName", invalidCountryName);
        }

        private async Task CreateCountryWithCountryCodeAsync(string countryCode) =>
            await SendAsync(CreateCountryCommand with { CountryCode = countryCode });
    }
}

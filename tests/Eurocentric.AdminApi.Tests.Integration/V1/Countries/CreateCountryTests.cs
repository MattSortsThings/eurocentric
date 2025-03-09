using ErrorOr;
using Eurocentric.AdminApi.Tests.Integration.Utils;
using Eurocentric.AdminApi.Tests.Utils.V1.Assertions;
using Eurocentric.AdminApi.Tests.Utils.V1.SampleData;
using Eurocentric.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.Tests.Assertions;

namespace Eurocentric.AdminApi.Tests.Integration.V1.Countries;

public static class CreateCountryTests
{
    public sealed class AppPipeline(CleanWebAppFixture fixture) : IntegrationTest(fixture)
    {
        [Fact]
        public async Task Should_return_created_country_given_valid_real_country_command()
        {
            // Arrange
            CreateCountryCommand command = TestCommands.CreateRealCountry();

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
            CreateCountryCommand command = TestCommands.CreatePseudoCountry();

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
            string sharedCountryCode = await CreateCountryAndReturnItsCountryCodeAsync();

            CreateCountryCommand command = TestCommands.CreateRealCountry() with { CountryCode = sharedCountryCode };

            // Act
            ErrorOr<CreateCountryResult> errorsOrResult = await SendAsync(command);

            // Assert
            var (isError, firstError) = errorsOrResult.ParseAsError();

            isError.ShouldBeTrue();

            firstError.ShouldHaveErrorType(ErrorType.Conflict);
            firstError.ShouldHaveCode("Country code conflict");
            firstError.ShouldHaveDescription("A country already exists with the specified country code value.");
            firstError.ShouldHaveMetadata("countryCode", sharedCountryCode);
        }

        [Fact]
        public async Task Should_return_errors_given_command_with_invalid_country_code()
        {
            // Arrange
            const string invalidCountryCode = "";

            CreateCountryCommand command = TestCommands.CreateRealCountry() with { CountryCode = invalidCountryCode };

            // Act
            ErrorOr<CreateCountryResult> errorsOrResult = await SendAsync(command);

            // Assert
            var (isError, firstError) = errorsOrResult.ParseAsError();

            isError.ShouldBeTrue();

            firstError.ShouldHaveErrorType(ErrorType.Failure);
            firstError.ShouldHaveCode("Invalid country code");
            firstError.ShouldHaveDescription("Country code value must be a string of 2 upper-case letters.");
            firstError.ShouldHaveMetadata("countryCode", invalidCountryCode);
        }

        [Fact]
        public async Task Should_return_errors_given_command_with_invalid_country_name()
        {
            // Arrange
            const string invalidCountryName = "";

            CreateCountryCommand command = TestCommands.CreateRealCountry() with { CountryName = invalidCountryName };

            // Act
            ErrorOr<CreateCountryResult> errorsOrResult = await SendAsync(command);

            // Assert
            (bool isError, Error firstError) = errorsOrResult.ParseAsError();

            isError.ShouldBeTrue();

            firstError.ShouldHaveErrorType(ErrorType.Failure);
            firstError.ShouldHaveCode("Invalid country name");
            firstError.ShouldHaveDescription("Country name value must be a non-empty, non-white-space string " +
                                             "of no more than 200 characters.");
            firstError.ShouldHaveMetadata("countryName", invalidCountryName);
        }

        private async Task<string> CreateCountryAndReturnItsCountryCodeAsync()
        {
            ErrorOr<CreateCountryResult> errorsOrResult = await SendAsync(TestCommands.CreatePseudoCountry());

            return errorsOrResult.Value.Country.CountryCode;
        }
    }
}

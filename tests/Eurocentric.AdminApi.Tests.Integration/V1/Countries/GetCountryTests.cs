using ErrorOr;
using Eurocentric.AdminApi.Tests.Integration.Utils;
using Eurocentric.AdminApi.Tests.Integration.Utils.Assertions;
using Eurocentric.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.AdminApi.V1.Countries.GetCountry;
using Eurocentric.AdminApi.V1.Models;

namespace Eurocentric.AdminApi.Tests.Integration.V1.Countries;

public static class GetCountryTests
{
    private static void ShouldBeEquivalentTo(this Country subject, Country expected) => Assert.Equivalent(expected, subject);

    public sealed class AppPipeline(CleanWebAppFixture fixture) : IntegrationTest(fixture)
    {
        [Fact]
        public async Task Should_return_requested_country_when_existing_country_queried()
        {
            // Arrange
            Country targetCountry = await CreateCountryAsync();

            GetCountryQuery query = new(targetCountry.Id);

            // Act
            ErrorOr<GetCountryResult> errorsOrResult = await SendAsync(query);

            // Assert
            (bool isError, GetCountryResult result) = errorsOrResult.ParseAsSuccess();

            isError.ShouldBeFalse();

            result.Country.ShouldBeEquivalentTo(targetCountry);
        }

        [Fact]
        public async Task Should_return_errors_when_no_country_matches_query()
        {
            // Arrange
            Guid countryId = Guid.Parse("62eae27d-0609-4ef9-9953-ec918abe015f");

            GetCountryQuery query = new(countryId);

            // Act
            ErrorOr<GetCountryResult> errorsOrResult = await SendAsync(query);

            // Assert
            (bool isError, Error firstError) = errorsOrResult.ParseAsError();

            isError.ShouldBeTrue();

            firstError.ShouldHaveErrorType(ErrorType.NotFound);
            firstError.ShouldHaveCode("Country not found");
            firstError.ShouldHaveDescription("No country exists with the specified ID.");
            firstError.ShouldHaveMetadata("countryId", countryId);
        }

        private async Task<Country> CreateCountryAsync()
        {
            CreateCountryCommand command = new()
            {
                CountryCode = "GB", CountryName = "United Kingdom", CountryType = CountryType.Real
            };

            ErrorOr<CreateCountryResult> errorsOrResult = await SendAsync(command);

            return errorsOrResult.Value.Country;
        }
    }
}

using ErrorOr;
using Eurocentric.AdminApi.Tests.Integration.Utils;
using Eurocentric.AdminApi.Tests.Utils.V1.Assertions;
using Eurocentric.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.AdminApi.V1.Countries.GetCountries;
using Eurocentric.AdminApi.V1.Models;
using Eurocentric.Tests.Assertions;

namespace Eurocentric.AdminApi.Tests.Integration.V1.Countries;

public static class GetCountriesTests
{
    public sealed class AppPipeline(CleanWebAppFixture fixture) : IntegrationTest(fixture)
    {
        [Fact]
        public async Task Should_return_empty_list_when_no_countries_exist()
        {
            // Arrange
            GetCountriesQuery query = new();

            // Act
            ErrorOr<GetCountriesResult> errorsOrResult = await SendAsync(query);

            // Assert
            (bool isError, GetCountriesResult result) = errorsOrResult.ParseAsSuccess();

            isError.ShouldBeFalse();

            result.Countries.ShouldBeEmpty();
        }

        [Fact]
        public async Task Should_return_list_of_all_existing_countries_in_country_code_order()
        {
            // Arrange
            Country eeCountry = await CreateCountryAsync("EE", "Estonia");
            Country czCountry = await CreateCountryAsync("CZ", "Czechia");
            Country chCountry = await CreateCountryAsync("CH", "Switzerland");
            Country gbCountry = await CreateCountryAsync("GB", "United Kingdom");

            Country[] expectedCountries = [chCountry, czCountry, eeCountry, gbCountry];

            GetCountriesQuery query = new();

            // Act
            ErrorOr<GetCountriesResult> errorsOrResult = await SendAsync(query);

            // Assert
            (bool isError, GetCountriesResult result) = errorsOrResult.ParseAsSuccess();

            isError.ShouldBeFalse();

            result.Countries.ShouldBe(expectedCountries);
        }

        private async Task<Country> CreateCountryAsync(string countryCode, string countryName)
        {
            CreateCountryCommand command = new()
            {
                CountryCode = countryCode, CountryName = countryName, CountryType = CountryType.Real
            };

            ErrorOr<CreateCountryResult> errorsOrResult = await SendAsync(command);

            return errorsOrResult.Value.Country;
        }
    }
}

using System.Net;
using Eurocentric.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.AdminApi.V1.Countries.GetCountries;
using Eurocentric.AdminApi.V1.Countries.Models;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.AdminApi.V1.Countries;

public static class GetCountriesTests
{
    private const string Route = "admin/api/v1.0/countries";

    private static void ShouldBeSequenceEqualTo(this Country[] subject, Country[] expected) =>
        Assert.Equal(subject, expected, (c1, c2) => c1.Id == c2.Id
                                                    && c1.CountryCode == c2.CountryCode
                                                    && c1.CountryName == c2.CountryName
                                                    && c1.CountryType == c2.CountryType
                                                    && c1.ContestIds.SequenceEqual(c2.ContestIds));

    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_200_with_empty_list_when_no_countries_exist()
        {
            // Arrange
            RestRequest request = Get(Route).UseAdminApiKey();

            // Act
            RestResponse<GetCountriesResult> response = await SendAsync<GetCountriesResult>(request);

            // Assert
            (HttpStatusCode statusCode, GetCountriesResult result) = response;

            statusCode.ShouldBe(HttpStatusCode.OK);
            result.Countries.ShouldBeEmpty();
        }

        [Fact]
        public async Task Should_return_200_with_all_existing_countries_in_country_code_order()
        {
            // Arrange
            Country eeCountry = await CreateCountryAsync("EE", "Estonia");
            Country czCountry = await CreateCountryAsync("CZ", "Czechia");
            Country chCountry = await CreateCountryAsync("CH", "Switzerland");
            Country gbCountry = await CreateCountryAsync("GB", "United Kingdom");

            Country[] expectedCountries = [chCountry, czCountry, eeCountry, gbCountry];

            RestRequest request = Get(Route).UseAdminApiKey();

            // Act
            RestResponse<GetCountriesResult> response = await SendAsync<GetCountriesResult>(request);

            // Assert
            (HttpStatusCode statusCode, GetCountriesResult result) = response;

            statusCode.ShouldBe(HttpStatusCode.OK);

            result.Countries.ShouldBeSequenceEqualTo(expectedCountries);
        }

        private async Task<Country> CreateCountryAsync(string countryCode, string countryName)
        {
            RestResponse<CreateCountryResult> response = await SendAsync<CreateCountryResult>(Post(Route)
                .UseAdminApiKey()
                .AddJsonBody(new CreateCountryCommand
                {
                    CountryCode = countryCode, CountryName = countryName, CountryType = CountryType.Real
                }));

            return response.Data!.Country;
        }
    }
}

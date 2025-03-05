using System.Net;
using Eurocentric.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.AdminApi.V1.Countries.GetCountry;
using Eurocentric.AdminApi.V1.Countries.Models;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.AdminApi.V1.Countries;

public static class GetCountryTests
{
    private const string Route = "/admin/api/v1.0/countries";

    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_200_with_placeholder_country_given_any_request()
        {
            // Arrange
            Country country = await CreateCountryAsync();

            RestRequest request = Get($"{Route}/{country.Id}").UseAdminApiKey();

            // Act
            (HttpStatusCode statusCode, GetCountryResult result) = await SendAsync<GetCountryResult>(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.OK),
                () => Assert.Equivalent(country, result.Country)
            );
        }

        private async Task<Country> CreateCountryAsync()
        {
            CreateCountryCommand command = new()
            {
                CountryCode = "GB", CountryName = "United Kingdom", CountryType = CountryType.Real
            };

            RestRequest request = Post(Route).UseAdminApiKey().AddJsonBody(command);

            var (_, result) = await SendAsync<CreateCountryResult>(request);

            return result.Country;
        }
    }
}

using System.Net;
using Eurocentric.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.AdminApi.V1.Countries.GetCountry;
using Eurocentric.AdminApi.V1.Countries.Models;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.AdminApi.V1.Countries;

public static class GetCountryTests
{
    private const string Route = "/admin/api/v1.0/countries";

    private static void ShouldBeEquivalentTo(this Country subject, Country expected) => Assert.Equivalent(expected, subject);

    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_200_with_requested_country_when_existing_country_requested()
        {
            // Arrange
            Country targetCountry = await CreateCountryAsync();

            RestRequest request = Get($"{Route}/{targetCountry.Id}").UseAdminApiKey();

            // Act
            RestResponse<GetCountryResult> response = await SendAsync<GetCountryResult>(request);

            // Assert
            (HttpStatusCode statusCode, GetCountryResult result) = response;

            statusCode.ShouldBe(HttpStatusCode.OK);

            result.Country.ShouldBeEquivalentTo(targetCountry);
        }

        [Fact]
        public async Task Should_return_404_with_ProblemDetails_when_no_country_matches_request()
        {
            // Arrange
            Guid countryId = Guid.Parse("62eae27d-0609-4ef9-9953-ec918abe015f");

            RestRequest request = Get($"{Route}/{countryId}").UseAdminApiKey();

            // Act
            RestResponse<ProblemDetails> response = await SendAsync<ProblemDetails>(request);

            // Assert
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = response;

            statusCode.ShouldBe(HttpStatusCode.NotFound);

            problemDetails.ShouldHaveStatus(StatusCodes.Status404NotFound);
            problemDetails.ShouldHaveTitle("Country not found");
            problemDetails.ShouldHaveDetail("No country exists with the specified ID.");
            problemDetails.ShouldHaveInstance($"GET {Route}/{countryId}");
            problemDetails.ShouldHaveExtension("countryId", countryId);
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

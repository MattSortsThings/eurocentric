using System.Net;
using Eurocentric.AdminApi.Tests.Utils.V1.Assertions;
using Eurocentric.AdminApi.Tests.Utils.V1.SampleData;
using Eurocentric.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.Tests.Assertions;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.AdminApi.V1.Countries;

public static class CreateCountryTests
{
    private const string Route = "/admin/api/v1.0/countries";

    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_201_with_created_country_and_location_given_valid_real_country_request()
        {
            // Arrange
            CreateCountryCommand command = TestCommands.CreateRealCountry();

            RestRequest request = Post(Route).UseAdminApiKey().AddJsonBody(command);

            // Act
            RestResponse<CreateCountryResult> response = await SendAsync<CreateCountryResult>(request);

            // Assert
            (HttpStatusCode statusCode, CreateCountryResult result, string location) = response;

            statusCode.ShouldBe(HttpStatusCode.Created);

            result.Country.ShouldBeCorrectlyCreatedFrom(command);

            location.ShouldEndWith($"{Route}/{result.Country.Id}");
        }

        [Fact]
        public async Task Should_return_201_with_created_country_and_location_given_valid_pseudo_country_request()
        {
            // Arrange
            CreateCountryCommand command = TestCommands.CreatePseudoCountry();

            RestRequest request = Post(Route).UseAdminApiKey().AddJsonBody(command);

            // Act
            RestResponse<CreateCountryResult> response = await SendAsync<CreateCountryResult>(request);

            // Assert
            (HttpStatusCode statusCode, CreateCountryResult result, string location) = response;

            statusCode.ShouldBe(HttpStatusCode.Created);

            result.Country.ShouldBeCorrectlyCreatedFrom(command);

            location.ShouldEndWith($"{Route}/{result.Country.Id}");
        }

        [Fact]
        public async Task Should_return_404_with_ProblemDetails_given_malformed_request()
        {
            // Arrange
            RestRequest request = Post(Route).UseAdminApiKey().AddJsonBody("""{ "this": "is_malformed" }""");

            // Act
            RestResponse<ProblemDetails> response = await SendAsync<ProblemDetails>(request);

            // Assert
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = response;

            statusCode.ShouldBe(HttpStatusCode.BadRequest);

            problemDetails.ShouldHaveStatus(StatusCodes.Status400BadRequest);
            problemDetails.ShouldHaveTitle("Bad HTTP request");
            problemDetails.ShouldHaveDetail("Failed to read parameter \"CreateCountryCommand command\" " +
                                            "from the request body as JSON.");
            problemDetails.ShouldHaveInstance($"POST {Route}");
        }

        [Fact]
        public async Task Should_return_409_with_ProblemDetails_given_request_with_duplicate_country_code()
        {
            // Arrange
            string sharedCountryCode = await CreateCountryAndReturnItsCountryCodeAsync();

            CreateCountryCommand command = TestCommands.CreateRealCountry() with { CountryCode = sharedCountryCode };

            RestRequest request = Post(Route).UseAdminApiKey().AddJsonBody(command);

            // Act
            RestResponse<ProblemDetails> response = await SendAsync<ProblemDetails>(request);

            // Assert
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = response;

            statusCode.ShouldBe(HttpStatusCode.Conflict);

            problemDetails.ShouldHaveStatus(StatusCodes.Status409Conflict);
            problemDetails.ShouldHaveTitle("Country code conflict");
            problemDetails.ShouldHaveDetail("A country already exists with the specified country code value.");
            problemDetails.ShouldHaveInstance($"POST {Route}");
            problemDetails.ShouldHaveExtension("countryCode", sharedCountryCode);
        }

        [Fact]
        public async Task Should_return_422_with_ProblemDetails_given_request_with_invalid_country_code()
        {
            // Arrange
            const string invalidCountryCode = "";

            CreateCountryCommand command = TestCommands.CreateRealCountry() with { CountryCode = invalidCountryCode };

            RestRequest request = Post(Route).UseAdminApiKey().AddJsonBody(command);

            // Act
            RestResponse<ProblemDetails> response = await SendAsync<ProblemDetails>(request);

            // Assert
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = response;

            statusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

            problemDetails.ShouldHaveStatus(StatusCodes.Status422UnprocessableEntity);
            problemDetails.ShouldHaveTitle("Invalid country code");
            problemDetails.ShouldHaveDetail("Country code value must be a string of 2 upper-case letters.");
            problemDetails.ShouldHaveInstance($"POST {Route}");
            problemDetails.ShouldHaveExtension("countryCode", invalidCountryCode);
        }

        [Fact]
        public async Task Should_return_422_with_ProblemDetails_given_request_with_invalid_country_name()
        {
            // Arrange
            const string invalidCountryName = "";

            CreateCountryCommand command = TestCommands.CreateRealCountry() with { CountryName = invalidCountryName };

            RestRequest request = Post(Route).UseAdminApiKey().AddJsonBody(command);

            // Act
            RestResponse<ProblemDetails> response = await SendAsync<ProblemDetails>(request);

            // Assert
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = response;

            statusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

            problemDetails.ShouldHaveStatus(StatusCodes.Status422UnprocessableEntity);
            problemDetails.ShouldHaveTitle("Invalid country name");
            problemDetails.ShouldHaveDetail("Country name value must be a non-empty, non-white-space string " +
                                            "of no more than 200 characters.");
            problemDetails.ShouldHaveInstance($"POST {Route}");
            problemDetails.ShouldHaveExtension("countryName", invalidCountryName);
        }

        private async Task<string> CreateCountryAndReturnItsCountryCodeAsync()
        {
            RestRequest request = Post(Route).UseAdminApiKey().AddJsonBody(TestCommands.CreatePseudoCountry());

            RestResponse<CreateCountryResult> response = await SendAsync<CreateCountryResult>(request);

            return response.Data!.Country.CountryCode;
        }
    }
}

using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Responses;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public static class GetCountryTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_retrieve_requested_country(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
            admin.Given_I_want_to_retrieve_my_country();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_country_should_be_my_country();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_non_existent_country_requested(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
            await admin.Given_I_have_deleted_my_country();
            admin.Given_I_want_to_retrieve_my_country();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(404);
            admin.Then_the_response_problem_details_should_match(status: 404,
                title: "Country not found",
                detail: "No country exists with the provided country ID.");
            admin.Then_the_response_problem_details_should_have_a_countryId_extension_with_my_country_ID();
        }
    }

    private sealed class Admin : AdminActorWithResponse<GetCountryResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, IRequestFactory requestFactory) :
            base(restClient, backDoor, requestFactory)
        {
        }

        public void Given_I_want_to_retrieve_my_country()
        {
            Country myCountry = GivenCountries.GetSingle();

            Request = RequestFactory.Countries.GetCountry(myCountry.Id);
        }

        public void Then_the_retrieved_country_should_be_my_country()
        {
            Assert.NotNull(ResponseObject);

            Country expectedCountry = GivenCountries.GetSingle();
            Country retrievedCountry = ResponseObject.Country;

            Assert.Equal(expectedCountry, retrievedCountry, new CountryEqualityComparer());
        }

        public void Then_the_response_problem_details_should_have_a_countryId_extension_with_my_country_ID()
        {
            Assert.NotNull(ResponseProblemDetails);

            Guid expectedCountryId = GivenCountries.GetSingle().Id;

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "countryId", Value: JsonElement je }
                                                                      && je.GetGuid() == expectedCountryId);
        }
    }
}

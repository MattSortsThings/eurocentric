using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.PublicApi.V0.Filters;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Filters;

public static class GetCountriesTests
{
    public sealed class Feature(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v0.1")]
        [InlineData("v0.2")]
        public async Task Should_retrieve_all_queryable_countries_in_country_code_order(string apiVersion)
        {
            EuroFan euroFan = new(BackDoor, RestClient, apiVersion);

            // Given
            await euroFan.Given_the_system_is_populated_with_the_sample_queryable_data();
            euroFan.Given_I_want_to_retrieve_all_queryable_countries();

            // When
            await euroFan.When_I_send_my_request();

            // Then
            euroFan.Then_my_request_should_succeed_with_status_code(200);
            euroFan.Then_the_retrieved_countries_should_be(
                """
                | CountryCode | CountryName    |
                |:------------|:---------------|
                | AT          | Austria        |
                | BE          | Belgium        |
                | CZ          | Czechia        |
                | DK          | Denmark        |
                | EE          | Estonia        |
                | FI          | Finland        |
                | GE          | Georgia        |
                """);
        }

        [Theory]
        [InlineData("v0.1")]
        [InlineData("v0.2")]
        public async Task Should_retrieve_empty_list_when_no_queryable_data_exists(string apiVersion)
        {
            EuroFan euroFan = new(BackDoor, RestClient, apiVersion);

            // Given
            euroFan.Given_I_want_to_retrieve_all_queryable_countries();

            // When
            await euroFan.When_I_send_my_request();

            // Then
            euroFan.Then_my_request_should_succeed_with_status_code(200);
            euroFan.Then_the_retrieved_countries_should_be_an_empty_list();
        }
    }

    private sealed class EuroFan : ActorWithResponse<GetCountriesResponse>
    {
        public EuroFan(IWebAppFixtureBackDoor backDoor, IWebAppFixtureRestClient restClient, string apiVersion = "v1.0") :
            base(backDoor, restClient, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_all_queryable_countries()
        {
            Request = new RestRequest("/public/api/{apiVersion}/filters/countries");
            Request.AddUrlSegment("apiVersion", ApiVersion);
        }

        public void Then_the_retrieved_countries_should_be(string markdownTable)
        {
            Assert.NotNull(ResponseObject);

            Country[] expectedCountries = markdownTable.ParseAll(MapToCountry);
            Assert.Equal(expectedCountries, ResponseObject.Countries);
        }

        public void Then_the_retrieved_countries_should_be_an_empty_list()
        {
            Assert.NotNull(ResponseObject);

            Assert.Empty(ResponseObject.Countries);
        }

        private static Country MapToCountry(Dictionary<string, string> row) => new(row["CountryCode"], row["CountryName"]);
    }
}

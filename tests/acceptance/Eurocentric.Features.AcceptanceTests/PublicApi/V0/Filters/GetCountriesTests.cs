using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils.Mixins.SampleData;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.PublicApi.V0.Filters;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Filters;

public static class GetCountriesTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v0.1")]
        [InlineData("v0.2")]
        public async Task Should_retrieve_all_queryable_countries(string apiVersion)
        {
            EuroFan euroFan = new(RestClient, BackDoor, apiVersion);

            // Given
            await euroFan.Given_the_system_is_populated_with_the_sample_countries();
            euroFan.Given_I_want_to_retrieve_all_queryable_countries();

            // When
            await euroFan.When_I_send_my_request();

            // Then
            euroFan.Then_my_request_should_succeed_with_status_code(200);
            euroFan.Then_the_retrieved_queryable_countries_should_be(
                """
                | CountryCode | CountryName |
                |:------------|:------------|
                | AT          | Austria     |
                | BE          | Belgium     |
                | CZ          | Czechia     |
                | DK          | Denmark     |
                | EE          | Estonia     |
                | FI          | Finland     |
                | GE          | Georgia     |
                """);
        }

        [Theory]
        [InlineData("v0.1")]
        [InlineData("v0.2")]
        public async Task Should_retrieve_empty_list_when_no_queryable_data(string apiVersion)
        {
            EuroFan euroFan = new(RestClient, BackDoor, apiVersion);

            // Given
            euroFan.Given_I_want_to_retrieve_all_queryable_countries();

            // When
            await euroFan.When_I_send_my_request();

            // Then
            euroFan.Then_my_request_should_succeed_with_status_code(200);
            euroFan.Then_the_retrieved_queryable_countries_should_be_an_empty_list();
        }
    }

    private sealed class EuroFan : EuroFanActor<GetCountriesResponse>
    {
        public EuroFan(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_all_queryable_countries() => Request = RequestFactory.Filters.GetCountries();

        public void Then_the_retrieved_queryable_countries_should_be(string markdownTable)
        {
            Assert.NotNull(ResponseObject);

            Country[] expectedCountries = MarkdownParser.ParseTable(markdownTable, RowMapper);
            Country[] actualCountries = ResponseObject.Countries;

            Assert.Equal(expectedCountries, actualCountries);
        }

        public void Then_the_retrieved_queryable_countries_should_be_an_empty_list()
        {
            Assert.NotNull(ResponseObject);

            Assert.Empty(ResponseObject.Countries);
        }

        private static Country RowMapper(Dictionary<string, string> row) => new(row["CountryCode"], row["CountryName"]);
    }
}

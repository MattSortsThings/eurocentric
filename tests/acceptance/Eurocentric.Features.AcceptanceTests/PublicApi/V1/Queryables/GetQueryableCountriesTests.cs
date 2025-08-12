using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableCountries;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Queryables;

public sealed class GetQueryableCountriesTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_all_queryable_countries_in_country_code_order(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_queryable_countries();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_retrieved_queryable_countries_should_be(
            """
            | CountryCode | CountryName       |
            |-------------|-------------------|
            | AL          | Albania           |
            | AM          | Armenia           |
            | AT          | Austria           |
            | AU          | Australia         |
            | AZ          | Azerbaijan        |
            | BE          | Belgium           |
            | BG          | Bulgaria          |
            | CH          | Switzerland       |
            | CY          | Cyprus            |
            | CZ          | Czechia           |
            | DE          | Germany           |
            | DK          | Denmark           |
            | EE          | Estonia           |
            | ES          | Spain             |
            | FI          | Finland           |
            | FR          | France            |
            | GB          | United Kingdom    |
            | GE          | Georgia           |
            | GR          | Greece            |
            | HR          | Croatia           |
            | IE          | Ireland           |
            | IL          | Israel            |
            | IS          | Iceland           |
            | IT          | Italy             |
            | LT          | Lithuania         |
            | LV          | Latvia            |
            | MD          | Moldova           |
            | ME          | Montenegro        |
            | MK          | North Macedonia   |
            | MT          | Malta             |
            | NL          | Netherlands       |
            | NO          | Norway            |
            | PL          | Poland            |
            | PT          | Portugal          |
            | RO          | Romania           |
            | RS          | Serbia            |
            | SE          | Sweden            |
            | SI          | Slovenia          |
            | SM          | San Marino        |
            | UA          | Ukraine           |
            | XX          | Rest of the World |
            """);
    }

    private sealed class EuroFanActor(IApiDriver apiDriver) : EuroFanActorWithResponse<GetQueryableCountriesResponse>(apiDriver)
    {
        public void Given_I_want_to_retrieve_all_queryable_countries() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableCountries();

        public async Task Then_the_retrieved_queryable_countries_should_be(string countries)
        {
            GetQueryableCountriesResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            QueryableCountry[] expectedQueryableCountries =
                MarkdownParser.ParseTable(countries, MapToQueryableCountry).ToArray();

            await Assert.That(responseBody.QueryableCountries)
                .IsEquivalentTo(expectedQueryableCountries, CollectionOrdering.Matching);
        }

        private QueryableCountry MapToQueryableCountry(Dictionary<string, string> row) => new()
        {
            CountryCode = row["CountryCode"], CountryName = row["CountryName"]
        };
    }
}

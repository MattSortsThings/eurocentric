using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.Queryables;
using Eurocentric.Apis.Public.V1.Features.Queryables;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.Queryables;

[Category("public-api")]
public sealed class GetQueryableCountriesTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_all_queryable_countries_in_country_code_order(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_queryable_countries_in_order_should_be(
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
            """
        );
    }

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetQueryableCountriesResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_all_existing_countries() =>
            Request = Kernel.Requests.Queryables.GetQueryableCountries();

        public async Task Then_the_retrieved_queryable_countries_in_order_should_be(string table)
        {
            QueryableCountry[] expectedItems = MarkdownParser.ParseTable(table, MapToQueryableCountry);

            await Assert
                .That(SuccessResponse?.Data?.QueryableCountries)
                .IsOrderedBy(qc => qc.CountryCode)
                .IsEquivalentTo(expectedItems, new EqualityComparer(), CollectionOrdering.Matching);
        }

        private static QueryableCountry MapToQueryableCountry(Dictionary<string, string> row) =>
            new() { CountryCode = row["CountryCode"], CountryName = row["CountryName"] };

        private sealed class EqualityComparer : IEqualityComparer<QueryableCountry>
        {
            public bool Equals(QueryableCountry? x, QueryableCountry? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x is null)
                {
                    return false;
                }

                if (y is null)
                {
                    return false;
                }

                if (x.GetType() != y.GetType())
                {
                    return false;
                }

                return x.CountryCode.Equals(y.CountryCode, StringComparison.Ordinal)
                    && x.CountryName.Equals(y.CountryName, StringComparison.Ordinal);
            }

            public int GetHashCode(QueryableCountry obj) => HashCode.Combine(obj.CountryCode, obj.CountryName);
        }
    }
}

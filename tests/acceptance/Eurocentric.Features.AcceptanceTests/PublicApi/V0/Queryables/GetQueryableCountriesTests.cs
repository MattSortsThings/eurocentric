using Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils.DataSourceGenerators;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableCountries;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Queryables;

public sealed class GetQueryableCountriesTests : ParallelSeededAcceptanceTest
{
    [Test]
    [PublicApiV0Point1AndUp]
    public async Task Endpoint_should_retrieve_all_queryable_countries_in_country_code_order(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_queryable_countries();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_retrieved_countries_should_be_in_country_code_order_and_have_count(41);
    }

    private sealed class EuroFanActor(IApiDriver apiDriver) : EuroFanActorWithResponse<GetQueryableCountriesResponse>(apiDriver)
    {
        public void Given_I_want_to_retrieve_all_queryable_countries() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableCountries();

        public async Task Then_the_retrieved_countries_should_be_in_country_code_order_and_have_count(int count)
        {
            GetQueryableCountriesResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            QueryableCountry[] countries = responseBody.QueryableCountries;

            await Assert.That(countries).IsInOrder(new CountryCodeComparer()).And.HasCount(count);
        }
    }

    private sealed class CountryCodeComparer : IComparer<QueryableCountry>
    {
        public int Compare(QueryableCountry? x, QueryableCountry? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (y is null)
            {
                return 1;
            }

            if (x is null)
            {
                return -1;
            }

            return string.Compare(x.CountryCode, y.CountryCode, StringComparison.Ordinal);
        }
    }
}

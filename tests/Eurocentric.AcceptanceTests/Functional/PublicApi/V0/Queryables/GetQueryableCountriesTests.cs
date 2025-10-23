using Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V0.Features.Queryables;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V0.Queryables;

[Category("public-api")]
public sealed class GetQueryableCountriesTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_retrieve_all_queryable_countries_in_country_code_order(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_the_queryable_countries();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_queryable_countries_should_have_count(41);
        await euroFan.Then_the_retrieved_queryable_countries_should_be_in_country_code_order();
    }

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetQueryableCountriesResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_all_the_queryable_countries() =>
            Request = Kernel.Requests.Queryables.GetQueryableCountries();

        public async Task Then_the_retrieved_queryable_countries_should_have_count(int count)
        {
            GetQueryableCountriesResponse response = await Assert.That(SuccessResponse?.Data).IsNotNull();

            await Assert.That(response.QueryableCountries).HasCount(count);
        }

        public async Task Then_the_retrieved_queryable_countries_should_be_in_country_code_order()
        {
            GetQueryableCountriesResponse response = await Assert.That(SuccessResponse?.Data).IsNotNull();

            await Assert.That(response.QueryableCountries).IsOrderedBy(country => country.CountryCode);
        }
    }
}

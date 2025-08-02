using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableCountries;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Queryables;

public static class GetQueryableCountriesTests
{
    public sealed class GetQueryableCountriesFeature : AcceptanceTest
    {
        [Test]
        [Arguments("v0.1")]
        [Arguments("v0.2")]
        public async Task Should_retrieve_all_queryable_countries_in_country_code_order(string apiVersion)
        {
            EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

            // Given
            euroFan.Given_I_want_to_retrieve_all_queryable_countries();

            // When
            await euroFan.When_I_send_my_request();

            // Then
            await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
            await euroFan.Then_the_retrieved_queryable_countries_should_be_a_non_empty_list_ordered_by_country_code();
        }
    }

    private sealed class EuroFanActor : EuroFanActorWithResponse<GetQueryableCountriesResponse>
    {
        public EuroFanActor(IApiDriver apiDriver) : base(apiDriver)
        {
        }

        public void Given_I_want_to_retrieve_all_queryable_countries() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableCountries();

        public async Task Then_the_retrieved_queryable_countries_should_be_a_non_empty_list_ordered_by_country_code()
        {
            GetQueryableCountriesResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.QueryableCountries)
                .IsNotEmpty()
                .And.IsOrderedBy(country => country.CountryCode);
        }
    }
}

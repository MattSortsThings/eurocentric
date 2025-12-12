using Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;
using Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Utils;
using Eurocentric.Tests.Acceptance.Utils;
using TUnit.Assertions.Enums;

namespace Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Countries;

public sealed partial class GetCountriesTests
{
    [Test]
    [V0Point1Upward]
    public async Task GetCountries_should_retrieve_all_existing_countries_in_country_code_order(string apiVersion)
    {
        HappyPathActor actor = Actor
            .WithResponse<GetCountriesResponse>()
            .Testing(SystemUnderTest)
            .UsingApiVersion(apiVersion)
            .UsingSecretApiKey()
            .Build(HappyPathActor.Create);

        // Given
        await actor.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await actor.Given_I_have_created_a_country(countryCode: "AT", countryName: "Austria");
        await actor.Given_I_have_created_a_country(countryCode: "CH", countryName: "Switzerland");
        await actor.Given_I_have_created_a_country(countryCode: "XX", countryName: "Rest of the World");
        await actor.Given_I_have_created_a_country(countryCode: "AU", countryName: "Australia");

        actor.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await actor.When_I_send_my_request();

        // Then
        await actor.Then_my_request_should_SUCCEED_with_status_code(200);
        await actor.Then_the_retrieved_countries_should_be_all_my_created_countries_in_country_code_order();
    }

    [Test]
    [V0Point1Upward]
    public async Task GetCountries_should_retrieve_empty_list_when_no_countries_exist(string apiVersion)
    {
        HappyPathActor actor = Actor
            .WithResponse<GetCountriesResponse>()
            .Testing(SystemUnderTest)
            .UsingApiVersion(apiVersion)
            .UsingSecretApiKey()
            .Build(HappyPathActor.Create);

        // Given
        actor.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await actor.When_I_send_my_request();

        // Then
        await actor.Then_my_request_should_SUCCEED_with_status_code(200);
        await actor.Then_the_retrieved_countries_should_be_an_empty_list();
    }

    public sealed class HappyPathActor : FeatureActor
    {
        private HappyPathActor(IActorKernel kernel)
            : base(kernel) { }

        public static HappyPathActor Create(IActorKernel kernel) => new(kernel);

        public async Task Then_the_retrieved_countries_should_be_all_my_created_countries_in_country_code_order()
        {
            IOrderedEnumerable<Country> myOrderedCountries = MyCountries.OrderBy(country => country.CountryCode);

            await Assert
                .That(SuccessResponse?.Data?.Countries)
                .IsEquivalentTo(myOrderedCountries, EqualityComparer<Country>.Default, CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_countries_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Countries).IsEmpty();
    }
}

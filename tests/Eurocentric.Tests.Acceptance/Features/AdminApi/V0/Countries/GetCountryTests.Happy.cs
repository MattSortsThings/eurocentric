using Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;
using Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Utils;
using Eurocentric.Tests.Acceptance.Utils;

namespace Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Countries;

public sealed partial class GetCountryTests
{
    [Test]
    [V0Point1Upward]
    public async Task GetCountry_should_retrieve_requested_country(string apiVersion)
    {
        HappyPathActor actor = Actor
            .WithResponse<GetCountryResponse>()
            .Testing(SystemUnderTest)
            .UsingApiVersion(apiVersion)
            .UsingSecretApiKey()
            .Build(HappyPathActor.Create);

        // Given
        await actor.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");

        await actor.Given_I_want_to_retrieve_my_country();

        // When
        await actor.When_I_send_my_request();

        // Then
        await actor.Then_my_request_should_SUCCEED_with_status_code(200);
        await actor.Then_the_retrieved_country_should_be_my_country();
    }

    private sealed class HappyPathActor : FeatureActor
    {
        private HappyPathActor(IActorKernel kernel)
            : base(kernel) { }

        public static HappyPathActor Create(IActorKernel kernel) => new(kernel);

        public async Task Then_the_retrieved_country_should_be_my_country()
        {
            Country myCountry = await Assert.That(MyCountry).IsNotNull();

            await Assert.That(SuccessResponse?.Data?.Country).IsEqualTo(myCountry);
        }
    }
}

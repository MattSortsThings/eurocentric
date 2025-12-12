using Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;
using Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Utils;
using Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Utils.Extensions.Countries;
using Eurocentric.Tests.Acceptance.Utils;

namespace Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Countries;

public sealed partial class DeleteCountryTests
{
    [Test]
    [V0Point1Upward]
    public async Task DeleteCountry_should_delete_requested_country(string apiVersion)
    {
        HappyPathActor actor = Actor
            .WithoutResponse()
            .Testing(SystemUnderTest)
            .UsingApiVersion(apiVersion)
            .UsingSecretApiKey()
            .Build(HappyPathActor.Create);

        // Given
        await actor.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");

        await actor.Given_I_want_to_delete_my_country();

        // When
        await actor.When_I_send_my_request();

        // Then
        await actor.Then_my_request_should_SUCCEED_with_status_code(204);
        await actor.Then_my_country_should_not_exist();
    }

    private sealed class HappyPathActor : FeatureActor
    {
        public HappyPathActor(IActorKernel kernel)
            : base(kernel) { }

        public static HappyPathActor Create(IActorKernel kernel) => new(kernel);

        public async Task Then_my_country_should_not_exist()
        {
            Country myCountry = await Assert.That(MyCountry).IsNotNull();

            Country[] existingCountries = await Kernel.GetAllCountriesAsync();

            await Assert.That(existingCountries).DoesNotContain(myCountry, EqualityComparer<Country>.Default);
        }
    }
}

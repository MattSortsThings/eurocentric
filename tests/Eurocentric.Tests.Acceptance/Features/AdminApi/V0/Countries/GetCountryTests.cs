using Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;
using Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Utils.Extensions.Countries;
using Eurocentric.Tests.Acceptance.Utils;

namespace Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Countries;

[Category("admin-api")]
[NotInParallel("AdminApi.V0.Countries.GetCountryTests")]
public sealed partial class GetCountryTests : AcceptanceTests
{
    private abstract class FeatureActor : ActorWithResponse<GetCountryResponse>
    {
        protected FeatureActor(IActorKernel kernel)
            : base(kernel) { }

        private protected Country? MyCountry { get; private set; }

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "")
        {
            Country createdCountry = await Kernel.CreateSingleCountryAsync(
                countryCode: countryCode,
                countryName: countryName
            );

            MyCountry = createdCountry;
        }

        public async Task Given_I_want_to_retrieve_my_country()
        {
            Guid myCountryId = await Assert.That(MyCountry?.Id).IsNotNull();

            Request = Kernel.RestRequestFactory.GetCountry(myCountryId);
        }
    }
}

using Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;
using Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Utils.Extensions.Countries;
using Eurocentric.Tests.Acceptance.Utils;

namespace Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Countries;

[Category("admin-api")]
[NotInParallel("AdminApi.V0.Countries.GetCountriesTests")]
public sealed partial class GetCountriesTests : AcceptanceTests
{
    public abstract class FeatureActor : ActorWithResponse<GetCountriesResponse>
    {
        protected FeatureActor(IActorKernel kernel)
            : base(kernel) { }

        private protected List<Country> MyCountries { get; } = [];

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "")
        {
            Country createdCountry = await Kernel.CreateSingleCountryAsync(
                countryCode: countryCode,
                countryName: countryName
            );

            MyCountries.Add(createdCountry);
        }

        public void Given_I_want_to_retrieve_all_existing_countries() =>
            Request = Kernel.RestRequestFactory.GetCountries();
    }
}

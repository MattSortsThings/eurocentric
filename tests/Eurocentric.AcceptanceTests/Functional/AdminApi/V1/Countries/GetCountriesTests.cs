using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V0.Features.Countries;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Countries;

[Category("admin-api")]
public sealed class GetCountriesTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_dummy_countries(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_countries_should_be_in_country_code_order();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetCountriesResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_all_existing_countries() =>
            Request = Kernel.Requests.Countries.GetCountries();

        public async Task Then_the_retrieved_countries_should_be_in_country_code_order() =>
            await Assert.That(SuccessResponse?.Data?.Countries).IsOrderedBy(country => country.CountryCode);
    }
}

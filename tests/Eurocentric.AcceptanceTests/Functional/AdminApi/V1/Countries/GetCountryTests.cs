using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Features.Countries;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Countries;

[Category("admin-api")]
public sealed class GetCountryTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_dummy_country(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_country_with_ID("7bd84846-d4f1-426c-ba08-41e449b103bb");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_country_should_have_the_ID("7bd84846-d4f1-426c-ba08-41e449b103bb");
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetCountryResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_the_country_with_ID(string countryId)
        {
            Guid targetId = Guid.Parse(countryId);

            Request = Kernel.Requests.Countries.GetCountry(targetId);
        }

        public async Task Then_the_retrieved_country_should_have_the_ID(string countryId)
        {
            Guid expectedId = Guid.Parse(countryId);

            Country retrievedCountry = await Assert.That(SuccessResponse?.Data?.Country).IsNotNull();

            await Assert.That(retrievedCountry.Id).IsEqualTo(expectedId);
        }
    }
}

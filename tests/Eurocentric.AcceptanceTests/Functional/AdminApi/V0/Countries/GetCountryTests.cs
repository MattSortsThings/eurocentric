using Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils;
using Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils.Attributes;
using Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils.Extensions;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V0.Dtos.Countries;
using Eurocentric.Apis.Admin.V0.Features.Countries;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.Countries;

[Category("admin-api")]
public sealed class GetCountryTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_retrieve_requested_country(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");

        await admin.Given_I_want_to_retrieve_my_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
    }

    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_fail_on_country_not_found(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_deleted_my_country();

        await admin.Given_I_want_to_retrieve_the_deleted_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(500);
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetCountryResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private Country? ExistingCountry { get; set; }

        private Guid? DeletedCountryId { get; set; }

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "")
        {
            Country createdCountry = await Kernel.CreateACountryAsync(
                countryCode: countryCode,
                countryName: countryName,
                cancellationToken: TestContext.Current!.CancellationToken
            );

            ExistingCountry = createdCountry;
        }

        public async Task Given_I_have_deleted_my_country()
        {
            Guid existingCountryId = await Assert.That(ExistingCountry?.Id).IsNotNull();

            await Kernel.DeleteACountryAsync(existingCountryId, TestContext.Current!.CancellationToken);

            DeletedCountryId = existingCountryId;
            ExistingCountry = null;
        }

        public async Task Given_I_want_to_retrieve_my_country()
        {
            Guid existingCountryId = await Assert.That(ExistingCountry?.Id).IsNotNull();

            Request = Kernel.Requests.Countries.GetCountry(existingCountryId);
        }

        public async Task Given_I_want_to_retrieve_the_deleted_country()
        {
            Guid deletedCountryId = await Assert.That(DeletedCountryId).IsNotNull();

            Request = Kernel.Requests.Countries.GetCountry(deletedCountryId);
        }
    }
}

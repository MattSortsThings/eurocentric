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
        await admin.Then_the_retrieved_country_should_be_my_country();
    }

    [Test]
    [ApiVersion1Point0AndUp]
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
        await admin.Then_my_request_should_FAIL_with_status_code(404);
        await admin.Then_the_response_problem_details_should_match(
            status: 404,
            title: "Country not found",
            detail: "The requested country does not exist."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_the_deleted_country_ID();
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
                countryName: countryName
            );

            ExistingCountry = createdCountry;
        }

        public async Task Given_I_have_deleted_my_country()
        {
            Guid countryId = await Assert.That(ExistingCountry?.Id).IsNotNull();

            await Kernel.DeleteACountryAsync(countryId);

            ExistingCountry = null;
            DeletedCountryId = countryId;
        }

        public async Task Given_I_want_to_retrieve_my_country()
        {
            Guid countryId = await Assert.That(ExistingCountry?.Id).IsNotNull();

            Request = Kernel.Requests.Countries.GetCountry(countryId);
        }

        public async Task Given_I_want_to_retrieve_the_deleted_country()
        {
            Guid countryId = await Assert.That(DeletedCountryId).IsNotNull();

            Request = Kernel.Requests.Countries.GetCountry(countryId);
        }

        public async Task Then_the_retrieved_country_should_be_my_country()
        {
            Country expectedCountry = await Assert.That(ExistingCountry).IsNotNull();

            await Assert
                .That(SuccessResponse?.Data?.Country)
                .IsNotNull()
                .And.IsEqualTo(expectedCountry, new CountryEqualityComparer());
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_deleted_country_ID()
        {
            Guid deletedCountryId = await Assert.That(DeletedCountryId).IsNotNull();

            await Assert.That(FailureResponse?.Data).IsNotNull().And.HasExtension("countryId", deletedCountryId);
        }
    }
}

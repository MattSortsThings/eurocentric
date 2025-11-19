using Eurocentric.AcceptanceTests.Functional.AdminApi.V0.Countries.TestUtils;
using Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V0.Dtos.Countries;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.Countries;

public sealed class DeleteCountryTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_delete_requested_country(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");

        await admin.Given_I_want_to_delete_my_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);
        await admin.Then_no_countries_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_fail_on_country_not_found(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_deleted_my_country();

        await admin.Given_I_want_to_delete_the_deleted_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(404);
        await admin.Then_the_response_problem_details_should_match(
            status: 404,
            title: "Country not found",
            detail: "The requested country does not exist."
        );
        await admin.Then_the_response_problem_details_should_include_the_deleted_country_ID();
    }

    [Test]
    [ApiVersion0Point1AndUp]
    public async Task Should_fail_on_country_deletion_not_allowed(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_my_country_has_a_fake_contest_role();

        await admin.Given_I_want_to_delete_my_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Country deletion not permitted",
            detail: "The requested country has one or more contest roles."
        );
        await admin.Then_the_response_problem_details_should_include_my_existing_country_ID();
        await admin.Then_my_existing_country_should_be_the_only_existing_country_in_the_system();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private Guid? ExistingCountryId { get; set; }

        private Guid? DeletedCountryId { get; set; }

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "")
        {
            Country createdCountry = await Kernel.CreateACountryAsync(
                countryCode: countryCode,
                countryName: countryName
            );

            ExistingCountryId = createdCountry.Id;
        }

        public async Task Given_my_country_has_a_fake_contest_role()
        {
            Guid existingCountryId = await Assert.That(ExistingCountryId).IsNotNull();

            await Kernel.BackDoor.ExecuteScopedAsync(
                CountryBackDoorOperations.AddFakeContestRoleToCountry(existingCountryId)
            );
        }

        public async Task Given_I_have_deleted_my_country()
        {
            Guid existingCountryId = await Assert.That(ExistingCountryId).IsNotNull();

            await Kernel.DeleteACountryAsync(existingCountryId);

            DeletedCountryId = existingCountryId;
            ExistingCountryId = null;
        }

        public async Task Given_I_want_to_delete_my_country()
        {
            Guid countryId = await Assert.That(ExistingCountryId).IsNotNull();

            Request = Kernel.Requests.Countries.DeleteCountry(countryId);
        }

        public async Task Given_I_want_to_delete_the_deleted_country()
        {
            Guid deletedCountryId = await Assert.That(DeletedCountryId).IsNotNull();

            Request = Kernel.Requests.Countries.DeleteCountry(deletedCountryId);
        }

        public async Task Then_no_countries_should_exist_in_the_system()
        {
            Country[] existingCountries = await Kernel.GetAllCountriesAsync();

            await Assert.That(existingCountries).IsEmpty();
        }

        public async Task Then_my_existing_country_should_be_the_only_existing_country_in_the_system()
        {
            Guid expectedCountryId = await Assert.That(ExistingCountryId).IsNotNull();

            Country[] existingCountries = await Kernel.GetAllCountriesAsync();

            await Assert
                .That(existingCountries)
                .HasSingleItem()
                .And.ContainsOnly(country => country.Id == expectedCountryId);
        }

        public async Task Then_the_response_problem_details_should_include_my_existing_country_ID()
        {
            Guid existingCountryId = await Assert.That(ExistingCountryId).IsNotNull();

            await Assert.That(FailureResponse?.Data).IsNotNull().And.HasExtension("countryId", existingCountryId);
        }

        public async Task Then_the_response_problem_details_should_include_the_deleted_country_ID()
        {
            Guid deletedCountryId = await Assert.That(DeletedCountryId).IsNotNull();

            await Assert.That(FailureResponse?.Data).IsNotNull().And.HasExtension("countryId", deletedCountryId);
        }
    }
}

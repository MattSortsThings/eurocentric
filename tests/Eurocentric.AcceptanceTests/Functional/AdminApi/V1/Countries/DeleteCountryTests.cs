using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Countries;

[Category("admin-api")]
public sealed class DeleteCountryTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
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
        await admin.Then_there_should_be_no_existing_countries();
    }

    [Test]
    [ApiVersion1Point0AndUp]
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
        await admin.Then_the_response_problem_details_extensions_should_include_the_deleted_country_ID();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_country_deletion_not_permitted(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_created_a_contest_for_my_country_and_5_new_discard_countries();

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
        await admin.Then_the_response_problem_details_extensions_should_include_my_country_ID();
        await admin.Then_my_country_should_exist_unchanged();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private Country? ExistingCountry { get; set; }

        private Guid? DeletedCountryId { get; set; }

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "") =>
            ExistingCountry = await Kernel.CreateACountryAsync(countryCode: countryCode, countryName: countryName);

        public async Task Given_I_have_created_a_contest_for_my_country_and_5_new_discard_countries()
        {
            Guid existingCountryId = await Assert.That(ExistingCountry?.Id).IsNotNull();

            List<Guid> countryIds = new(6) { existingCountryId };

            await foreach (Country country in Kernel.CreateMultipleCountriesAsync("AA", "BB", "CC", "DD", "EE"))
            {
                countryIds.Add(country.Id);
            }

            _ = await Kernel.CreateAStockholmRulesContestAsync(
                contestYear: TestDefaults.ContestYear,
                semiFinal1CountryIds: countryIds.Take(3).ToArray(),
                semiFinal2CountryIds: countryIds.Skip(3).ToArray()
            );

            ExistingCountry = await Kernel.GetACountryAsync(existingCountryId);
        }

        public async Task Given_I_have_deleted_my_country()
        {
            Guid countryId = await Assert.That(ExistingCountry?.Id).IsNotNull();

            await Kernel.DeleteACountryAsync(countryId);

            DeletedCountryId = countryId;
            ExistingCountry = null;
        }

        public async Task Given_I_want_to_delete_my_country()
        {
            Guid countryId = await Assert.That(ExistingCountry?.Id).IsNotNull();

            Request = Kernel.Requests.Countries.DeleteCountry(countryId);
        }

        public async Task Given_I_want_to_delete_the_deleted_country()
        {
            Guid countryId = await Assert.That(DeletedCountryId).IsNotNull();

            Request = Kernel.Requests.Countries.DeleteCountry(countryId);
        }

        public async Task Then_there_should_be_no_existing_countries()
        {
            Country[] existingCountries = await Kernel.GetAllCountriesAsync();

            await Assert.That(existingCountries).IsEmpty();
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_deleted_country_ID()
        {
            Guid countryId = await Assert.That(DeletedCountryId).IsNotNull();

            await Assert.That(FailureResponse?.Data).HasExtension("countryId", countryId);
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_country_ID()
        {
            Guid countryId = await Assert.That(ExistingCountry?.Id).IsNotNull();

            await Assert.That(FailureResponse?.Data).HasExtension("countryId", countryId);
        }

        public async Task Then_my_country_should_exist_unchanged()
        {
            Country existingCountry = await Assert.That(ExistingCountry).IsNotNull();
            Country retrievedCountry = await Kernel.GetACountryAsync(existingCountry.Id);

            await Assert.That(retrievedCountry).IsEqualTo(existingCountry, new CountryEqualityComparer());
        }
    }
}

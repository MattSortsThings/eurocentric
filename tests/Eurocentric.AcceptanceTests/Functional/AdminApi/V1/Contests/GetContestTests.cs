using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Features.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests;

[Category("admin-api")]
public sealed class GetContestTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_requested_contest(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2018,
            cityName: "Lisbon",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );

        await admin.Given_I_want_to_retrieve_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_contest_should_be_my_contest();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_contest_not_found(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2018,
            cityName: "Lisbon",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_deleted_my_contest();

        await admin.Given_I_want_to_retrieve_the_deleted_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(404);
        await admin.Then_the_response_problem_details_should_match(
            status: 404,
            title: "Contest not found",
            detail: "The requested contest does not exist."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_the_deleted_contest_ID();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetContestResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private CountryIdLookup ExistingCountryIds { get; } = new();

        private Contest? ExistingContest { get; set; }

        private Guid? DeletedContestId { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            ExistingCountryIds.EnsureCapacity(countryCodes.Length);

            await foreach (Country country in Kernel.CreateMultipleCountriesAsync(countryCodes))
            {
                ExistingCountryIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest(
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            string cityName = "",
            int contestYear = 0
        )
        {
            ExistingContest = await Kernel.CreateAStockholmRulesContestAsync(
                contestYear: contestYear,
                cityName: cityName,
                semiFinal1CountryIds: semiFinal1Countries.Select(ExistingCountryIds.GetId).ToArray(),
                semiFinal2CountryIds: semiFinal2Countries.Select(ExistingCountryIds.GetId).ToArray()
            );
        }

        public async Task Given_I_have_deleted_my_contest()
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            await Kernel.DeleteAContestAsync(contestId);

            ExistingContest = null;
            DeletedContestId = contestId;
        }

        public async Task Given_I_want_to_retrieve_my_contest()
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            Request = Kernel.Requests.Contests.GetContest(contestId);
        }

        public async Task Given_I_want_to_retrieve_the_deleted_contest()
        {
            Guid contestId = await Assert.That(DeletedContestId).IsNotNull();

            Request = Kernel.Requests.Contests.GetContest(contestId);
        }

        public async Task Then_the_retrieved_contest_should_be_my_contest()
        {
            Contest expectedContest = await Assert.That(ExistingContest).IsNotNull();

            await Assert.That(SuccessResponse?.Data?.Contest).IsEqualTo(expectedContest, new ContestEqualityComparer());
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_deleted_contest_ID()
        {
            Guid deletedContestId = await Assert.That(DeletedContestId).IsNotNull();

            await Assert.That(FailureResponse?.Data).HasExtension("contestId", deletedContestId);
        }
    }
}

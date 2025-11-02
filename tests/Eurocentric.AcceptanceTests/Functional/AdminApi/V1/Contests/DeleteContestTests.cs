using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Enums;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests;

[Category("admin-api")]
public sealed class DeleteContestTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_delete_requested_contest(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2016,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );

        await admin.Given_I_want_to_delete_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);
        await admin.Then_there_should_be_no_existing_contests();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_contest_not_found(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2016,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_deleted_my_contest();

        await admin.Given_I_want_to_delete_the_deleted_contest();

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

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_contest_deletion_not_permitted(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2016,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_GrandFinal_broadcast_for_my_contest();

        await admin.Given_I_want_to_delete_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_the_response_problem_details_should_match(
            status: 409,
            title: "Contest deletion not permitted",
            detail: "The requested contest has one or more child broadcasts."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_my_contest_ID();
        await admin.Then_my_contest_should_exist_unchanged();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor
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
            int contestYear = 0
        )
        {
            ExistingContest = await Kernel.CreateAStockholmRulesContestAsync(
                contestYear: contestYear,
                semiFinal1CountryIds: ExistingCountryIds.MapToGuids(semiFinal1Countries),
                semiFinal2CountryIds: ExistingCountryIds.MapToGuids(semiFinal2Countries)
            );
        }

        public async Task Given_I_have_deleted_my_contest()
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            await Kernel.DeleteAContestAsync(contestId);

            ExistingContest = null;
            DeletedContestId = contestId;
        }

        public async Task Given_I_have_created_the_GrandFinal_broadcast_for_my_contest()
        {
            Contest contest = await Assert.That(ExistingContest).IsNotNull();

            Guid contestId = contest.Id;
            DateOnly broadcastDate = new(contest.ContestYear, 1, 1);

            Guid?[] competingCountryIds = contest
                .Participants.Select(participant => (Guid?)participant.ParticipatingCountryId)
                .ToArray();

            _ = await Kernel.CreateABroadcastAsync(
                contestId: contestId,
                broadcastDate: broadcastDate,
                contestStage: ContestStage.GrandFinal,
                competingCountryIds: competingCountryIds
            );

            ExistingContest = await Kernel.GetAContestAsync(contestId);
        }

        public async Task Given_I_want_to_delete_my_contest()
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            Request = Kernel.Requests.Contests.DeleteContest(contestId);
        }

        public async Task Given_I_want_to_delete_the_deleted_contest()
        {
            Guid contestId = await Assert.That(DeletedContestId).IsNotNull();

            Request = Kernel.Requests.Contests.DeleteContest(contestId);
        }

        public async Task Then_there_should_be_no_existing_contests()
        {
            Contest[] existingContests = await Kernel.GetAllContestsAsync();

            await Assert.That(existingContests).IsEmpty();
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_deleted_contest_ID()
        {
            Guid deletedContestId = await Assert.That(DeletedContestId).IsNotNull();

            await Assert.That(FailureResponse?.Data).HasExtension("contestId", deletedContestId);
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_contest_ID()
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            await Assert.That(FailureResponse?.Data).HasExtension("contestId", contestId);
        }

        public async Task Then_my_contest_should_exist_unchanged()
        {
            Contest contest = await Assert.That(ExistingContest).IsNotNull();
            Contest retrievedContest = await Kernel.GetAContestAsync(contest.Id);

            await Assert.That(retrievedContest).IsEqualTo(contest, new ContestEqualityComparer());
        }
    }
}

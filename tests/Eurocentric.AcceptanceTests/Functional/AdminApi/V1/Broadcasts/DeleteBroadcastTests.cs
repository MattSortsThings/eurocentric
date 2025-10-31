using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Enums;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Broadcasts;

[Category("admin-api")]
public sealed class DeleteBroadcastTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_delete_requested_broadcast(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2025,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            competingCountries: ["DK", "EE", "AT", "BE"]
        );

        await admin.Given_I_want_to_delete_my_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);
        await admin.Then_there_should_be_no_existing_broadcasts();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_broadcast_not_found(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2025,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            competingCountries: ["DK", "EE", "AT", "BE"]
        );
        await admin.Given_I_have_deleted_my_broadcast();

        await admin.Given_I_want_to_delete_the_deleted_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(404);
        await admin.Then_the_response_problem_details_should_match(
            status: 404,
            title: "Broadcast not found",
            detail: "The requested broadcast does not exist."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_the_deleted_broadcast_ID();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private CountryIdLookup ExistingCountryIds { get; } = new();

        private Guid? ExistingContestId { get; set; }

        private Broadcast? ExistingBroadcast { get; set; }

        private Guid? DeletedBroadcastId { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            ExistingCountryIds.EnsureCapacity(countryCodes.Length);

            await foreach (Country country in Kernel.CreateMultipleCountriesAsync(countryCodes))
            {
                ExistingCountryIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Stockholm_rules_contest(
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            int contestYear = 0
        )
        {
            Contest contest = await Kernel.CreateAStockholmRulesContestAsync(
                contestYear: contestYear,
                semiFinal1CountryIds: ExistingCountryIds.MapToGuids(semiFinal1Countries),
                semiFinal2CountryIds: ExistingCountryIds.MapToGuids(semiFinal2Countries)
            );

            ExistingContestId = contest.Id;
        }

        public async Task Given_I_have_created_a_broadcast_for_my_contest(
            string?[] competingCountries = null!,
            string contestStage = "",
            string broadcastDate = ""
        )
        {
            Guid contestId = await Assert.That(ExistingContestId).IsNotNull();

            ExistingBroadcast = await Kernel.CreateABroadcastAsync(
                contestId: contestId,
                broadcastDate: DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                contestStage: Enum.Parse<ContestStage>(contestStage),
                competingCountryIds: ExistingCountryIds.MapToNullableGuids(competingCountries)
            );
        }

        public async Task Given_I_have_deleted_my_broadcast()
        {
            Guid broadcastId = await Assert.That(ExistingBroadcast?.Id).IsNotNull();

            await Kernel.DeleteABroadcastAsync(broadcastId);

            ExistingBroadcast = null;
            DeletedBroadcastId = broadcastId;
        }

        public async Task Given_I_want_to_delete_my_broadcast()
        {
            Guid broadcastId = await Assert.That(ExistingBroadcast?.Id).IsNotNull();

            Request = Kernel.Requests.Broadcasts.DeleteBroadcast(broadcastId);
        }

        public async Task Given_I_want_to_delete_the_deleted_broadcast()
        {
            Guid broadcastId = await Assert.That(DeletedBroadcastId).IsNotNull();

            Request = Kernel.Requests.Broadcasts.DeleteBroadcast(broadcastId);
        }

        public async Task Then_there_should_be_no_existing_broadcasts()
        {
            Broadcast[] existingBroadcasts = await Kernel.GetAllBroadcastsAsync();

            await Assert.That(existingBroadcasts).IsEmpty();
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_deleted_broadcast_ID()
        {
            Guid deletedBroadcastId = await Assert.That(DeletedBroadcastId).IsNotNull();

            await Assert.That(FailureResponse?.Data).HasExtension("broadcastId", deletedBroadcastId);
        }
    }
}

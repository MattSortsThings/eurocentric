using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Broadcasts;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastsTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_all_existing_broadcasts_in_broadcast_date_order(string apiVersion)
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
            contestStage: "SemiFinal2",
            broadcastDate: "2025-05-02",
            competingCountries: ["DK", "EE"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-01",
            competingCountries: ["AT", "BE"]
        );
        await admin.Given_I_have_created_a_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-03",
            competingCountries: ["AT", "FI"]
        );

        admin.Given_I_want_to_retrieve_all_existing_broadcasts();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_broadcasts_should_be_my_broadcasts_in_broadcast_date_order();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_list_when_no_broadcasts_exist(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_broadcasts();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_broadcasts_should_be_an_empty_list();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetBroadcastsResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private CountryIdLookup ExistingCountryIds { get; } = new();

        private Guid? ExistingContestId { get; set; }

        private List<Broadcast> ExistingBroadcasts { get; } = [];

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

            Broadcast broadcast = await Kernel.CreateABroadcastAsync(
                contestId: contestId,
                broadcastDate: DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                contestStage: Enum.Parse<ContestStage>(contestStage),
                competingCountryIds: ExistingCountryIds.MapToNullableGuids(competingCountries)
            );

            ExistingBroadcasts.Add(broadcast);
        }

        public void Given_I_want_to_retrieve_all_existing_broadcasts() =>
            Request = Kernel.Requests.Broadcasts.GetBroadcasts();

        public async Task Then_the_retrieved_broadcasts_should_be_my_broadcasts_in_broadcast_date_order()
        {
            IOrderedEnumerable<Broadcast> expectedBroadcasts = ExistingBroadcasts.OrderBy(broadcast =>
                broadcast.BroadcastDate
            );

            await Assert
                .That(SuccessResponse?.Data?.Broadcasts)
                .IsEquivalentTo(expectedBroadcasts, new BroadcastEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_broadcasts_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Broadcasts).IsEmpty();
    }
}

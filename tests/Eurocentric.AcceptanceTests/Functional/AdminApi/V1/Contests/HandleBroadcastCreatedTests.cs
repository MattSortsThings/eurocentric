using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests;

[Category("admin-api")]
public sealed class HandleBroadcastCreatedTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_add_child_broadcast_to_Liverpool_rules_parent_contest_on_broadcast_created(
        string apiVersion
    )
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );

        await admin.Given_I_want_to_create_a_GrandFinal_broadcast_for_my_contest_with_broadcast_date("2025-05-01");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_my_contest_should_be_updated_with_a_child_broadcast_for_the_created_broadcast();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_add_child_broadcast_to_Stockholm_rules_parent_contest_on_broadcast_created(
        string apiVersion
    )
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2025,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );

        await admin.Given_I_want_to_create_a_GrandFinal_broadcast_for_my_contest_with_broadcast_date("2025-05-01");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(201);
        await admin.Then_my_contest_should_be_updated_with_a_child_broadcast_for_the_created_broadcast();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_not_update_Liverpool_rules_parent_contest_on_failed_broadcast_creation(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest(
            contestYear: 2025,
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );

        await admin.Given_I_want_to_create_a_GrandFinal_broadcast_for_my_contest_with_broadcast_date("1066-10-14");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_my_contest_should_not_be_updated();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_not_update_Stockholm_rules_parent_contest_on_failed_broadcast_creation(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Stockholm_rules_contest(
            contestYear: 2025,
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );

        await admin.Given_I_want_to_create_a_GrandFinal_broadcast_for_my_contest_with_broadcast_date("1066-10-14");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(422);
        await admin.Then_my_contest_should_not_be_updated();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<CreateContestBroadcastResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private CountryIdLookup ExistingCountryIds { get; } = new();

        private Contest? ExistingContest { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            ExistingCountryIds.EnsureCapacity(countryCodes.Length);

            await foreach (Country country in Kernel.CreateMultipleCountriesAsync(countryCodes))
            {
                ExistingCountryIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Liverpool_rules_contest(
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            string globalTelevoteCountry = "",
            int contestYear = 0
        )
        {
            ExistingContest = await Kernel.CreateALiverpoolRulesContestAsync(
                contestYear: contestYear,
                globalTelevoteCountryId: ExistingCountryIds.GetId(globalTelevoteCountry),
                semiFinal1CountryIds: semiFinal1Countries.Select(ExistingCountryIds.GetId).ToArray(),
                semiFinal2CountryIds: semiFinal2Countries.Select(ExistingCountryIds.GetId).ToArray()
            );
        }

        public async Task Given_I_have_created_a_Stockholm_rules_contest(
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            int contestYear = 0
        )
        {
            ExistingContest = await Kernel.CreateAStockholmRulesContestAsync(
                contestYear: contestYear,
                semiFinal1CountryIds: semiFinal1Countries.Select(ExistingCountryIds.GetId).ToArray(),
                semiFinal2CountryIds: semiFinal2Countries.Select(ExistingCountryIds.GetId).ToArray()
            );
        }

        public async Task Given_I_want_to_create_a_GrandFinal_broadcast_for_my_contest_with_broadcast_date(
            string broadcastDate
        )
        {
            Contest existingContest = await Assert.That(ExistingContest).IsNotNull();

            CreateContestBroadcastRequest requestBody = new()
            {
                BroadcastDate = DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                ContestStage = ContestStage.GrandFinal,
                CompetingCountryIds = existingContest
                    .Participants.Take(2)
                    .Select(participant => (Guid?)participant.ParticipatingCountryId)
                    .ToArray(),
            };

            Request = Kernel.Requests.Contests.CreateContestBroadcast(existingContest.Id, requestBody);
        }

        public async Task Then_my_contest_should_be_updated_with_a_child_broadcast_for_the_created_broadcast()
        {
            Broadcast createdBroadcast = await Assert.That(SuccessResponse?.Data?.Broadcast).IsNotNull();
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            Contest retrievedContest = await Kernel.GetAContestAsync(contestId);

            await Assert
                .That(retrievedContest.ChildBroadcasts)
                .HasSingleItem()
                .And.Contains(item =>
                    item.ChildBroadcastId.Equals(createdBroadcast.Id)
                    && item.ContestStage == createdBroadcast.ContestStage
                    && !item.Completed
                );
        }

        public async Task Then_my_contest_should_not_be_updated()
        {
            Contest existingContest = await Assert.That(ExistingContest).IsNotNull();

            Contest retrievedContest = await Kernel.GetAContestAsync(ExistingContest.Id);

            await Assert.That(retrievedContest).IsEqualTo(existingContest, new ContestEqualityComparer());
        }
    }
}

using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Enums;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Countries;

[Category("admin-api")]
public sealed class HandleContestDeletedTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_update_all_participating_and_voting_countries_on_contest_deleted(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest_for_my_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );

        await admin.Given_I_want_to_delete_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(204);
        await admin.Then_no_existing_country_should_have_a_contest_role_referencing_my_contest();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_not_update_participating_and_voting_countries_on_failed_contest_deletion(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_rules_contest_for_my_countries(
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_the_GrandFinal_broadcast_for_my_contest();

        await admin.Given_I_want_to_delete_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(409);
        await admin.Then_every_existing_country_should_still_have_a_contest_role_referencing_my_contest();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor
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

        public async Task Given_I_have_created_a_Liverpool_rules_contest_for_my_countries(
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            string globalTelevoteCountry = ""
        )
        {
            ExistingContest = await Kernel.CreateALiverpoolRulesContestAsync(
                contestYear: TestDefaults.ContestYear,
                globalTelevoteCountryId: ExistingCountryIds.GetId(globalTelevoteCountry),
                semiFinal1CountryIds: ExistingCountryIds.MapToGuids(semiFinal1Countries),
                semiFinal2CountryIds: ExistingCountryIds.MapToGuids(semiFinal2Countries)
            );
        }

        public async Task Given_I_have_created_the_GrandFinal_broadcast_for_my_contest()
        {
            Contest contest = await Assert.That(ExistingContest).IsNotNull();

            _ = await Kernel.CreateABroadcastAsync(
                contestId: contest.Id,
                contestStage: ContestStage.GrandFinal,
                broadcastDate: new DateOnly(contest.ContestYear, 1, 1),
                competingCountryIds: contest
                    .Participants.Select(participant => (Guid?)participant.ParticipatingCountryId)
                    .ToArray()
            );

            ExistingContest = await Kernel.GetAContestAsync(contest.Id);
        }

        public async Task Given_I_want_to_delete_my_contest()
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            Request = Kernel.Requests.Contests.DeleteContest(contestId);
        }

        public async Task Then_no_existing_country_should_have_a_contest_role_referencing_my_contest()
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            Country[] existingCountries = await Kernel.GetAllCountriesAsync();

            await Assert
                .That(existingCountries)
                .DoesNotContain(country => country.ContestRoles.Any(role => role.ContestId.Equals(contestId)));
        }

        public async Task Then_every_existing_country_should_still_have_a_contest_role_referencing_my_contest()
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            Country[] existingCountries = await Kernel.GetAllCountriesAsync();

            await Assert
                .That(existingCountries)
                .ContainsOnly(country => country.ContestRoles.Any(role => role.ContestId.Equals(contestId)));
        }
    }
}

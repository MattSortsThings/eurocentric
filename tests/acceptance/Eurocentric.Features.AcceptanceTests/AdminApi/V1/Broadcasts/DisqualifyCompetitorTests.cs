using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Infrastructure.EFCore;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed class DisqualifyCompetitorTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_disqualify_competitor_in_initialized_broadcast_scenario_1(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_in_2025(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_GrandFinal_child_broadcast_for_my_contest_with_competitors(
            "AT", "BE", "CZ", "DK", "EE", "FI");
        admin.Given_I_want_to_disqualify_the_competitor_in_my_broadcast("AT");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_my_broadcast_should_now_match(
            broadcastStatus: "Initialized",
            competitors: """
                         | Finish | CountryCode | RunningOrder | JuryAwards | TelevoteAwards |
                         |      1 | BE          |           2  | []         | []             |
                         |      2 | CZ          |           3  | []         | []             |
                         |      3 | DK          |           4  | []         | []             |
                         |      4 | EE          |           5  | []         | []             |
                         |      5 | FI          |           6  | []         | []             |
                         """);
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_disqualify_competitor_in_initialized_broadcast_scenario_2(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_in_2025(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_GrandFinal_child_broadcast_for_my_contest_with_competitors(
            "AT", "BE", "CZ", "DK", "EE", "FI");
        admin.Given_I_want_to_disqualify_the_competitor_in_my_broadcast("CZ");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_my_broadcast_should_now_match(
            broadcastStatus: "Initialized",
            competitors: """
                         | Finish | CountryCode | RunningOrder | JuryAwards | TelevoteAwards |
                         |      1 | AT          |           1  | []         | []             |
                         |      2 | BE          |           2  | []         | []             |
                         |      3 | DK          |           4  | []         | []             |
                         |      4 | EE          |           5  | []         | []             |
                         |      5 | FI          |           6  | []         | []             |
                         """);
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_disqualify_competitor_in_initialized_broadcast_scenario_3(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_in_2025(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_GrandFinal_child_broadcast_for_my_contest_with_competitors(
            "AT", "BE", "CZ", "DK", "EE", "FI");
        admin.Given_I_want_to_disqualify_the_competitor_in_my_broadcast("FI");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_my_broadcast_should_now_match(
            broadcastStatus: "Initialized",
            competitors: """
                         | Finish | CountryCode | RunningOrder | JuryAwards | TelevoteAwards |
                         |      1 | AT          |           1  | []         | []             |
                         |      2 | BE          |           2  | []         | []             |
                         |      3 | CZ          |           3  | []         | []             |
                         |      4 | DK          |           4  | []         | []             |
                         |      5 | EE          |           5  | []         | []             |
                         """);
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_disqualify_competitor_in_broadcast_in_progress(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_in_2025(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest_with_competitors("AT", "BE");
        await admin.Given_I_have_awarded_a_set_of_televote_points_in_my_broadcast(
            votingCountryCode: "AT",
            rankedCompetingCountryCodes: ["BE"]);
        admin.Given_I_want_to_disqualify_the_competitor_in_my_broadcast("AT");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Cannot disqualify",
            detail: "A competitor may only be disqualified when the broadcast status is Initialized.");
        await admin.Then_my_broadcast_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_disqualify_competitor_in_completed_broadcast(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_in_2025(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest_with_competitors("AT", "BE");
        await admin.Given_I_have_awarded_a_set_of_televote_points_in_my_broadcast(
            votingCountryCode: "AT",
            rankedCompetingCountryCodes: ["BE"]);
        await admin.Given_I_have_awarded_a_set_of_televote_points_in_my_broadcast(
            votingCountryCode: "BE",
            rankedCompetingCountryCodes: ["AT"]);
        await admin.Given_I_have_awarded_a_set_of_televote_points_in_my_broadcast(
            votingCountryCode: "CZ",
            rankedCompetingCountryCodes: ["AT", "BE"]);
        await admin.Given_I_have_awarded_a_set_of_televote_points_in_my_broadcast(
            votingCountryCode: "XX",
            rankedCompetingCountryCodes: ["AT", "BE"]);
        admin.Given_I_want_to_disqualify_the_competitor_in_my_broadcast("AT");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Cannot disqualify",
            detail: "A competitor may only be disqualified when the broadcast status is Initialized.");
        await admin.Then_my_broadcast_should_be_unchanged();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_disqualify_non_existent_competitor_in_broadcast(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_in_2025(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest_with_competitors("AT", "BE");
        admin.Given_I_want_to_disqualify_the_competitor_in_my_broadcast("FI");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_409_Conflict();
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Competitor not found",
            detail: "Broadcast has no competitor with the provided competing country ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_broadcast_ID_with_key("broadcastId");
        admin.Then_the_problem_details_extensions_should_contain_the_country_ID(key: "competingCountryId", countryCode: "FI");
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_disqualify_competitor_in_non_existent_broadcast(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_in_2025(
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest_with_competitors("AT", "BE");
        await admin.Given_I_have_deleted_my_broadcast();
        admin.Given_I_want_to_disqualify_the_competitor_in_my_broadcast("AT");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_404_NotFound();
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Broadcast not found",
            detail: "No broadcast exists with the provided broadcast ID.");
    }

    private sealed class AdminActor : ActorWithoutResponse
    {
        public AdminActor(IAdminApiV1Driver apiDriver, IWebAppFixtureBackDoor backDoor) : base(apiDriver)
        {
            BackDoor = backDoor;
        }

        private IWebAppFixtureBackDoor BackDoor { get; }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(7);

        private Guid MyContestId { get; set; }

        private Broadcast? MyBroadcast { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Country[] myCountries = await ApiDriver.Countries.CreateMultipleCountriesAsync(countryCodes,
                TestContext.Current.CancellationToken);

            foreach (Country country in myCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Liverpool_format_contest_in_2025(
            string[]? group1CountryCodes = null,
            string[]? group2CountryCodes = null,
            string group0CountryCode = "")
        {
            Contest myContest = await ApiDriver.Contests.CreateAContestAsync(
                cityName: "CityName",
                contestFormat: ContestFormat.Liverpool,
                contestYear: 2025,
                group0CountryId: MyCountryCodesAndIds[group0CountryCode],
                group1CountryIds: group1CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray(),
                group2CountryIds: group2CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray(),
                cancellationToken: TestContext.Current.CancellationToken);

            MyContestId = myContest.Id;
        }

        public async Task Given_I_have_created_the_GrandFinal_child_broadcast_for_my_contest_with_competitors(
            params string[] countryCodes)
        {
            Guid[] competingCountryIds = countryCodes.Select(code => MyCountryCodesAndIds[code]).ToArray();

            MyBroadcast = await ApiDriver.Contests.CreateAChildBroadcastAsync(contestId: MyContestId,
                contestStage: ContestStage.GrandFinal,
                broadcastDate: DateOnly.ParseExact("2025-05-03", "yyyy-MM-dd"),
                competingCountryIds: competingCountryIds,
                cancellationToken: TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest_with_competitors(
            params string[] countryCodes)
        {
            Guid[] competingCountryIds = countryCodes.Select(code => MyCountryCodesAndIds[code]).ToArray();

            MyBroadcast = await ApiDriver.Contests.CreateAChildBroadcastAsync(contestId: MyContestId,
                contestStage: ContestStage.SemiFinal1,
                broadcastDate: DateOnly.ParseExact("2025-05-01", "yyyy-MM-dd"),
                competingCountryIds: competingCountryIds,
                cancellationToken: TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_awarded_a_set_of_televote_points_in_my_broadcast(
            string[]? rankedCompetingCountryCodes = null,
            string votingCountryCode = "")
        {
            Assert.NotNull(MyBroadcast);

            await ApiDriver.Broadcasts.AwardASetOfTelevotePointsAsync(broadcastId: MyBroadcast.Id,
                votingCountryId: MyCountryCodesAndIds[votingCountryCode],
                rankedCompetingCountryIds: rankedCompetingCountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray(),
                cancellationToken: TestContext.Current.CancellationToken);

            MyBroadcast = await ApiDriver.Broadcasts.GetABroadcastAsync(MyBroadcast.Id);
        }

        public async Task Given_I_have_deleted_my_broadcast()
        {
            Assert.NotNull(MyBroadcast);

            BroadcastId broadcastId = BroadcastId.FromValue(MyBroadcast.Id);

            Func<IServiceProvider, Task> delete = async sp =>
            {
                await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                await dbContext.Broadcasts.Where(broadcast => broadcast.Id == broadcastId)
                    .ExecuteDeleteAsync();
            };

            await BackDoor.ExecuteScopedAsync(delete);
        }

        public void Given_I_want_to_disqualify_the_competitor_in_my_broadcast(string countryCode)
        {
            Assert.NotNull(MyBroadcast);

            Guid broadcastId = MyBroadcast.Id;
            DisqualifyCompetitorRequest requestBody = new(MyCountryCodesAndIds[countryCode]);

            SendMyRequest = apiDriver =>
                apiDriver.Broadcasts.DisqualifyCompetitor(broadcastId, requestBody, TestContext.Current.CancellationToken);
        }

        public async Task Then_my_broadcast_should_now_match(string competitors = "", string broadcastStatus = "Initialized")
        {
            Assert.NotNull(MyBroadcast);

            Broadcast myBroadcastRetrievedAgain =
                await ApiDriver.Broadcasts.GetABroadcastAsync(MyBroadcast.Id, TestContext.Current.CancellationToken);

            Broadcast expectedBroadcast = MyBroadcast with
            {
                BroadcastStatus = Enum.Parse<BroadcastStatus>(broadcastStatus),
                Competitors = ParseExpectedCompetitors(competitors)
            };

            Assert.Equal(expectedBroadcast, myBroadcastRetrievedAgain, new BroadcastEqualityComparer());
        }

        public async Task Then_my_broadcast_should_be_unchanged()
        {
            Assert.NotNull(MyBroadcast);

            Broadcast myBroadcastRetrievedAgain =
                await ApiDriver.Broadcasts.GetABroadcastAsync(MyBroadcast.Id, TestContext.Current.CancellationToken);

            Assert.Equal(MyBroadcast, myBroadcastRetrievedAgain, new BroadcastEqualityComparer());
        }

        public void Then_the_problem_details_extensions_should_contain_my_broadcast_ID_with_key(string key)
        {
            Assert.NotNull(MyBroadcast);
            Assert.NotNull(ResponseProblemDetails);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp.Key == key
                                                                      && kvp.Value is JsonElement e
                                                                      && e.GetGuid() == MyBroadcast.Id);
        }

        public void Then_the_problem_details_extensions_should_contain_the_country_ID(string countryCode = "", string key = "")
        {
            Guid countryId = MyCountryCodesAndIds[countryCode];

            Then_the_problem_details_extensions_should_contain(key, countryId);
        }

        private Competitor[] ParseExpectedCompetitors(string multiLineTable) => multiLineTable.ParseItems<ExpectedCompetitor>()
            .Select(item => new Competitor
            {
                FinishingPosition = item.Finish,
                CompetingCountryId = MyCountryCodesAndIds[item.CountryCode],
                RunningOrderPosition = item.RunningOrder,
                JuryAwards = ParseExpectedAwards(item.JuryAwards),
                TelevoteAwards = ParseExpectedAwards(item.TelevoteAwards)
            })
            .OrderBy(item => item.FinishingPosition)
            .ToArray();

        private Award[] ParseExpectedAwards(IEnumerable<ExpectedAward> expectedAwards) => expectedAwards
            .Select(award => new Award(MyCountryCodesAndIds[award.CountryCode], award.PointsValue))
            .OrderByDescending(award => award.PointsValue)
            .ThenBy(award => award.VotingCountryId)
            .ToArray();

        private sealed record ExpectedCompetitor
        {
            public int Finish { get; [UsedImplicitly] init; }

            public string CountryCode { get; [UsedImplicitly] init; } = string.Empty;

            public int RunningOrder { get; [UsedImplicitly] init; }

            [TypeConverter(typeof(ExpectedAwardArrayConverter))]
            public ExpectedAward[] JuryAwards { get; [UsedImplicitly] init; } = [];

            [TypeConverter(typeof(ExpectedAwardArrayConverter))]
            public ExpectedAward[] TelevoteAwards { get; [UsedImplicitly] init; } = [];
        }

        private sealed class ExpectedAwardArrayConverter : TypeConverter<ExpectedAward[]>
        {
            public override ExpectedAward[]? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData) => text
                ?.TrimStart('[')
                .TrimEnd(']')
                .Split(',')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(ExpectedAward.FromString).ToArray();

            public override string? ConvertToString(ExpectedAward[]? value, IWriterRow row, MemberMapData memberMapData)
            {
                if (value is null)
                {
                    return null;
                }

                return "[" + string.Join(",", value.Select(v => v.ToString())) + "]";
            }
        }

        private sealed record ExpectedAward
        {
            public int PointsValue { get; [UsedImplicitly] init; }

            public string CountryCode { get; [UsedImplicitly] init; } = string.Empty;

            public override string ToString() => $"{CountryCode}:{PointsValue}";

            public static ExpectedAward FromString(string s)
            {
                string[] elements = s.Split(':');

                return new ExpectedAward { CountryCode = elements[0], PointsValue = int.Parse(elements[1]) };
            }
        }
    }
}

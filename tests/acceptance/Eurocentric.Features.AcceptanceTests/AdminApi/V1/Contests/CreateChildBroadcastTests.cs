using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts.Utilities;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Infrastructure.EFCore;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed partial class CreateChildBroadcastTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_create_child_broadcast_for_non_existent_contest(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_deleted_my_contest();
        admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "FI"]);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_404_NotFound();
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Contest not found",
            detail: "No contest exists with the provided contest ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_contest_ID_with_key("contestId");
        await admin.Then_no_broadcasts_should_exist();
    }

    private sealed class AdminActor : ActorWithResponse<CreateChildBroadcastResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver, IWebAppFixtureBackDoor backDoor) : base(apiDriver)
        {
            BackDoor = backDoor;
        }

        private IWebAppFixtureBackDoor BackDoor { get; }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(8);

        private Contest? MyContest { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Country[] myCountries = await ApiDriver.Countries.CreateMultipleCountriesAsync(countryCodes,
                TestContext.Current.CancellationToken);

            foreach (Country country in myCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest(
            string[]? group1CountryCodes = null,
            string[]? group2CountryCodes = null,
            int contestYear = 0) => MyContest =
            await ApiDriver.Contests.CreateAContestAsync(
                cityName: "CityName",
                contestFormat: ContestFormat.Stockholm,
                contestYear: contestYear,
                group0CountryId: null,
                group1CountryIds: group1CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray(),
                group2CountryIds: group2CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray());

        public async Task Given_I_have_created_a_Liverpool_format_contest(
            string group0CountryCode = "",
            string[]? group1CountryCodes = null,
            string[]? group2CountryCodes = null,
            int contestYear = 0) => MyContest =
            await ApiDriver.Contests.CreateAContestAsync(
                cityName: "CityName",
                contestFormat: ContestFormat.Liverpool,
                contestYear: contestYear,
                group0CountryId: MyCountryCodesAndIds[group0CountryCode],
                group1CountryIds: group1CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray(),
                group2CountryIds: group2CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray());

        public async Task Given_I_have_created_a_SemiFinal1_child_broadcast_for_my_contest_with_broadcast_date(
            string broadcastDate)
        {
            Assert.NotNull(MyContest);

            Guid myContestId = MyContest.Id;
            Guid[] myCompetingCountryIds = MyContest.Participants.Where(p => p.ParticipantGroup == 1)
                .Take(2)
                .Select(p => p.ParticipatingCountryId)
                .ToArray();

            _ = await ApiDriver.Contests.CreateAChildBroadcastAsync(contestStage: ContestStage.SemiFinal1,
                contestId: MyContest.Id,
                broadcastDate: DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
                competingCountryIds: myCompetingCountryIds,
                cancellationToken: TestContext.Current.CancellationToken);

            MyContest = await ApiDriver.Contests.GetAContestAsync(myContestId, TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_created_a_SemiFinal2_child_broadcast_for_my_contest_with_broadcast_date(
            string broadcastDate)
        {
            Assert.NotNull(MyContest);

            Guid myContestId = MyContest.Id;
            Guid[] myCompetingCountryIds = MyContest.Participants.Where(p => p.ParticipantGroup == 2)
                .Take(2)
                .Select(p => p.ParticipatingCountryId)
                .ToArray();

            _ = await ApiDriver.Contests.CreateAChildBroadcastAsync(contestStage: ContestStage.SemiFinal2,
                contestId: MyContest.Id,
                broadcastDate: DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
                competingCountryIds: myCompetingCountryIds,
                cancellationToken: TestContext.Current.CancellationToken);

            MyContest = await ApiDriver.Contests.GetAContestAsync(myContestId, TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_created_a_GrandFinal_child_broadcast_for_my_contest_with_broadcast_date(
            string broadcastDate)
        {
            Assert.NotNull(MyContest);

            Guid myContestId = MyContest.Id;
            Guid[] myCompetingCountryIds = MyContest.Participants.Where(p => p.ParticipantGroup == 1)
                .Take(2)
                .Concat(MyContest.Participants.Where(p => p.ParticipantGroup == 2).Take(2))
                .Select(p => p.ParticipatingCountryId)
                .ToArray();

            _ = await ApiDriver.Contests.CreateAChildBroadcastAsync(contestStage: ContestStage.GrandFinal,
                contestId: MyContest.Id,
                broadcastDate: DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
                competingCountryIds: myCompetingCountryIds,
                cancellationToken: TestContext.Current.CancellationToken);

            MyContest = await ApiDriver.Contests.GetAContestAsync(myContestId, TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_deleted_my_contest()
        {
            Assert.NotNull(MyContest);

            ContestId myContestId = ContestId.FromValue(MyContest.Id);

            Func<IServiceProvider, Task> delete = async sp =>
            {
                await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                await dbContext.Contests.Where(contest => contest.Id == myContestId)
                    .ExecuteDeleteAsync();
            };

            await BackDoor.ExecuteScopedAsync(delete);
        }

        public void Given_I_want_to_create_a_child_broadcast_for_my_contest(string broadcastDate = "",
            string contestStage = "",
            string[]? competingCountryCodes = null)
        {
            Assert.NotNull(MyContest);

            CreateChildBroadcastRequest requestBody = new()
            {
                ContestStage = Enum.Parse<ContestStage>(contestStage),
                BroadcastDate = DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
                CompetingCountryIds = competingCountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray() ?? []
            };

            Guid myContestId = MyContest.Id;

            SendMyRequest = apiDriver =>
                apiDriver.Contests.CreateChildBroadcast(myContestId, requestBody, TestContext.Current.CancellationToken);
        }

        public void Then_the_created_broadcast_parent_contest_ID_should_be_my_contest_ID()
        {
            Assert.NotNull(MyContest);
            Assert.NotNull(ResponseObject);

            Assert.Equal(MyContest.Id, ResponseObject.Broadcast.ParentContestId);
        }

        public void Then_the_created_broadcast_should_match(string broadcastDate = "",
            string contestStage = "",
            string broadcastStatus = "",
            string competitors = "",
            string? juries = null,
            string? televotes = null)
        {
            Assert.NotNull(ResponseObject);

            DateOnly expectedBroadcastDate = DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd");
            ContestStage expectedContestStage = Enum.Parse<ContestStage>(contestStage);
            BroadcastStatus expectedBroadcastStatus = Enum.Parse<BroadcastStatus>(broadcastStatus);
            Competitor[] expectedCompetitors = ParseExpectedCompetitors(competitors);
            Voter[] expectedJuries = juries is not null ? ParseExpectedVoters(juries) : [];
            Voter[] expectedTelevotes = televotes is not null ? ParseExpectedVoters(televotes) : [];

            Assert.Equal(expectedBroadcastDate, ResponseObject.Broadcast.BroadcastDate);
            Assert.Equal(expectedContestStage, ResponseObject.Broadcast.ContestStage);
            Assert.Equal(expectedBroadcastStatus, ResponseObject.Broadcast.BroadcastStatus);
            Assert.Equal(expectedCompetitors, ResponseObject.Broadcast.Competitors, new CompetitorEqualityComparer());
            Assert.Equal(expectedJuries, ResponseObject.Broadcast.Juries);
            Assert.Equal(expectedTelevotes, ResponseObject.Broadcast.Televotes);
        }

        public async Task Then_my_contest_should_now_have_additional_memoized_created_broadcast_and_InProgress_contest_status()
        {
            Assert.NotNull(MyContest);
            Assert.NotNull(ResponseObject);

            Contest myContestRetrievedAgain =
                await ApiDriver.Contests.GetAContestAsync(MyContest.Id, TestContext.Current.CancellationToken);

            IOrderedEnumerable<BroadcastMemo> expectedChildBroadcasts = MyContest.ChildBroadcasts
                .Append(ResponseObject.Broadcast.ToBroadcastMemoDto())
                .OrderBy(memo => memo.ContestStage);

            Assert.Equal(ContestStatus.InProgress, myContestRetrievedAgain.ContestStatus);
            Assert.Equal(expectedChildBroadcasts, myContestRetrievedAgain.ChildBroadcasts);
        }

        public async Task Then_my_contest_should_be_unchanged()
        {
            Assert.NotNull(MyContest);

            Contest myContestRetrievedAgain =
                await ApiDriver.Contests.GetAContestAsync(MyContest.Id, TestContext.Current.CancellationToken);

            Assert.Equal(MyContest, myContestRetrievedAgain, new ContestEqualityComparer());
        }

        public async Task Then_the_created_broadcast_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(ResponseObject);

            Broadcast createdBroadcast = ResponseObject.Broadcast;
            Broadcast retrievedBroadcast =
                await ApiDriver.Broadcasts.GetABroadcastAsync(createdBroadcast.Id, TestContext.Current.CancellationToken);

            Assert.Equal(createdBroadcast, retrievedBroadcast, new BroadcastEqualityComparer());
        }

        public async Task Then_no_broadcasts_should_exist()
        {
            Broadcast[] existingBroadcasts =
                await ApiDriver.Broadcasts.GetAllBroadcastsAsync(TestContext.Current.CancellationToken);

            Assert.Empty(existingBroadcasts);
        }

        public async Task Then_my_existing_contest_child_broadcasts_should_be_the_only_existing_broadcasts()
        {
            Assert.NotNull(MyContest);

            Broadcast[] existingBroadcasts =
                await ApiDriver.Broadcasts.GetAllBroadcastsAsync(TestContext.Current.CancellationToken);

            IOrderedEnumerable<BroadcastMemo> memoizedExistingBroadcasts = existingBroadcasts
                .Select(broadcast => broadcast.ToBroadcastMemoDto())
                .OrderBy(memo => memo.ContestStage);

            Assert.Equal(MyContest.ChildBroadcasts, memoizedExistingBroadcasts);
        }

        public void Then_the_problem_details_extensions_should_contain_my_contest_ID_with_key(string key)
        {
            Assert.NotNull(MyContest);

            Then_the_problem_details_extensions_should_contain(key, MyContest.Id);
        }

        public void Then_the_problem_details_extensions_should_contain_the_country_ID(string countryCode = "", string key = "")
        {
            Guid countryId = MyCountryCodesAndIds[countryCode];

            Then_the_problem_details_extensions_should_contain(key, countryId);
        }

        private Competitor[] ParseExpectedCompetitors(string multiLineTable) => multiLineTable.ParseItems<ExpectedCompetitor>()
            .Select(item => new Competitor
            {
                FinishingPosition = item.FinishingPosition,
                CompetingCountryId = MyCountryCodesAndIds[item.CountryCode],
                RunningOrderPosition = item.RunningOrderPosition,
                JuryAwards = item.JuryAwards,
                TelevoteAwards = item.TelevoteAwards
            })
            .OrderBy(item => item.FinishingPosition)
            .ToArray();

        private Voter[] ParseExpectedVoters(string multiLineTable) => multiLineTable.ParseItems<ExpectedVoter>()
            .Select(item => new Voter(MyCountryCodesAndIds[item.CountryCode], item.PointsAwarded))
            .OrderBy(item => item.PointsAwarded)
            .ThenBy(item => item.VotingCountryId)
            .ToArray();

        [UsedImplicitly]
        private sealed record ExpectedCompetitor
        {
            public required int FinishingPosition { get; [UsedImplicitly] init; }

            public required string CountryCode { get; [UsedImplicitly] init; }

            public required int RunningOrderPosition { get; [UsedImplicitly] init; }

            [TypeConverter(typeof(AwardArrayConverter))]
            public required Award[] JuryAwards { get; [UsedImplicitly] init; }

            [TypeConverter(typeof(AwardArrayConverter))]
            public required Award[] TelevoteAwards { get; [UsedImplicitly] init; }
        }

        [UsedImplicitly]
        private sealed record ExpectedVoter
        {
            public required string CountryCode { get; [UsedImplicitly] init; }

            public required bool PointsAwarded { get; [UsedImplicitly] init; }
        }

        private sealed class AwardArrayConverter : TypeConverter<Award[]>
        {
            public override Award[]? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData) => text
                ?.TrimStart('[')
                .TrimEnd(']')
                .Split(',')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(item =>
                {
                    string[] elements = item.Split(':');

                    return new Award(Guid.Parse(elements[0]), int.Parse(elements[1]));
                }).ToArray();

            public override string? ConvertToString(Award[]? value, IWriterRow row, MemberMapData memberMapData)
            {
                if (value is null)
                {
                    return null;
                }

                return "["
                       + string.Join(",", value.Select(item => $"{item.VotingCountryId}:{item.PointsValue}"))
                       + "]";
            }
        }
    }
}

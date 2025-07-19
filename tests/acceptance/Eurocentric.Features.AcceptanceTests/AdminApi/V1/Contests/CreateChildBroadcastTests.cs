using System.ComponentModel;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Responses;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public static partial class CreateChildBroadcastTests
{
    public sealed partial class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_child_broadcast_for_non_existent_contest(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            await admin.Given_I_have_created_a_Liverpool_format_contest(contestYear: 2025,
                cityName: "Basel",
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            await admin.Given_I_have_deleted_my_contest();
            admin.Given_I_want_to_create_a_child_broadcast_for_my_contest(contestStage: "SemiFinal1",
                broadcastDate: "2025-05-01",
                competingCountryCodes: ["AT", "BE"]);

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(404);
            admin.Then_the_response_problem_details_should_match(status: 404,
                title: "Contest not found",
                detail: "No contest exists with the provided contest ID.");
            admin.Then_the_response_problem_details_extensions_should_include_the_ID_of_my_contest();
        }
    }

    private sealed class Admin : AdminActorWithResponse<CreateChildBroadcastResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, IRequestFactory requestFactory) :
            base(restClient, backDoor, requestFactory)
        {
        }

        public void Given_I_want_to_create_a_child_broadcast_for_my_contest(string[] competingCountryCodes = null!,
            string broadcastDate = "",
            string contestStage = "")
        {
            Contest myContest = GivenContests.GetSingle();

            CreateChildBroadcastRequest requestBody = new()
            {
                ContestStage = Enum.Parse<ContestStage>(contestStage),
                BroadcastDate = DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
                CompetingCountryIds = competingCountryCodes.Select(GivenCountries.LookupId).ToArray()
            };

            Request = RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public void Given_I_want_to_create_a_child_broadcast_for_my_contest_with_contest_stage(string contestStage)
        {
            Contest myContest = GivenContests.GetSingle();

            ContestStage stage = Enum.Parse<ContestStage>(contestStage);

            Func<Participant, bool> participantPredicate = stage switch
            {
                ContestStage.SemiFinal1 => p => p.ParticipantGroup == 1,
                ContestStage.SemiFinal2 => p => p.ParticipantGroup == 2,
                ContestStage.GrandFinal => p => p.ParticipantGroup is 1 or 2,
                _ => throw new InvalidEnumArgumentException(nameof(stage), (int)stage, typeof(ContestStage))
            };

            CreateChildBroadcastRequest requestBody = new()
            {
                ContestStage = stage,
                BroadcastDate = new DateOnly(myContest.ContestYear, 05, 31),
                CompetingCountryIds = myContest.Participants.Where(participantPredicate)
                    .Take(3)
                    .Select(participant => participant.ParticipatingCountryId)
                    .ToArray()
            };

            Request = RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public void Given_I_want_to_create_a_child_broadcast_for_my_contest_with_broadcast_date(string broadcastDate)
        {
            Contest myContest = GivenContests.GetSingle();

            CreateChildBroadcastRequest requestBody = new()
            {
                ContestStage = ContestStage.GrandFinal,
                BroadcastDate = DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
                CompetingCountryIds = myContest.Participants.Where(participant => participant.ParticipantGroup is 1 or 2)
                    .Take(3)
                    .Select(participant => participant.ParticipatingCountryId)
                    .ToArray()
            };

            Request = RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public void Given_I_want_to_create_a_SemiFinal1_child_broadcast_for_my_contest_with_competing_countries(
            params string[] competingCountryCodes)
        {
            Contest myContest = GivenContests.GetSingle();

            CreateChildBroadcastRequest requestBody = new()
            {
                ContestStage = ContestStage.SemiFinal1,
                BroadcastDate = new DateOnly(myContest.ContestYear, 05, 31),
                CompetingCountryIds = competingCountryCodes.Select(GivenCountries.LookupId).ToArray()
            };

            Request = RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public void Given_I_want_to_create_a_SemiFinal2_child_broadcast_for_my_contest_with_competing_countries(
            params string[] competingCountryCodes)
        {
            Contest myContest = GivenContests.GetSingle();

            CreateChildBroadcastRequest requestBody = new()
            {
                ContestStage = ContestStage.SemiFinal2,
                BroadcastDate = new DateOnly(myContest.ContestYear, 05, 31),
                CompetingCountryIds = competingCountryCodes.Select(GivenCountries.LookupId).ToArray()
            };

            Request = RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public void Given_I_want_to_create_a_GrandFinal_child_broadcast_for_my_contest_with_competing_countries(
            params string[] competingCountryCodes)
        {
            Contest myContest = GivenContests.GetSingle();

            CreateChildBroadcastRequest requestBody = new()
            {
                ContestStage = ContestStage.GrandFinal,
                BroadcastDate = new DateOnly(myContest.ContestYear, 05, 31),
                CompetingCountryIds = competingCountryCodes.Select(GivenCountries.LookupId).ToArray()
            };

            Request = RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public void Then_the_created_broadcast_should_match(string competitors = "",
            string broadcastDate = "",
            string contestStage = "",
            string? juries = null,
            string televotes = "",
            bool completed = false)
        {
            Assert.NotNull(ResponseObject);

            Broadcast createdBroadcast = ResponseObject.Broadcast;

            DateOnly expectedBroadcastDate = DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd");
            ContestStage expectedContestStage = Enum.Parse<ContestStage>(contestStage);

            IOrderedEnumerable<Competitor> expectedCompetitors = MarkdownParser.ParseTable(competitors, MapRowToCompetitor)
                .OrderBy(competitor => competitor.FinishingPosition);
            IOrderedEnumerable<Competitor> actualCompetitors = createdBroadcast.Competitors
                .OrderBy(competitor => competitor.FinishingPosition);

            IEnumerable<Voter> expectedJuries = juries is null
                ? Enumerable.Empty<Voter>()
                : MarkdownParser.ParseTable(juries, MapRowToVoter)
                    .OrderBy(voter => voter.VotingCountryId);
            IOrderedEnumerable<Voter> actualJuries = createdBroadcast.Juries
                .OrderBy(voter => voter.VotingCountryId);

            IOrderedEnumerable<Voter> expectedTelevotes = MarkdownParser.ParseTable(televotes, MapRowToVoter)
                .OrderBy(voter => voter.VotingCountryId);
            IOrderedEnumerable<Voter> actualTelevotes = createdBroadcast.Televotes
                .OrderBy(voter => voter.VotingCountryId);

            Assert.Equal(expectedBroadcastDate, createdBroadcast.BroadcastDate);
            Assert.Equal(expectedContestStage, createdBroadcast.ContestStage);
            Assert.Equal(completed, createdBroadcast.Completed);
            Assert.Equal(expectedCompetitors, actualCompetitors, new CompetitorEqualityComparer());
            Assert.Equal(expectedJuries, actualJuries);
            Assert.Equal(expectedTelevotes, actualTelevotes);
        }

        public void Then_the_created_broadcast_should_reference_my_contest_as_its_parent_contest()
        {
            Assert.NotNull(ResponseObject);

            Contest myContest = GivenContests.GetSingle();
            Broadcast createdBroadcast = ResponseObject.Broadcast;

            Assert.Equal(myContest.Id, createdBroadcast.ParentContestId);
        }

        public async Task Then_the_created_broadcast_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(ResponseObject);

            Broadcast createdBroadcast = ResponseObject.Broadcast;
            Broadcast retrievedBroadcast = await GetExistingBroadcastByIdAsync(createdBroadcast.Id);

            Assert.Equal(createdBroadcast, retrievedBroadcast, new BroadcastEqualityComparer());
        }

        public async Task Then_my_contest_should_now_reference_the_created_broadcast_as_one_of_its_child_broadcasts()
        {
            Assert.NotNull(ResponseObject);
            Contest myContest = GivenContests.GetSingle();

            Contest retrievedContest = await GetExistingContestByIdAsync(myContest.Id);
            var (broadcastId, contestStage) = (ResponseObject.Broadcast.Id, ResponseObject.Broadcast.ContestStage);

            Assert.Contains(retrievedContest.ChildBroadcasts, memo =>
                memo.BroadcastId == broadcastId && memo.ContestStage == contestStage && memo.Completed == false);
        }

        public async Task Then_my_contest_should_be_unchanged()
        {
            Contest myContest = GivenContests.GetSingle();
            Contest retrievedContest = await GetExistingContestByIdAsync(myContest.Id);

            Assert.Equal(myContest, retrievedContest, new ContestEqualityComparer());
        }

        public void Then_the_response_problem_details_extensions_should_include_the_ID_of_my_contest()
        {
            Guid expectedContestId = GivenContests.GetSingle().Id;

            this.Then_the_response_problem_details_extensions_should_contain(key: "contestId",
                value: expectedContestId.ToString());
        }

        public void Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country_with_country_code(
            string countryCode)
        {
            Guid expectedCountryId = GivenCountries.LookupId(countryCode);

            this.Then_the_response_problem_details_extensions_should_contain(key: "countryId",
                value: expectedCountryId.ToString());
        }

        public async Task Then_my_given_broadcast_should_be_the_only_existing_contest()
        {
            Broadcast expectedBroadcast = GivenBroadcasts.GetSingle();

            Broadcast[] existingBroadcasts = await this.GetAllExistingBroadcastsAsync();

            Broadcast existingBroadcast = Assert.Single(existingBroadcasts);

            Assert.Equal(expectedBroadcast, existingBroadcast, new BroadcastEqualityComparer());
        }

        private async Task<Broadcast> GetExistingBroadcastByIdAsync(Guid broadcastId)
        {
            RestRequest request = RequestFactory.Broadcasts.GetBroadcast(broadcastId);

            ProblemOrResponse<GetBroadcastResponse> problemOrResponse =
                await RestClient.SendAsync<GetBroadcastResponse>(request, TestContext.Current.CancellationToken);

            return problemOrResponse.AsResponse.Data!.Broadcast;
        }

        private async Task<Contest> GetExistingContestByIdAsync(Guid contestId)
        {
            RestRequest request = RequestFactory.Contests.GetContest(contestId);

            ProblemOrResponse<GetContestResponse> problemOrResponse =
                await RestClient.SendAsync<GetContestResponse>(request, TestContext.Current.CancellationToken);

            return problemOrResponse.AsResponse.Data!.Contest;
        }

        private Competitor MapRowToCompetitor(Dictionary<string, string> row) => new()
        {
            CompetingCountryId = GivenCountries.LookupId(row["CountryCode"]),
            RunningOrderPosition = int.Parse(row["RunningOrder"]),
            FinishingPosition = int.Parse(row["Finishing"]),
            JuryAwards = [],
            TelevoteAwards = []
        };

        private Voter MapRowToVoter(Dictionary<string, string> row) => new()
        {
            VotingCountryId = GivenCountries.LookupId(row["CountryCode"]), PointsAwarded = bool.Parse(row["PointsAwarded"])
        };
    }
}

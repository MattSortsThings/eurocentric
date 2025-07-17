using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public static partial class CreateContestTests
{
    public sealed partial class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture);

    private sealed partial class Admin : AdminActorWithResponse<CreateContestResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, IRequestFactory requestFactory) :
            base(restClient, backDoor, requestFactory)
        {
        }

        public void Given_I_want_to_create_a_contest(string cityName = "",
            int contestYear = 0,
            string group2Participants = "",
            string group1Participants = "",
            string? group0ParticipatingCountryCode = null,
            string contestFormat = "")
        {
            CreateContestRequest requestBody = new()
            {
                ContestYear = contestYear,
                CityName = cityName,
                ContestFormat = Enum.Parse<ContestFormat>(contestFormat),
                Group1Participants = MarkdownParser.ParseTable(group1Participants, MapRowToContestParticipantSpecification),
                Group2Participants = MarkdownParser.ParseTable(group2Participants, MapRowToContestParticipantSpecification),
                Group0ParticipatingCountryId = group0ParticipatingCountryCode is null
                    ? null
                    : GivenCountries.LookupId(group0ParticipatingCountryCode)
            };

            Request = RequestFactory.Contests.CreateContest(requestBody);
        }

        public void Then_the_created_contest_should_match(string cityName = "",
            int contestYear = 0,
            string participants = "",
            string contestFormat = "",
            bool completed = false,
            int childBroadcasts = 0)
        {
            Assert.NotNull(ResponseObject);

            Contest createdContest = ResponseObject.Contest;

            ContestFormat expectedFormat = Enum.Parse<ContestFormat>(contestFormat);
            IOrderedEnumerable<Participant> expectedParticipants = MarkdownParser.ParseTable(participants, MapRowToParticipant)
                .OrderBy(participant => participant.ParticipantGroup)
                .ThenBy(participant => participant.ParticipatingCountryId);

            Assert.Equal(contestYear, createdContest.ContestYear);
            Assert.Equal(cityName, createdContest.CityName);
            Assert.Equal(expectedFormat, createdContest.ContestFormat);
            Assert.Equal(completed, createdContest.Completed);
            Assert.Equal(childBroadcasts, createdContest.ChildBroadcasts.Length);
            Assert.Equal(expectedParticipants, createdContest.Participants);
        }

        public void Then_the_response_problem_details_should_have_a_countryId_extension_with_the_ID_of_the_country(
            string countryCode)
        {
            Assert.NotNull(ResponseProblemDetails);

            Guid expectedCountryId = GivenCountries.LookupId(countryCode);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "countryId", Value: JsonElement je }
                                                                      && je.GetGuid() == expectedCountryId);
        }

        public void Then_the_response_problem_details_should_have_a_contestYear_extension_with(int contestYear)
        {
            Assert.NotNull(ResponseProblemDetails);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "contestYear", Value: JsonElement je }
                                                                      && je.GetInt32() == contestYear);
        }

        public void Then_the_response_problem_details_should_have_a_cityName_extension_with(string cityName)
        {
            Assert.NotNull(ResponseProblemDetails);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "cityName", Value: JsonElement je }
                                                                      && je.GetString() == cityName);
        }

        public void Then_the_response_problem_details_should_have_an_actName_extension_with(string actName)
        {
            Assert.NotNull(ResponseProblemDetails);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "actName", Value: JsonElement je }
                                                                      && je.GetString() == actName);
        }


        public void Then_the_response_problem_details_should_have_a_songTitle_extension_with(string songTitle)
        {
            Assert.NotNull(ResponseProblemDetails);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "songTitle", Value: JsonElement je }
                                                                      && je.GetString() == songTitle);
        }

        public async Task Then_the_created_contest_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(ResponseObject);

            Contest createdContest = ResponseObject.Contest;
            Contest retrievedContest = await GetExistingContestByIdAsync(createdContest.Id);

            Assert.Equal(createdContest, retrievedContest, new ContestEqualityComparer());
        }

        public async Task Then_no_contests_should_exist()
        {
            Contest[] existingContests = await GetAllExistingContestsAsync();

            Assert.Empty(existingContests);
        }

        public async Task Then_my_given_contest_should_be_the_only_existing_contest()
        {
            Contest expectedContest = GivenContests.GetSingle();

            Contest[] existingContests = await GetAllExistingContestsAsync();

            Contest existingContest = Assert.Single(existingContests);

            Assert.Equal(expectedContest, existingContest, new ContestEqualityComparer());
        }

        private async Task<Contest> GetExistingContestByIdAsync(Guid contestId)
        {
            RestRequest request = RequestFactory.Contests.GetContest(contestId);

            ProblemOrResponse<GetContestResponse> problemOrResponse =
                await RestClient.SendAsync<GetContestResponse>(request, TestContext.Current.CancellationToken);

            return problemOrResponse.AsResponse.Data!.Contest;
        }

        private async Task<Contest[]> GetAllExistingContestsAsync()
        {
            RestRequest request = RequestFactory.Contests.GetContests();

            ProblemOrResponse<GetContestsResponse> problemOrResponse =
                await RestClient.SendAsync<GetContestsResponse>(request, TestContext.Current.CancellationToken);

            return problemOrResponse.AsResponse.Data!.Contests;
        }

        private ContestParticipantSpecification MapRowToContestParticipantSpecification(Dictionary<string, string> row) =>
            new(GivenCountries.LookupId(row["CountryCode"]), row["ActName"], row["SongTitle"]);

        private Participant MapRowToParticipant(Dictionary<string, string> row) => new()
        {
            ParticipatingCountryId = GivenCountries.LookupId(row["CountryCode"]),
            ParticipantGroup = int.Parse(row["Group"]),
            ActName = string.IsNullOrWhiteSpace(row["ActName"]) ? null : row["ActName"],
            SongTitle = string.IsNullOrWhiteSpace(row["SongTitle"]) ? null : row["SongTitle"]
        };
    }
}

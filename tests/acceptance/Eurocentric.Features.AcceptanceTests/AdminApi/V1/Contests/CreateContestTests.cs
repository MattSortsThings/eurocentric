using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public static partial class CreateContestTests
{
    public sealed partial class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture);

    private sealed partial class Admin : AdminActor<CreateContestResponse>
    {
        private const string DefaultCityName = "CityName";
        private const int DefaultContestYear = 2025;

        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
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
                    : GivenCountries.GetId(group0ParticipatingCountryCode)
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

        public void Then_the_problem_details_extensions_should_contain_the_country_ID_for_the_country(string countryCode)
        {
            Assert.NotNull(ResponseProblemDetails);
            Guid expectedId = GivenCountries.GetId(countryCode);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "countryId", Value: JsonElement je }
                                                                      && je.GetGuid() == expectedId);
        }

        public async Task Then_the_created_contest_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(ResponseObject);

            Contest createdContest = ResponseObject.Contest;
            Contest retrievedContest = await GetAllContestByIdAsync(createdContest.Id);

            Assert.Equal(createdContest, retrievedContest, new ContestEqualityComparer());
        }

        public async Task Then_my_given_contest_should_be_the_only_existing_contest()
        {
            Assert.Single(GivenContests);

            Contest[] existingContests = await GetAllContestsAsync();

            Assert.Equal(GivenContests, existingContests, new ContestEqualityComparer());
        }

        public async Task Then_no_contests_should_exist()
        {
            Contest[] existingContests = await GetAllContestsAsync();

            Assert.Empty(existingContests);
        }

        private async Task<Contest> GetAllContestByIdAsync(Guid contestId)
        {
            RestRequest request = RequestFactory.Contests.GetContest(contestId);

            ProblemOrResponse<GetContestResponse> problemOrResponse =
                await RestClient.SendAsync<GetContestResponse>(request, TestContext.Current.CancellationToken);

            return problemOrResponse.AsResponse.Data!.Contest;
        }

        private async Task<Contest[]> GetAllContestsAsync()
        {
            RestRequest request = RequestFactory.Contests.GetContests();

            ProblemOrResponse<GetContestsResponse> problemOrResponse =
                await RestClient.SendAsync<GetContestsResponse>(request, TestContext.Current.CancellationToken);

            return problemOrResponse.AsResponse.Data!.Contests;
        }

        private ContestParticipantSpecification MapRowToContestParticipantSpecification(Dictionary<string, string> row) =>
            new(GivenCountries.GetId(row["CountryCode"]), row["ActName"], row["SongTitle"]);

        private Participant MapRowToParticipant(Dictionary<string, string> row)
        {
            Dictionary<string, string> r = row;

            return new Participant
            {
                ParticipatingCountryId = GivenCountries.GetId(row["CountryCode"]),
                ParticipantGroup = int.Parse(row["Group"]),
                ActName = string.IsNullOrWhiteSpace(row["ActName"]) ? null : row["ActName"],
                SongTitle = string.IsNullOrWhiteSpace(row["SongTitle"]) ? null : row["SongTitle"]
            };
        }
    }
}

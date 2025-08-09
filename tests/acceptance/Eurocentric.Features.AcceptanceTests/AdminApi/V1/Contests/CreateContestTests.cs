using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests.CreateContest;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed partial class CreateContestTests : SerialCleanAcceptanceTest
{
    private sealed partial class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<CreateContestResponse>(apiDriver)
    {
        private Dictionary<string, Guid> CountryCodesAndIds { get; } = new(10);

        private Contest? Contest { get; set; }

        private Guid? DeletedCountryId { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            List<Country> createdCountries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);

            foreach (Country country in createdCountries)
            {
                CountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_deleted_my_country(string countryCode)
        {
            Guid myCountryId = CountryCodesAndIds[countryCode];

            await ApiDriver.DeleteSingleCountryAsync(myCountryId);

            CountryCodesAndIds.Remove(countryCode);
            DeletedCountryId = myCountryId;
        }

        public void Given_I_want_to_create_a_contest_for_my_countries(string group2Participants = "",
            string group1Participants = "",
            string? group0CountryCode = null,
            string contestFormat = "",
            string cityName = "",
            int contestYear = 0)
        {
            CreateContestRequest requestBody = new()
            {
                ContestFormat = Enum.Parse<ContestFormat>(contestFormat),
                CityName = cityName,
                ContestYear = contestYear,
                Group0ParticipatingCountryId =
                    group0CountryCode is not null ? CountryCodesAndIds[group0CountryCode] : null,
                Group1ParticipantData = MarkdownParser.ParseTable(group1Participants, MapRowToParticipantDatum).ToArray(),
                Group2ParticipantData = MarkdownParser.ParseTable(group2Participants, MapRowToParticipantDatum).ToArray()
            };

            Request = ApiDriver.RequestFactory.Contests.CreateContest(requestBody);
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_contest_year(int contestYear) =>
            await Assert.That(ResponseProblemDetails).IsNotNull()
                .And.HasExtension("contestYear", contestYear);

        public async Task Then_the_response_problem_details_extensions_should_include_the_city_name(string cityName) =>
            await Assert.That(ResponseProblemDetails).IsNotNull()
                .And.HasExtension("cityName", cityName);

        public async Task Then_the_response_problem_details_extensions_should_include_the_act_name(string actName) =>
            await Assert.That(ResponseProblemDetails).IsNotNull()
                .And.HasExtension("actName", actName);

        public async Task Then_the_response_problem_details_extensions_should_include_the_song_title(string songTitle) =>
            await Assert.That(ResponseProblemDetails).IsNotNull()
                .And.HasExtension("songTitle", songTitle);

        public async Task Then_the_response_problem_details_extensions_should_include_my_deleted_country_ID()
        {
            Guid myDeletedCountryId = await Assert.That(DeletedCountryId).IsNotNull();

            await Assert.That(ResponseProblemDetails).IsNotNull()
                .And.HasExtension("countryId", myDeletedCountryId);
        }

        public async Task Then_the_created_contest_should_match(string participants = "",
            bool completed = true,
            string contestFormat = "",
            string cityName = "",
            int contestYear = 0)
        {
            CreateContestResponse responseBody = await Assert.That(ResponseBody).IsNotNull();
            Contest createdContest = responseBody.Contest;

            ContestFormat expectedContestFormat = Enum.Parse<ContestFormat>(contestFormat);

            Participant[] expectedParticipants = MarkdownParser.ParseTable(participants, MapRowToParticipant).ToArray();

            await Assert.That(createdContest)
                .HasMember(contest => contest.ContestYear).EqualTo(contestYear)
                .And.HasMember(contest => contest.CityName).EqualTo(cityName)
                .And.HasMember(contest => contest.ContestFormat).EqualTo(expectedContestFormat)
                .And.HasMember(contest => contest.Completed).EqualTo(completed)
                .And.Satisfies(contest => contest.Participants, assert =>
                    assert.IsEquivalentTo(expectedParticipants, EqualityComparer<Participant>.Default, CollectionOrdering.Any));
        }

        public async Task Then_the_created_contest_should_have_no_child_broadcasts()
        {
            CreateContestResponse responseBody = await Assert.That(ResponseBody).IsNotNull();
            Contest createdContest = responseBody.Contest;

            await Assert.That(createdContest.ChildBroadcasts).IsEmpty();
        }

        public async Task Then_my_given_contest_should_be_the_only_existing_contest_in_the_system()
        {
            Contest myGivenContest = await Assert.That(Contest).IsNotNull();

            Contest[] existingContests = await ApiDriver.GetAllContestsAsync();

            Contest? singleExistingContest = await Assert.That(existingContests).HasSingleItem();

            await Assert.That(singleExistingContest).IsNotNull()
                .And.IsEqualTo(myGivenContest, new ContestEqualityComparer());
        }

        public async Task Then_the_created_contest_should_be_the_only_existing_contest_in_the_system()
        {
            CreateContestResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            Contest[] existingContests = await ApiDriver.GetAllContestsAsync();

            Contest? singleExistingContest = await Assert.That(existingContests).HasSingleItem();

            await Assert.That(singleExistingContest)
                .IsNotNull()
                .And.IsEqualTo(responseBody.Contest, new ContestEqualityComparer());
        }

        public async Task Then_no_contests_should_exist_in_the_system()
        {
            Contest[] existingCountries = await ApiDriver.GetAllContestsAsync();

            await Assert.That(existingCountries).IsEmpty();
        }

        private Participant MapRowToParticipant(Dictionary<string, string> row) => new()
        {
            ParticipantGroup = int.Parse(row["Group"]),
            ParticipatingCountryId = CountryCodesAndIds[row["CountryCode"]],
            ActName = string.IsNullOrWhiteSpace(row["ActName"]) ? null : row["ActName"],
            SongTitle = string.IsNullOrWhiteSpace(row["SongTitle"]) ? null : row["SongTitle"]
        };

        private ContestParticipantDatum MapRowToParticipantDatum(Dictionary<string, string> row) => new()
        {
            ParticipatingCountryId = CountryCodesAndIds[row["CountryCode"]],
            ActName = row["ActName"],
            SongTitle = row["SongTitle"]
        };
    }
}

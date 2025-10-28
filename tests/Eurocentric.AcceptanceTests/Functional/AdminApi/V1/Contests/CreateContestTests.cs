using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests;

[Category("admin-api")]
public sealed partial class CreateContestTests : SerialCleanAcceptanceTest
{
    private sealed partial class Admin(AdminKernel kernel) : AdminActor<CreateContestResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private CountryIdLookup ExistingCountryIds { get; } = new();

        private Guid? DeletedCountryId { get; set; }

        private Contest? ExistingContest { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            ExistingCountryIds.EnsureCapacity(countryCodes.Length);

            await foreach (Country country in Kernel.CreateMultipleCountriesAsync(countryCodes))
            {
                ExistingCountryIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_contest_with_contest_year(int contestYear)
        {
            Guid[] allCountryIds = ExistingCountryIds.GetAllIds();

            ExistingContest = await Kernel.CreateAStockholmRulesContestAsync(
                contestYear: contestYear,
                semiFinal1CountryIds: allCountryIds.Take(3).ToArray(),
                semiFinal2CountryIds: allCountryIds.Skip(3).Take(3).ToArray()
            );
        }

        public void Given_I_want_to_create_a_contest(
            string participants = "",
            string? globalTelevoteCountry = null,
            string cityName = "",
            int contestYear = 0,
            string contestRules = ""
        )
        {
            CreateContestRequest requestBody = new()
            {
                ContestYear = contestYear,
                CityName = cityName,
                ContestRules = Enum.Parse<ContestRules>(contestRules),
                GlobalTelevoteVotingCountryId = globalTelevoteCountry is null
                    ? null
                    : ExistingCountryIds.GetId(globalTelevoteCountry),
                Participants = MarkdownParser.ParseTable(participants, MapToCreateParticipantRequest),
            };

            Request = Kernel.Requests.Contests.CreateContest(requestBody);
        }

        public async Task Given_I_have_deleted_my_country(string countryCode)
        {
            Guid countryId = ExistingCountryIds.Remove(countryCode);

            await Kernel.DeleteACountryAsync(countryId);

            DeletedCountryId = countryId;
        }

        public async Task Then_the_response_headers_should_include_the_created_contest_location_for_API_version(
            string apiVersion
        )
        {
            Guid createdContestId = await Assert.That(SuccessResponse?.Data?.Contest.Id).IsNotNull();

            string expectedLocationSuffix = $"/admin/api/{apiVersion}/contests/{createdContestId}";

            await Assert
                .That(SuccessResponse?.Headers)
                .Contains(headerParameter =>
                    headerParameter.Name == "Location" && headerParameter.Value.EndsWith(expectedLocationSuffix)
                );
        }

        public async Task Then_the_created_contest_should_match(
            string participants = "",
            string? globalTelevoteCountry = null,
            string cityName = "",
            int contestYear = 0,
            string contestRules = "",
            bool queryable = true
        )
        {
            Participant[] expectedParticipants = MarkdownParser.ParseTable(participants, MapToParticipant);

            GlobalTelevote? expectedGlobalTelevote = globalTelevoteCountry is null
                ? null
                : new GlobalTelevote { VotingCountryId = ExistingCountryIds.GetId(globalTelevoteCountry) };

            ContestRules expectedContestRules = Enum.Parse<ContestRules>(contestRules);

            await Assert
                .That(SuccessResponse?.Data?.Contest)
                .HasProperty(contest => contest.ContestYear, contestYear)
                .And.HasProperty(contest => contest.CityName, cityName)
                .And.HasProperty(contest => contest.ContestRules, expectedContestRules)
                .And.HasProperty(contest => contest.Queryable, queryable)
                .And.Member(contest => contest.GlobalTelevote, source => source.IsEqualTo(expectedGlobalTelevote))
                .And.Member(
                    contest => contest.Participants,
                    collection => collection.IsEquivalentTo(expectedParticipants, new ParticipantEqualityComparer())
                );
        }

        public async Task Then_the_created_contest_should_have_no_child_broadcasts() =>
            await Assert.That(SuccessResponse?.Data?.Contest.ChildBroadcasts).IsEmpty();

        public async Task Then_the_created_contest_should_be_the_only_existing_contest()
        {
            Contest createdContest = await Assert.That(SuccessResponse?.Data?.Contest).IsNotNull();

            Contest[] existingContests = await Kernel.GetAllContestsAsync();

            await Assert
                .That(existingContests)
                .HasSingleItem()
                .And.Contains(createdContest, new ContestEqualityComparer());
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_deleted_country_ID()
        {
            Guid deletedCountryId = await Assert.That(DeletedCountryId).IsNotNull();

            await Assert.That(FailureResponse?.Data).HasExtension("countryId", deletedCountryId);
        }

        public async Task Then_there_should_be_no_existing_contests()
        {
            Contest[] existingContests = await Kernel.GetAllContestsAsync();

            await Assert.That(existingContests).IsEmpty();
        }

        public async Task Then_my_existing_contest_should_be_the_only_existing_contest()
        {
            Contest existingContest = await Assert.That(ExistingContest).IsNotNull();

            Contest[] existingContests = await Kernel.GetAllContestsAsync();

            await Assert
                .That(existingContests)
                .HasSingleItem()
                .And.Contains(existingContest, new ContestEqualityComparer());
        }

        private CreateParticipantRequest MapToCreateParticipantRequest(Dictionary<string, string> row)
        {
            return new CreateParticipantRequest
            {
                ParticipatingCountryId = ExistingCountryIds.GetId(row["ParticipatingCountry"]),
                SemiFinalDraw = Enum.Parse<SemiFinalDraw>(row["SemiFinalDraw"]),
                ActName = row["ActName"],
                SongTitle = row["SongTitle"],
            };
        }

        private Participant MapToParticipant(Dictionary<string, string> row)
        {
            return new Participant
            {
                ParticipatingCountryId = ExistingCountryIds.GetId(row["ParticipatingCountry"]),
                SemiFinalDraw = Enum.Parse<SemiFinalDraw>(row["SemiFinalDraw"]),
                ActName = row["ActName"],
                SongTitle = row["SongTitle"],
            };
        }
    }
}

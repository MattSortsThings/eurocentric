using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests;

public sealed partial class CreateContestBroadcastTests : SerialCleanAcceptanceTest
{
    private sealed partial class Admin(AdminKernel kernel) : AdminActor<CreateContestBroadcastResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private CountryIdLookup ExistingCountryIds { get; } = new();

        private Contest? ExistingContest { get; set; }

        private Broadcast? ExistingBroadcast { get; set; }

        private Guid? DeletedContestId { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            ExistingCountryIds.EnsureCapacity(countryCodes.Length);

            await foreach (Country country in Kernel.CreateMultipleCountriesAsync(countryCodes))
            {
                ExistingCountryIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_broadcast_for_my_contest(
            string?[] competingCountries = null!,
            string contestStage = "",
            string broadcastDate = ""
        )
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            ExistingBroadcast = await Kernel.CreateABroadcastAsync(
                contestId: contestId,
                broadcastDate: DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                contestStage: Enum.Parse<ContestStage>(contestStage),
                competingCountryIds: ExistingCountryIds.MapToNullableIds(competingCountries)
            );

            ExistingContest = await Kernel.GetAContestAsync(contestId);
        }

        public async Task Given_I_want_to_create_a_broadcast_for_my_contest(
            string?[] competingCountries = null!,
            string contestStage = "",
            string broadcastDate = ""
        )
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            CreateContestBroadcastRequest requestBody = new()
            {
                BroadcastDate = DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                ContestStage = Enum.Parse<ContestStage>(contestStage),
                CompetingCountryIds = ExistingCountryIds.MapToNullableIds(competingCountries),
            };

            Request = Kernel.Requests.Contests.CreateContestBroadcast(contestId, requestBody);
        }

        public async Task Given_I_have_deleted_my_contest()
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            await Kernel.DeleteAContestAsync(contestId);

            ExistingContest = null;
            DeletedContestId = contestId;
        }

        public async Task Then_the_response_headers_should_include_the_created_broadcast_location_for_API_version(
            string apiVersion
        )
        {
            Guid createdBroadcastId = await Assert.That(SuccessResponse?.Data?.Broadcast.Id).IsNotNull();

            string expectedLocationSuffix = $"/admin/api/{apiVersion}/broadcasts/{createdBroadcastId}";

            await Assert
                .That(SuccessResponse?.Headers)
                .Contains(headerParameter =>
                    headerParameter.Name == "Location" && headerParameter.Value.EndsWith(expectedLocationSuffix)
                );
        }

        public async Task Then_the_created_broadcast_should_reference_my_existing_contest_as_its_parent_contest()
        {
            Guid existingContestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            await Assert
                .That(SuccessResponse?.Data?.Broadcast)
                .HasProperty(broadcast => broadcast.ParentContestId, existingContestId);
        }

        public async Task Then_the_created_broadcast_should_match(
            string? televotes = null,
            string? juries = null,
            string competitors = "",
            bool completed = true,
            string contestStage = "",
            string broadcastDate = ""
        )
        {
            DateOnly expectedBroadcastDate = DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat);
            ContestStage expectedContestStage = Enum.Parse<ContestStage>(contestStage);
            Competitor[] expectedCompetitors = MarkdownParser.ParseTable(competitors, MapToCompetitor);
            Jury[] expectedJuries = MarkdownParser.ParseTable(juries, MapToJury);
            Televote[] expectedTelevotes = MarkdownParser.ParseTable(televotes, MapToTelevote);

            await Assert
                .That(SuccessResponse?.Data?.Broadcast)
                .HasProperty(broadcast => broadcast.BroadcastDate, expectedBroadcastDate)
                .And.HasProperty(broadcast => broadcast.ContestStage, expectedContestStage)
                .And.HasProperty(broadcast => broadcast.Completed, completed)
                .And.Member(
                    broadcast => broadcast.Competitors,
                    collection => collection.IsEquivalentTo(expectedCompetitors, new CompetitorEqualityComparer())
                )
                .And.Member(broadcast => broadcast.Juries, collection => collection.IsEquivalentTo(expectedJuries))
                .And.Member(
                    broadcast => broadcast.Televotes,
                    collection => collection.IsEquivalentTo(expectedTelevotes)
                );
        }

        public async Task Then_the_created_broadcast_should_be_the_only_existing_broadcast()
        {
            Broadcast createdBroadcast = await Assert.That(SuccessResponse?.Data?.Broadcast).IsNotNull();
            Broadcast[] existingBroadcasts = await Kernel.GetAllBroadcastsAsync();

            await Assert
                .That(existingBroadcasts)
                .HasSingleItem()
                .And.Contains(createdBroadcast, new BroadcastEqualityComparer());
        }

        public async Task Then_my_existing_broadcast_should_be_the_only_existing_broadcast()
        {
            Broadcast existingBroadcast = await Assert.That(ExistingBroadcast).IsNotNull();
            Broadcast[] existingBroadcasts = await Kernel.GetAllBroadcastsAsync();

            await Assert
                .That(existingBroadcasts)
                .HasSingleItem()
                .And.Contains(existingBroadcast, new BroadcastEqualityComparer());
        }

        public async Task Then_there_should_be_no_existing_broadcasts()
        {
            Broadcast[] existingBroadcasts = await Kernel.GetAllBroadcastsAsync();

            await Assert.That(existingBroadcasts).IsEmpty();
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_deleted_contest_ID()
        {
            Guid deletedContestId = await Assert.That(DeletedContestId).IsNotNull();

            await Assert.That(FailureResponse?.Data).HasExtension("contestId", deletedContestId);
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_contest_ID()
        {
            Guid contestId = await Assert.That(ExistingContest?.Id).IsNotNull();

            await Assert.That(FailureResponse?.Data).HasExtension("contestId", contestId);
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_ID_of_the_country(
            string country
        )
        {
            Guid countryId = ExistingCountryIds.GetId(country);

            await Assert.That(FailureResponse?.Data).HasExtension("countryId", countryId);
        }

        private Competitor MapToCompetitor(Dictionary<string, string> row)
        {
            return new Competitor
            {
                RunningOrderSpot = int.Parse(row["RunningOrderSpot"]),
                CompetingCountryId = ExistingCountryIds.GetId(row["CompetingCountry"]),
                FinishingPosition = int.Parse(row["FinishingPosition"]),
                JuryAwards = [],
                TelevoteAwards = [],
            };
        }

        private Jury MapToJury(Dictionary<string, string> row)
        {
            return new Jury
            {
                VotingCountryId = ExistingCountryIds.GetId(row["VotingCountry"]),
                PointsAwarded = bool.Parse(row["PointsAwarded"]),
            };
        }

        private Televote MapToTelevote(Dictionary<string, string> row)
        {
            return new Televote
            {
                VotingCountryId = ExistingCountryIds.GetId(row["VotingCountry"]),
                PointsAwarded = bool.Parse(row["PointsAwarded"]),
            };
        }
    }
}

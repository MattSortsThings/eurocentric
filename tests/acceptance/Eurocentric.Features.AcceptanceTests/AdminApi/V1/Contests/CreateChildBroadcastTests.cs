using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Contests.CreateChildBroadcast;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed partial class CreateChildBroadcastTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_child_broadcast_for_non_existent_contest(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(
            contestYear: 2016,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_deleted_my_contest();

        await admin.Given_I_want_to_create_a_child_broadcast_for_my_deleted_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_404_NotFound();
        await admin.Then_the_response_problem_details_should_match(status: 404,
            title: "Contest not found",
            detail: "No contest exists with the provided contest ID.");
        await admin.Then_the_response_problem_details_extensions_should_include_my_deleted_contest_ID();
    }

    private sealed partial class AdminActor(IApiDriver apiDriver)
        : AdminActorWithResponse<CreateChildBroadcastResponse>(apiDriver)
    {
        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(8);

        private Contest? MyContest { get; set; }

        private Guid? MyDeletedContestId { get; set; }

        private Broadcast? MyExistingBroadcast { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            List<Country> createdCountries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);

            foreach (Country country in createdCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_SemiFinal1_broadcast_for_my_contest_with_broadcast_date(string broadcastDate)
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            Guid[] competingCountryIds = myContest.Participants.Where(participant => participant.ParticipantGroup == 1)
                .Select(participant => participant.ParticipatingCountryId)
                .ToArray();

            MyExistingBroadcast = await ApiDriver.CreateSingleBroadcastAsync(
                broadcastDate: DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                contestStage: ContestStage.SemiFinal1,
                competingCountryIds: competingCountryIds,
                contestId: myContest.Id);

            MyContest = await ApiDriver.GetSingleContestAsync(myContest.Id);
        }

        public async Task Given_I_have_created_a_SemiFinal2_broadcast_for_my_contest_with_broadcast_date(string broadcastDate)
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            Guid[] competingCountryIds = myContest.Participants.Where(participant => participant.ParticipantGroup == 2)
                .Select(participant => participant.ParticipatingCountryId)
                .ToArray();

            MyExistingBroadcast = await ApiDriver.CreateSingleBroadcastAsync(
                broadcastDate: DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                contestStage: ContestStage.SemiFinal2,
                competingCountryIds: competingCountryIds,
                contestId: myContest.Id);

            MyContest = await ApiDriver.GetSingleContestAsync(myContest.Id);
        }

        public async Task Given_I_have_created_a_GrandFinal_broadcast_for_my_contest_with_broadcast_date(string broadcastDate)
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            Guid[] competingCountryIds = myContest.Participants.Where(participant => participant.ParticipantGroup != 0)
                .Select(participant => participant.ParticipatingCountryId)
                .ToArray();

            MyExistingBroadcast = await ApiDriver.CreateSingleBroadcastAsync(
                broadcastDate: DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                contestStage: ContestStage.GrandFinal,
                competingCountryIds: competingCountryIds,
                contestId: myContest.Id);

            MyContest = await ApiDriver.GetSingleContestAsync(myContest.Id);
        }

        public async Task Given_I_have_deleted_my_contest()
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();
            Guid myContestId = myContest.Id;

            await ApiDriver.DeleteSingleContestAsync(myContestId);

            MyContest = null;
            MyDeletedContestId = myContestId;
        }

        public async Task Given_I_want_to_create_a_child_broadcast_for_my_contest(string[] competingCountryCodes = null!,
            string broadcastDate = "",
            string contestStage = "")
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            CreateChildBroadcastRequest requestBody = new()
            {
                ContestStage = Enum.Parse<ContestStage>(contestStage),
                BroadcastDate = DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                CompetingCountryIds = competingCountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray()
            };

            Request = ApiDriver.RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public async Task Given_I_want_to_create_a_child_broadcast_for_my_deleted_contest()
        {
            Guid myDeletedContestId = await Assert.That(MyDeletedContestId).IsNotNull();

            CreateChildBroadcastRequest requestBody = new()
            {
                BroadcastDate = new DateOnly(2025, 5, 31),
                ContestStage = ContestStage.SemiFinal1,
                CompetingCountryIds = MyCountryCodesAndIds.Values.Take(2).ToArray()
            };

            Request = ApiDriver.RequestFactory.Contests.CreateChildBroadcast(myDeletedContestId, requestBody);
        }

        public async Task Given_I_want_to_create_a_SemiFinal1_child_broadcast_for_my_contest_with_broadcast_date(
            string broadcastDate)
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            CreateChildBroadcastRequest requestBody = new()
            {
                BroadcastDate = DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                ContestStage = ContestStage.SemiFinal1,
                CompetingCountryIds = myContest.Participants.Where(participant => participant.ParticipantGroup == 1)
                    .Select(participant => participant.ParticipatingCountryId)
                    .Take(2)
                    .ToArray()
            };

            Request = ApiDriver.RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public async Task Given_I_want_to_create_a_SemiFinal2_child_broadcast_for_my_contest_with_broadcast_date(
            string broadcastDate)
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            CreateChildBroadcastRequest requestBody = new()
            {
                BroadcastDate = DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                ContestStage = ContestStage.SemiFinal2,
                CompetingCountryIds = myContest.Participants.Where(participant => participant.ParticipantGroup == 2)
                    .Select(participant => participant.ParticipatingCountryId)
                    .Take(2)
                    .ToArray()
            };

            Request = ApiDriver.RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public async Task Given_I_want_to_create_a_GrandFinal_child_broadcast_for_my_contest_with_broadcast_date(
            string broadcastDate)
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            CreateChildBroadcastRequest requestBody = new()
            {
                BroadcastDate = DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                ContestStage = ContestStage.GrandFinal,
                CompetingCountryIds = myContest.Participants.Where(participant => participant.ParticipantGroup != 0)
                    .Select(participant => participant.ParticipatingCountryId)
                    .Take(2)
                    .ToArray()
            };

            Request = ApiDriver.RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public async Task Given_I_want_to_create_a_SemiFinal1_child_broadcast_for_my_contest_with_competing_countries(
            params string[] competingCountryCodes)
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            CreateChildBroadcastRequest requestBody = new()
            {
                BroadcastDate = new DateOnly(myContest.ContestYear, 5, 31),
                ContestStage = ContestStage.SemiFinal1,
                CompetingCountryIds = competingCountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray()
            };

            Request = ApiDriver.RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public async Task Given_I_want_to_create_a_SemiFinal2_child_broadcast_for_my_contest_with_competing_countries(
            params string[] competingCountryCodes)
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            CreateChildBroadcastRequest requestBody = new()
            {
                BroadcastDate = new DateOnly(myContest.ContestYear, 5, 31),
                ContestStage = ContestStage.SemiFinal2,
                CompetingCountryIds = competingCountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray()
            };

            Request = ApiDriver.RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public async Task Given_I_want_to_create_a_GrandFinal_child_broadcast_for_my_contest_with_competing_countries(
            params string[] competingCountryCodes)
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            CreateChildBroadcastRequest requestBody = new()
            {
                BroadcastDate = new DateOnly(myContest.ContestYear, 5, 31),
                ContestStage = ContestStage.GrandFinal,
                CompetingCountryIds = competingCountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray()
            };

            Request = ApiDriver.RequestFactory.Contests.CreateChildBroadcast(myContest.Id, requestBody);
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_deleted_contest_ID()
        {
            ProblemDetails problemDetails = await Assert.That(ResponseProblemDetails).IsNotNull();
            Guid myDeletedContestId = await Assert.That(MyDeletedContestId).IsNotNull();

            await Assert.That(problemDetails).HasExtension("contestId", myDeletedContestId);
        }

        public async Task Then_the_created_broadcast_should_match(string? televotes = null,
            string? juries = null,
            string competitors = "",
            bool completed = true,
            string contestStage = "",
            string broadcastDate = "")
        {
            CreateChildBroadcastResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            Broadcast createdBroadcast = responseBody.Broadcast;

            ContestStage expectedContestStage = Enum.Parse<ContestStage>(contestStage);
            DateOnly expectedBroadcastDate = DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat);
            Voter[] expectedTelevotes = televotes is null
                ? []
                : MarkdownParser.ParseTable(televotes, MapToVoter).OrderBy(voter => voter.VotingCountryId).ToArray();
            Voter[] expectedJuries = juries is null
                ? []
                : MarkdownParser.ParseTable(juries, MapToVoter).OrderBy(voter => voter.VotingCountryId).ToArray();
            Competitor[] expectedCompetitors = MarkdownParser.ParseTable(competitors, MapToCompetitor)
                .OrderBy(competitor => competitor.CompetingCountryId).ToArray();

            await Assert.That(createdBroadcast.BroadcastDate).IsEqualTo(expectedBroadcastDate);
            await Assert.That(createdBroadcast.ContestStage).IsEqualTo(expectedContestStage);
            await Assert.That(createdBroadcast.Completed).IsEqualTo(completed);
            await Assert.That(createdBroadcast.Competitors.OrderBy(competitor => competitor.CompetingCountryId))
                .IsEquivalentTo(expectedCompetitors, new CompetitorEqualityComparer());
            await Assert.That(createdBroadcast.Juries.OrderBy(voter => voter.VotingCountryId)).IsEquivalentTo(expectedJuries);
            await Assert.That(createdBroadcast.Televotes.OrderBy(voter => voter.VotingCountryId))
                .IsEquivalentTo(expectedTelevotes);
        }

        public async Task Then_the_created_broadcast_should_reference_my_contest_as_its_parent_contest()
        {
            CreateChildBroadcastResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            Contest myContest = await Assert.That(MyContest).IsNotNull();

            await Assert.That(responseBody.Broadcast)
                .HasMember(broadcast => broadcast.ParentContestId).EqualTo(myContest.Id);
        }

        public async Task Then_the_created_broadcast_should_be_the_only_existing_broadcast_in_the_system()
        {
            CreateChildBroadcastResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            Broadcast[] existingBroadcasts = await ApiDriver.GetAllBroadcastsAsync();

            Broadcast? singleExistingBroadcast = await Assert.That(existingBroadcasts).HasSingleItem();

            await Assert.That(singleExistingBroadcast).IsNotNull()
                .And.IsEqualTo(responseBody.Broadcast, new BroadcastEqualityComparer());
        }

        public async Task Then_my_contest_should_now_contain_a_single_child_broadcast_referencing_the_created_broadcast()
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();
            Contest myRetrievedContest = await ApiDriver.GetSingleContestAsync(myContest.Id);

            CreateChildBroadcastResponse responseBody = await Assert.That(ResponseBody).IsNotNull();
            Broadcast createdBroadcast = responseBody.Broadcast;

            ChildBroadcast? singleChildBroadcast = await Assert.That(myRetrievedContest.ChildBroadcasts).HasSingleItem();

            await Assert.That(singleChildBroadcast).IsNotNull()
                .And.HasMember(broadcast => broadcast.BroadcastId).EqualTo(createdBroadcast.Id)
                .And.HasMember(broadcast => broadcast.ContestStage).EqualTo(createdBroadcast.ContestStage)
                .And.HasMember(broadcast => broadcast.Completed).EqualTo(false);
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_contest_stage(string contestStage)
        {
            ProblemDetails responseProblemDetails = await Assert.That(ResponseProblemDetails).IsNotNull();

            await Assert.That(responseProblemDetails).HasExtension("contestStage", contestStage);
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_broadcast_date(string broadcastDate)
        {
            ProblemDetails responseProblemDetails = await Assert.That(ResponseProblemDetails).IsNotNull();

            await Assert.That(responseProblemDetails).HasExtension("broadcastDate", broadcastDate);
        }

        public async Task Then_my_contest_should_be_unchanged()
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            Contest myRetrievedContest = await ApiDriver.GetSingleContestAsync(myContest.Id);

            await Assert.That(myRetrievedContest).IsEqualTo(myContest, new ContestEqualityComparer());
        }

        public async Task Then_my_given_broadcast_should_be_the_only_existing_broadcast_in_the_system()
        {
            Broadcast myGivenBroadcast = await Assert.That(MyExistingBroadcast).IsNotNull();

            Broadcast[] existingBroadcasts = await ApiDriver.GetAllBroadcastsAsync();

            Broadcast? singleExistingBroadcast = await Assert.That(existingBroadcasts).HasSingleItem();

            await Assert.That(singleExistingBroadcast).IsNotNull()
                .And.IsEqualTo(myGivenBroadcast, new BroadcastEqualityComparer());
        }

        public async Task Then_no_broadcasts_should_exist_in_the_system()
        {
            Broadcast[] existingBroadcasts = await ApiDriver.GetAllBroadcastsAsync();

            await Assert.That(existingBroadcasts).IsEmpty();
        }

        private Voter MapToVoter(Dictionary<string, string> row) => new()
        {
            VotingCountryId = MyCountryCodesAndIds[row["CountryCode"]], PointsAwarded = bool.Parse(row["PointsAwarded"])
        };

        private Competitor MapToCompetitor(Dictionary<string, string> row) => new()
        {
            CompetingCountryId = MyCountryCodesAndIds[row["CountryCode"]],
            RunningOrderPosition = int.Parse(row["RunningOrder"]),
            FinishingPosition = int.Parse(row["Finish"]),
            JuryAwards = [],
            TelevoteAwards = []
        };
    }
}

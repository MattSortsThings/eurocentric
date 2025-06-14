using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed class DeleteBroadcastTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_delete_broadcast_by_ID_scenario_1(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-17",
            competingCountryCodes: ["AT", "BE", "DK", "EE"]);
        admin.Given_I_want_to_delete_my_broadcast_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_no_broadcasts_should_exist();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_delete_broadcast_by_ID_scenario_2(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-17",
            competingCountryCodes: ["AT", "BE", "DK", "EE"]);
        await admin.Given_I_have_awarded_a_set_of_televote_points_in_my_broadcast(
            votingCountryCode: "XX",
            rankedCompetingCountryCodes: ["AT", "BE", "DK", "EE"]);
        admin.Given_I_want_to_delete_my_broadcast_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_204_NoContent();
        await admin.Then_no_broadcasts_should_exist();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_delete_non_existent_broadcast_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-17",
            competingCountryCodes: ["AT", "BE", "DK", "EE"]);
        await admin.Given_I_have_deleted_my_broadcast();
        admin.Given_I_want_to_delete_my_broadcast_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_404_NotFound();
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Broadcast not found",
            detail: "No broadcast exists with the provided broadcast ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_broadcast_ID_with_key("broadcastId");
    }

    private sealed class AdminActor : ActorWithoutResponse
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(7);

        private Contest? MyContest { get; set; }

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

        public async Task Given_I_have_created_a_child_broadcast_for_my_contest(string[]? competingCountryCodes,
            string broadcastDate = "",
            string contestStage = "")
        {
            Assert.NotNull(MyContest);

            MyBroadcast = await ApiDriver.Contests.CreateAChildBroadcastAsync(
                contestStage: Enum.Parse<ContestStage>(contestStage),
                broadcastDate: DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
                contestId: MyContest.Id,
                competingCountryIds: competingCountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray() ?? [],
                cancellationToken: TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_deleted_my_broadcast()
        {
            Assert.NotNull(MyBroadcast);

            await ApiDriver.Broadcasts.DeleteABroadcastAsync(MyBroadcast.Id, TestContext.Current.CancellationToken);
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

        public void Given_I_want_to_delete_my_broadcast_by_its_ID()
        {
            Assert.NotNull(MyBroadcast);

            Guid myBroadcastId = MyBroadcast.Id;

            SendMyRequest = apiDriver =>
                apiDriver.Broadcasts.DeleteBroadcast(myBroadcastId, TestContext.Current.CancellationToken);
        }

        public async Task Then_no_broadcasts_should_exist()
        {
            Broadcast[] existingBroadcasts =
                await ApiDriver.Broadcasts.GetAllBroadcastsAsync(TestContext.Current.CancellationToken);

            Assert.Empty(existingBroadcasts);
        }

        public void Then_the_problem_details_extensions_should_contain_my_broadcast_ID_with_key(string key)
        {
            Assert.NotNull(MyBroadcast);
            Assert.NotNull(ResponseProblemDetails);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp.Key == key
                                                                      && kvp.Value is JsonElement e
                                                                      && e.GetGuid() == MyBroadcast.Id);
        }
    }
}

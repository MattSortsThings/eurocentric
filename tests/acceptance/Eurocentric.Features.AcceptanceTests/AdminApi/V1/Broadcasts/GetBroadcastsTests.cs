using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastsTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_all_existing_broadcasts_in_broadcast_date_order(string apiVersion)
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
            competingCountryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2025-05-13",
            competingCountryCodes: ["AT", "BE"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2025-05-15",
            competingCountryCodes: ["DK", "EE"]);
        admin.Given_I_want_to_retrieve_all_existing_broadcasts();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_200_OK();
        admin.Then_the_retrieved_broadcasts_should_be_my_broadcasts_in_broadcast_date_order();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_empty_list_when_no_broadcasts_exist(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_broadcasts();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_200_OK();
        admin.Then_the_retrieved_broadcasts_should_be_an_empty_list();
    }

    private sealed class AdminActor : ActorWithResponse<GetBroadcastsResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(7);

        private Contest? MyContest { get; set; }

        private List<Broadcast> MyBroadcasts { get; } = new(3);

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

            Broadcast myBroadcast = await ApiDriver.Contests.CreateAChildBroadcastAsync(
                contestStage: Enum.Parse<ContestStage>(contestStage),
                broadcastDate: DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd"),
                contestId: MyContest.Id,
                competingCountryIds: competingCountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray() ?? [],
                cancellationToken: TestContext.Current.CancellationToken);

            MyBroadcasts.Add(myBroadcast);
        }

        public void Given_I_want_to_retrieve_all_existing_broadcasts() =>
            SendMyRequest = apiDriver => apiDriver.Broadcasts.GetBroadcasts(TestContext.Current.CancellationToken);

        public void Then_the_retrieved_broadcasts_should_be_my_broadcasts_in_broadcast_date_order()
        {
            Assert.NotNull(ResponseObject);

            IOrderedEnumerable<Broadcast> expectedBroadcasts = MyBroadcasts.OrderBy(broadcast => broadcast.BroadcastDate);

            Assert.Equal(expectedBroadcasts, ResponseObject.Broadcasts, new BroadcastEqualityComparer());
        }

        public void Then_the_retrieved_broadcasts_should_be_an_empty_list()
        {
            Assert.NotNull(ResponseObject);

            Assert.Empty(ResponseObject.Broadcasts);
        }
    }
}

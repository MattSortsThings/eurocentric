using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AdminApi.V1.Broadcasts.GetBroadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastsTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_all_existing_broadcasts_in_broadcast_date_order(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(
            contestYear: 2016,
            cityName: "Stockholm",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal2",
            broadcastDate: "2016-05-02",
            competingCountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2016-05-03",
            competingCountryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
            contestStage: "SemiFinal1",
            broadcastDate: "2016-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);

        admin.Given_I_want_to_retrieve_all_existing_broadcasts();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_broadcasts_should_be_my_broadcasts_in_broadcast_date_order();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_empty_list_when_no_broadcasts_exist(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_broadcasts();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_broadcasts_should_be_an_empty_list();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<GetBroadcastsResponse>(apiDriver)
    {
        private CountryIdLookup CountryIds { get; } = new();

        private Guid? ContestId { get; set; }

        private List<Broadcast> Broadcasts { get; } = [];

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            List<Country> createdCountries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);
            CountryIds.Populate(createdCountries);
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string cityName = "",
            int contestYear = 0)
        {
            Guid[] group1CountryIds = group1CountryCodes.Select(CountryIds.GetSingle).ToArray();
            Guid[] group2CountryIds = group2CountryCodes.Select(CountryIds.GetSingle).ToArray();

            Contest myContest = await ApiDriver.CreateSingleStockholmFormatContestAsync(contestYear: contestYear,
                cityName: cityName,
                group1CountryIds: group1CountryIds,
                group2CountryIds: group2CountryIds);

            ContestId = myContest.Id;
        }

        public async Task Given_I_have_created_a_child_broadcast_for_my_contest(
            string[] competingCountryCodes = null!,
            string broadcastDate = "",
            string contestStage = "")
        {
            Guid myContestId = await Assert.That(ContestId).IsNotNull();
            DateOnly date = DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat);
            Guid[] competingCountryIds = competingCountryCodes.Select(CountryIds.GetSingle).ToArray();
            ContestStage stage = Enum.Parse<ContestStage>(contestStage);

            Broadcast broadcast = await ApiDriver.CreateSingleBroadcastAsync(contestStage: stage,
                broadcastDate: date,
                competingCountryIds: competingCountryIds,
                contestId: myContestId);

            Broadcasts.Add(broadcast);
        }

        public void Given_I_want_to_retrieve_all_existing_broadcasts() =>
            Request = ApiDriver.RequestFactory.Broadcasts.GetBroadcasts();

        public async Task Then_the_retrieved_broadcasts_should_be_my_broadcasts_in_broadcast_date_order()
        {
            GetBroadcastsResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            IOrderedEnumerable<Broadcast> expectedBroadcasts = Broadcasts.OrderBy(broadcast => broadcast.BroadcastDate);

            await Assert.That(responseBody.Broadcasts)
                .IsEquivalentTo(expectedBroadcasts, new BroadcastEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_broadcasts_should_be_an_empty_list()
        {
            GetBroadcastsResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.Broadcasts).IsEmpty();
        }
    }
}

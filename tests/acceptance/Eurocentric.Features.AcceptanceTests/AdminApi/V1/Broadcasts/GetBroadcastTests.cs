using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Eurocentric.Features.AdminApi.V1.Broadcasts.GetBroadcast;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_requested_broadcast(string apiVersion)
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
            contestStage: "GrandFinal",
            broadcastDate: "2016-05-01",
            competingCountryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI"]);

        await admin.Given_I_want_to_retrieve_my_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_broadcast_should_be_my_broadcast();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_non_existent_broadcast_requested(string apiVersion)
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
            contestStage: "GrandFinal",
            broadcastDate: "2016-05-01",
            competingCountryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI"]);
        await admin.Given_I_have_deleted_my_broadcast();

        await admin.Given_I_want_to_retrieve_my_deleted_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_404_NotFound();
        await admin.Then_the_response_problem_details_should_match(status: 404,
            title: "Broadcast not found",
            detail: "No broadcast exists with the provided broadcast ID.");
        await admin.Then_the_response_problem_details_extensions_should_include_my_deleted_broadcast_ID();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<GetBroadcastResponse>(apiDriver)
    {
        private CountryIdLookup CountryIds { get; } = new();

        private Guid? ContestId { get; set; }

        private Broadcast? Broadcast { get; set; }

        private Guid? DeletedBroadcastId { get; set; }

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
            Guid[] group1CountryIds = CountryIds.GetMultiple(group1CountryCodes);
            Guid[] group2CountryIds = CountryIds.GetMultiple(group2CountryCodes);

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
            Guid[] competingCountryIds = CountryIds.GetMultiple(competingCountryCodes);
            ContestStage stage = Enum.Parse<ContestStage>(contestStage);

            Broadcast = await ApiDriver.CreateSingleBroadcastAsync(contestStage: stage,
                broadcastDate: date,
                competingCountryIds: competingCountryIds,
                contestId: myContestId);
        }

        public async Task Given_I_have_deleted_my_broadcast()
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();
            Guid myBroadcastId = myBroadcast.Id;

            await ApiDriver.DeleteSingleBroadcastAsync(myBroadcastId);

            Broadcast = null;
            DeletedBroadcastId = myBroadcastId;
        }

        public async Task Given_I_want_to_retrieve_my_broadcast()
        {
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();

            Request = ApiDriver.RequestFactory.Broadcasts.GetBroadcast(myBroadcast.Id);
        }

        public async Task Given_I_want_to_retrieve_my_deleted_broadcast()
        {
            Guid myDeletedBroadcastId = await Assert.That(DeletedBroadcastId).IsNotNull();

            Request = ApiDriver.RequestFactory.Broadcasts.GetBroadcast(myDeletedBroadcastId);
        }

        public async Task Then_the_retrieved_broadcast_should_be_my_broadcast()
        {
            GetBroadcastResponse responseBody = await Assert.That(ResponseBody).IsNotNull();
            Broadcast myBroadcast = await Assert.That(Broadcast).IsNotNull();

            await Assert.That(responseBody.Broadcast).IsEqualTo(myBroadcast, new BroadcastEqualityComparer());
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_deleted_broadcast_ID()
        {
            ProblemDetails problemDetails = await Assert.That(ResponseProblemDetails).IsNotNull();
            Guid myDeletedBroadcastId = await Assert.That(DeletedBroadcastId).IsNotNull();

            await Assert.That(problemDetails).HasExtension("broadcastId", myDeletedBroadcastId);
        }
    }
}

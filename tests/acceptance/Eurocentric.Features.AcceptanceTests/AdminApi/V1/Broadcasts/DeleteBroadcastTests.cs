using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed class DeleteBroadcastTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_delete_requested_broadcast(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(
            contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_GrandFinal_child_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI"]);

        await admin.Given_I_want_to_delete_my_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_no_broadcasts_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_non_existent_broadcast_requested(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(
            contestYear: 2025,
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_GrandFinal_child_broadcast_for_my_contest(
            broadcastDate: "2025-05-01",
            competingCountryCodes: ["AT", "BE", "CZ", "DK", "EE", "FI"]);
        await admin.Given_I_have_deleted_my_broadcast();

        await admin.Given_I_want_to_delete_my_deleted_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_404_NotFound();
        await admin.Then_the_response_problem_details_should_match(status: 404,
            title: "Broadcast not found",
            detail: "No broadcast exists with the provided broadcast ID.");
        await admin.Then_the_response_problem_details_extensions_should_include_my_deleted_broadcast_ID();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithoutResponse(apiDriver)
    {
        private Dictionary<string, Guid> CountryCodesAndIds { get; } = new(6);

        private Guid? ContestId { get; set; }

        private Broadcast? MyBroadcast { get; set; }

        private Guid? DeletedBroadcastId { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            List<Country> createdCountries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);

            foreach (Country country in createdCountries)
            {
                CountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            int contestYear = 0)
        {
            Guid[] group1CountryIds = group1CountryCodes.Select(c => CountryCodesAndIds[c]).ToArray();
            Guid[] group2CountryIds = group2CountryCodes.Select(c => CountryCodesAndIds[c]).ToArray();

            Contest myContest = await ApiDriver.CreateSingleStockholmFormatContestAsync(contestYear: contestYear,
                cityName: TestDefaults.CityName,
                group1CountryIds: group1CountryIds,
                group2CountryIds: group2CountryIds);

            ContestId = myContest.Id;
        }

        public async Task Given_I_have_created_a_GrandFinal_child_broadcast_for_my_contest(
            string[] competingCountryCodes = null!,
            string broadcastDate = "")
        {
            Guid myContestId = await Assert.That(ContestId).IsNotNull();
            DateOnly date = DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat);
            Guid[] competingCountryIds = competingCountryCodes.Select(c => CountryCodesAndIds[c]).ToArray();

            MyBroadcast = await ApiDriver.CreateSingleBroadcastAsync(contestStage: ContestStage.GrandFinal,
                broadcastDate: date,
                competingCountryIds: competingCountryIds,
                contestId: myContestId);
        }

        public async Task Given_I_have_deleted_my_broadcast()
        {
            Broadcast myBroadcast = await Assert.That(MyBroadcast).IsNotNull();
            Guid myBroadcastId = myBroadcast.Id;

            await ApiDriver.DeleteSingleBroadcastAsync(myBroadcastId);

            MyBroadcast = null;
            DeletedBroadcastId = myBroadcastId;
        }

        public async Task Given_I_want_to_delete_my_broadcast()
        {
            Broadcast myBroadcast = await Assert.That(MyBroadcast).IsNotNull();

            Request = ApiDriver.RequestFactory.Broadcasts.DeleteBroadcast(myBroadcast.Id);
        }

        public async Task Given_I_want_to_delete_my_deleted_broadcast()
        {
            Guid myDeletedBroadcastId = await Assert.That(DeletedBroadcastId).IsNotNull();

            Request = ApiDriver.RequestFactory.Broadcasts.DeleteBroadcast(myDeletedBroadcastId);
        }

        public async Task Then_no_broadcasts_should_exist_in_the_system()
        {
            Broadcast[] existingBroadcasts = await ApiDriver.GetAllBroadcastsAsync();

            await Assert.That(existingBroadcasts).IsEmpty();
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_deleted_broadcast_ID()
        {
            ProblemDetails problemDetails = await Assert.That(ResponseProblemDetails).IsNotNull();
            Guid myDeletedBroadcastId = await Assert.That(DeletedBroadcastId).IsNotNull();

            await Assert.That(problemDetails).HasExtension("broadcastId", myDeletedBroadcastId);
        }
    }
}

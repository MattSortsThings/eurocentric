using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Broadcasts;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class DeleteContestTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_delete_requested_contest(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2022,
            cityName: "Turin",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_want_to_delete_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_204_NoContent();
        await admin.Then_no_contests_should_exist_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_contest_with_any_child_broadcasts(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2022,
            cityName: "Turin",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(broadcastDate: "2022-05-01",
            competingCountryCodes: ["AT", "BE", "CZ"]);

        await admin.Given_I_want_to_delete_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_409_Conflict();
        await admin.Then_the_response_problem_details_should_match(status: 409,
            title: "Contest deletion blocked",
            detail: "The contest cannot be deleted because it has one or more child broadcasts.");
        await admin.Then_my_contest_should_be_the_only_existing_contest_in_the_system();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_non_existent_contest_requested(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2022,
            cityName: "Turin",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_deleted_my_contest();

        await admin.Given_I_want_to_delete_my_deleted_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_404_NotFound();
        await admin.Then_the_response_problem_details_should_match(status: 404,
            title: "Contest not found",
            detail: "No contest exists with the provided contest ID.");
        await admin.Then_the_response_problem_details_extensions_should_include_my_deleted_contest_ID();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithoutResponse(apiDriver)
    {
        private CountryIdLookup CountryIds { get; } = new();

        private Contest? Contest { get; set; }

        private Broadcast? Broadcast { get; set; }

        private Guid? DeletedContestId { get; set; }

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

            Contest = await ApiDriver.CreateSingleStockholmFormatContestAsync(contestYear: contestYear,
                cityName: cityName,
                group1CountryIds: group1CountryIds,
                group2CountryIds: group2CountryIds);
        }

        public async Task Given_I_have_created_the_SemiFinal1_child_broadcast_for_my_contest(
            string[] competingCountryCodes = null!,
            string broadcastDate = "")
        {
            Contest myContest = await Assert.That(Contest).IsNotNull();

            Broadcast = await ApiDriver.CreateSingleBroadcastAsync(contestId: myContest.Id,
                broadcastDate: DateOnly.ParseExact(broadcastDate, TestDefaults.DateFormat),
                contestStage: ContestStage.SemiFinal1,
                competingCountryIds: CountryIds.GetMultiple(competingCountryCodes));

            Contest = await ApiDriver.GetSingleContestAsync(myContest.Id);
        }

        public async Task Given_I_have_deleted_my_contest()
        {
            Contest myContest = await Assert.That(Contest).IsNotNull();
            Guid myContestId = myContest.Id;

            await ApiDriver.DeleteSingleContestAsync(myContestId);

            Contest = null;
            DeletedContestId = myContestId;
        }

        public async Task Given_I_want_to_delete_my_contest()
        {
            Contest myContest = await Assert.That(Contest).IsNotNull();

            Request = ApiDriver.RequestFactory.Contests.DeleteContest(myContest.Id);
        }

        public async Task Given_I_want_to_delete_my_deleted_contest()
        {
            Guid myDeletedContestId = await Assert.That(DeletedContestId).IsNotNull();

            Request = ApiDriver.RequestFactory.Contests.DeleteContest(myDeletedContestId);
        }

        public async Task Then_no_contests_should_exist_in_the_system()
        {
            Contest[] existingContests = await ApiDriver.GetAllContestsAsync();

            await Assert.That(existingContests).IsEmpty();
        }

        public async Task Then_my_contest_should_be_the_only_existing_contest_in_the_system()
        {
            Contest myContest = await Assert.That(Contest).IsNotNull();

            Contest[] existingContests = await ApiDriver.GetAllContestsAsync();

            Contest? singleExistingContest = await Assert.That(existingContests).HasSingleItem();

            await Assert.That(singleExistingContest).IsNotNull()
                .And.IsEqualTo(myContest, new ContestEqualityComparer());
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_deleted_contest_ID()
        {
            ProblemDetails problemDetails = await Assert.That(ResponseProblemDetails).IsNotNull();
            Guid myDeletedContestId = await Assert.That(DeletedContestId).IsNotNull();

            await Assert.That(problemDetails).HasExtension("contestId", myDeletedContestId);
        }
    }
}

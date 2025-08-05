using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Contests.GetContest;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestTests : SerialCleanAcceptanceTest
{
    [Test]
    [Arguments("v1.0")]
    public async Task Endpoint_should_retrieve_requested_contest_scenario_1(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(contestYear: 2025,
            cityName: "Basel",
            group0CountryCode: "XX",
            group1CountryCodes: ["DK", "EE", "FI"],
            group2CountryCodes: ["AT", "BE", "CZ"]);

        await admin.Given_I_want_to_retrieve_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_contest_should_be_my_contest();
    }

    [Test]
    [Arguments("v1.0")]
    public async Task Endpoint_should_retrieve_requested_contest_scenario_2(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(contestYear: 2022,
            cityName: "Turin",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);

        await admin.Given_I_want_to_retrieve_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_contest_should_be_my_contest();
    }

    [Test]
    [Arguments("v1.0")]
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

        await admin.Given_I_want_to_retrieve_my_deleted_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_404_NotFound();
        await admin.Then_the_response_problem_details_should_match(status: 404,
            title: "Contest not found",
            detail: "No contest exists with the provided contest ID.");
        await admin.Then_the_response_problem_details_extensions_should_include_my_deleted_contest_ID();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<GetContestResponse>(apiDriver)
    {
        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(7);

        private Contest? MyContest { get; set; }

        private Guid? MyDeletedContestId { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            List<Country> createdCountries = await ApiDriver.CreateMultipleCountriesAsync(countryCodes);

            foreach (Country country in createdCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string cityName = "",
            int contestYear = 0)
        {
            Guid[] group1CountryIds = group1CountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();
            Guid[] group2CountryIds = group2CountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();

            MyContest = await ApiDriver.CreateSingleStockholmFormatContestAsync(contestYear: contestYear,
                cityName: cityName,
                group1CountryIds: group1CountryIds,
                group2CountryIds: group2CountryIds);
        }

        public async Task Given_I_have_created_a_Liverpool_format_contest_for_my_countries(string[] group2CountryCodes = null!,
            string[] group1CountryCodes = null!,
            string group0CountryCode = "",
            string cityName = "",
            int contestYear = 0)
        {
            Guid[] group1CountryIds = group1CountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();
            Guid[] group2CountryIds = group2CountryCodes.Select(c => MyCountryCodesAndIds[c]).ToArray();

            MyContest = await ApiDriver.CreateSingleLiverpoolFormatContestAsync(contestYear: contestYear,
                cityName: cityName,
                group0CountryId: MyCountryCodesAndIds[group0CountryCode],
                group1CountryIds: group1CountryIds,
                group2CountryIds: group2CountryIds);
        }

        public async Task Given_I_have_deleted_my_contest()
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();
            Guid myContestId = myContest.Id;

            await ApiDriver.DeleteSingleContestAsync(myContestId);

            MyContest = null;
            MyDeletedContestId = myContestId;
        }

        public async Task Given_I_want_to_retrieve_my_contest()
        {
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            Request = ApiDriver.RequestFactory.Contests.GetContest(myContest.Id);
        }

        public async Task Given_I_want_to_retrieve_my_deleted_contest()
        {
            Guid myDeletedContestId = await Assert.That(MyDeletedContestId).IsNotNull();

            Request = ApiDriver.RequestFactory.Contests.GetContest(myDeletedContestId);
        }

        public async Task Then_the_retrieved_contest_should_be_my_contest()
        {
            GetContestResponse responseBody = await Assert.That(ResponseBody).IsNotNull();
            Contest myContest = await Assert.That(MyContest).IsNotNull();

            await Assert.That(responseBody.Contest).IsEqualTo(myContest, new ContestEqualityComparer());
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_deleted_contest_ID()
        {
            ProblemDetails problemDetails = await Assert.That(ResponseProblemDetails).IsNotNull();
            Guid myDeletedContestId = await Assert.That(MyDeletedContestId).IsNotNull();

            await Assert.That(problemDetails).HasExtension("contestId", myDeletedContestId);
        }
    }
}

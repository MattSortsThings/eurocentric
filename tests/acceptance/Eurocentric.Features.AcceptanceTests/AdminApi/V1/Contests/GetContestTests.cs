using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.DataSourceGenerators;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.Helpers.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.Helpers.Countries;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils.Assertions;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Contests.GetContest;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestTests : SerialCleanAcceptanceTest
{
    [Test]
    [AdminApiV1Point0AndUp]
    public async Task Endpoint_should_retrieve_requested_contest_scenario_1(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest_for_my_countries(
            contestYear: 2024,
            cityName: "Malmö",
            group1Countries: ["AT", "BE", "CZ"],
            group2Countries: ["DK", "EE", "FI"],
            globalTelevoteCountry: "XX");

        await admin.Given_I_want_to_retrieve_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_contest_should_be_my_contest();
    }

    [Test]
    [AdminApiV1Point0AndUp]
    public async Task Endpoint_should_retrieve_requested_contest_scenario_2(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(
            contestYear: 2018,
            cityName: "Lisbon",
            group1Countries: ["BE", "DK", "FI"],
            group2Countries: ["AT", "CZ", "EE"]);

        await admin.Given_I_want_to_retrieve_my_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_contest_should_be_my_contest();
    }

    [Test]
    [AdminApiV1Point0AndUp]
    public async Task Endpoint_should_fail_on_non_existent_contest_requested(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Stockholm_format_contest_for_my_countries(
            contestYear: 2018,
            cityName: "Lisbon",
            group1Countries: ["BE", "DK", "FI"],
            group2Countries: ["AT", "CZ", "EE"]);
        await admin.Given_I_have_deleted_my_contest();

        await admin.Given_I_want_to_retrieve_my_deleted_contest();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_404_NotFound();
        await admin.Then_the_response_problem_details_should_match(
            status: 404,
            title: "Contest not found",
            detail: "No contest exists with the provided contest ID.");
        await admin.Then_the_response_problem_details_extensions_should_include_my_deleted_contest_ID();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<GetContestResponse>(apiDriver)
    {
        private CountryIdLookup CountryIds { get; } = new(7);

        private Contest? Contest { get; set; }

        private Guid? DeletedContestId { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            await foreach (Country createdCountry in ApiDriver.CreateMultipleCountriesAsync(countryCodes))
            {
                CountryIds.Add(createdCountry);
            }
        }

        public async Task Given_I_have_created_a_Liverpool_format_contest_for_my_countries(
            string globalTelevoteCountry = "",
            string[] group2Countries = null!,
            string[] group1Countries = null!,
            string cityName = "",
            int contestYear = 0)
        {
            Contest createdContest = await ApiDriver.CreateSingleLiverpoolFormatContestAsync(contestYear: contestYear,
                cityName: cityName,
                group1CountryIds: group1Countries.Select(CountryIds.GetSingle).ToArray(),
                group2CountryIds: group2Countries.Select(CountryIds.GetSingle).ToArray(),
                globalTelevoteCountryId: CountryIds.GetSingle(globalTelevoteCountry));

            Contest = createdContest;
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest_for_my_countries(
            string[] group2Countries = null!,
            string[] group1Countries = null!,
            string cityName = "",
            int contestYear = 0)
        {
            Contest createdContest = await ApiDriver.CreateSingleStockholmFormatContestAsync(contestYear: contestYear,
                cityName: cityName,
                group1CountryIds: group1Countries.Select(CountryIds.GetSingle).ToArray(),
                group2CountryIds: group2Countries.Select(CountryIds.GetSingle).ToArray());

            Contest = createdContest;
        }

        public async Task Given_I_have_deleted_my_contest()
        {
            Contest contest = await Assert.That(Contest).IsNotNull();

            await ApiDriver.DeleteSingleContestAsync(contest.Id);

            DeletedContestId = contest.Id;
            Contest = null;
        }

        public async Task Given_I_want_to_retrieve_my_contest()
        {
            Contest contest = await Assert.That(Contest).IsNotNull();

            Request = ApiDriver.RequestFactory.Contests.GetContest(contest.Id);
        }

        public async Task Given_I_want_to_retrieve_my_deleted_contest()
        {
            Guid deletedContestId = await Assert.That(DeletedContestId).IsNotNull();

            Request = ApiDriver.RequestFactory.Contests.GetContest(deletedContestId);
        }

        public async Task Then_the_retrieved_contest_should_be_my_contest()
        {
            GetContestResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            Contest expectedContest = await Assert.That(Contest).IsNotNull();

            Contest actualContest = responseBody.Contest;

            await Assert.That(expectedContest).IsEqualTo(actualContest, new ContestEqualityComparer());
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_deleted_contest_ID()
        {
            ProblemDetails problemDetails = await Assert.That(ResponseProblemDetails).IsNotNull();

            Guid expectedContestId = await Assert.That(DeletedContestId).IsNotNull();

            await Assert.That(problemDetails).HasExtension("contestId", expectedContestId);
        }
    }
}

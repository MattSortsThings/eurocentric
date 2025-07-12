using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public static class GetContestTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_retrieve_requested_contest(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(
                contestYear: 2016,
                cityName: "Stockholm",
                group1Countries: ["AT", "BE", "CZ"],
                group2Countries: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_retrieve_my_contest_by_its_ID();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_contest_should_be_my_contest();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_non_existent_contest_requested(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI");
            await admin.Given_I_have_created_a_Stockholm_format_contest(
                contestYear: 2016,
                cityName: "Stockholm",
                group1Countries: ["AT", "BE", "CZ"],
                group2Countries: ["DK", "EE", "FI"]);
            await admin.Given_I_have_deleted_every_contest_I_have_created();
            admin.Given_I_want_to_retrieve_my_contest_by_its_ID();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(404);
            admin.Then_the_response_problem_details_should_match(status: 404,
                title: "Contest not found",
                detail: "No contest exists with the provided contest ID.");
            admin.Then_the_response_problem_details_extensions_should_contain_my_contest_ID();
        }
    }

    private sealed class Admin : AdminActor<GetContestResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_my_contest_by_its_ID()
        {
            Contest myContest = Assert.Single(GivenContests);

            Request = RequestFactory.Contests.GetContest(myContest.Id);
        }

        public void Then_the_retrieved_contest_should_be_my_contest()
        {
            Assert.NotNull(ResponseObject);

            Contest myContest = Assert.Single(GivenContests);
            Contest retrievedContest = ResponseObject.Contest;

            Assert.Equal(myContest, retrievedContest, ContestEquality.Compare);
        }

        public void Then_the_response_problem_details_extensions_should_contain_my_contest_ID()
        {
            Assert.NotNull(ResponseProblemDetails);
            Contest myContest = Assert.Single(GivenContests);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "contestId", Value: JsonElement je }
                                                                      && je.GetGuid() == myContest.Id);
        }
    }
}

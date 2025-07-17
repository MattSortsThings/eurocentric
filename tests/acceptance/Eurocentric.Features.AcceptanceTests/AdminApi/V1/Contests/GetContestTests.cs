using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Contests;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Countries;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Mixins.Responses;
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
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            await admin.Given_I_have_created_a_Liverpool_format_contest(contestYear: 2025,
                cityName: "Basel",
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            admin.Given_I_want_to_retrieve_my_contest();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_contest_should_be_my_contest();
        }

        [Theory]
        [InlineData("v1.0")]
        public async Task Should_fail_on_non_existent_country_requested(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, RequestFactory.WithApiVersion(apiVersion));

            // Given
            await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
            await admin.Given_I_have_created_a_Liverpool_format_contest(contestYear: 2025,
                cityName: "Basel",
                group0CountryCode: "XX",
                group1CountryCodes: ["AT", "BE", "CZ"],
                group2CountryCodes: ["DK", "EE", "FI"]);
            await admin.Given_I_have_deleted_my_contest();
            admin.Given_I_want_to_retrieve_my_contest();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_fail_with_status_code(404);
            admin.Then_the_response_problem_details_should_match(status: 404,
                title: "Contest not found",
                detail: "No contest exists with the provided contest ID.");
            admin.Then_the_response_problem_details_should_have_a_contestId_extension_with_my_contest_ID();
        }
    }

    private sealed class Admin : AdminActorWithResponse<GetContestResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, IRequestFactory requestFactory) :
            base(restClient, backDoor, requestFactory)
        {
        }

        public void Given_I_want_to_retrieve_my_contest()
        {
            Contest myContest = GivenContests.GetSingle();

            Request = RequestFactory.Contests.GetContest(myContest.Id);
        }

        public void Then_the_retrieved_contest_should_be_my_contest()
        {
            Assert.NotNull(ResponseObject);

            Contest expectedContest = GivenContests.GetSingle();
            Contest retrievedContest = ResponseObject.Contest;

            Assert.Equal(expectedContest, retrievedContest, new ContestEqualityComparer());
        }

        public void Then_the_response_problem_details_should_have_a_contestId_extension_with_my_contest_ID()
        {
            Assert.NotNull(ResponseProblemDetails);

            Guid expectedContestId = GivenContests.GetSingle().Id;

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp is { Key: "contestId", Value: JsonElement je }
                                                                      && je.GetGuid() == expectedContestId);
        }
    }
}

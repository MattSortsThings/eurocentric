using Eurocentric.Features.AcceptanceTests.AdminApi.Contests.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;
using Eurocentric.Features.AdminApi.V0.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.Contests;

public static class GetContestsTests
{
    public sealed class Feature(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v0.2")]
        public async Task Should_retrieve_all_contests_in_contest_year_order(string apiVersion)
        {
            Admin admin = new(BackDoor, RestClient, apiVersion);

            // Given
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2022, cityName: "Turin");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2016, cityName: "Stockholm");
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2017, cityName: "Kyiv");
            admin.Given_I_want_to_retrieve_all_contests();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order();
        }

        [Theory]
        [InlineData("v0.2")]
        public async Task Should_retrieve_empty_list_when_no_contests_exist(string apiVersion)
        {
            Admin admin = new(BackDoor, RestClient, apiVersion);

            // Given
            admin.Given_I_want_to_retrieve_all_contests();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_contests_should_be_an_empty_list();
        }
    }

    private sealed class Admin : ActorWithResponse<GetContestsResponse>
    {
        public Admin(IWebAppFixtureBackDoor backDoor, IWebAppFixtureRestClient restClient, string apiVersion = "v1.0") :
            base(backDoor, restClient, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_all_contests()
        {
            Request = new RestRequest("/admin/api/{apiVersion}/contests");
            Request.AddUrlSegment("apiVersion", ApiVersion);
        }

        public void Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order()
        {
            Assert.NotNull(ResponseObject);

            IOrderedEnumerable<Contest> expectedContests = GivenContests.OrderBy(contest => contest.ContestYear);
            Contest[] actualContests = ResponseObject.Contests;

            Assert.Equal(expectedContests, actualContests, ContestEquality.Compare);
        }

        public void Then_the_retrieved_contests_should_be_an_empty_list()
        {
            Assert.NotNull(ResponseObject);

            Assert.Empty(ResponseObject.Contests);
        }
    }
}

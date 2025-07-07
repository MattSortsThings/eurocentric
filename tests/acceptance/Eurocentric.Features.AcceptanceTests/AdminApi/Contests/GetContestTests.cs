using Eurocentric.Features.AcceptanceTests.AdminApi.Contests.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;
using Eurocentric.Features.AdminApi.V0.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.Contests;

public static class GetContestTests
{
    public sealed class Feature(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v0.1")]
        [InlineData("v0.2")]
        public async Task Should_retrieve_specified_contest(string apiVersion)
        {
            Admin admin = new(BackDoor, RestClient, apiVersion);

            // Given
            await admin.Given_I_have_created_a_Stockholm_format_contest(contestYear: 2022, cityName: "Turin");
            admin.Given_I_want_to_retrieve_my_contest_by_its_ID();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_contest_should_be_my_contest();
        }
    }

    private sealed class Admin : ActorWithResponse<GetContestResponse>
    {
        public Admin(IWebAppFixtureBackDoor backDoor, IWebAppFixtureRestClient restClient, string apiVersion = "v1.0") :
            base(backDoor, restClient, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_my_contest_by_its_ID()
        {
            Contest myContest = Assert.Single(GivenContests);

            Request = new RestRequest("/admin/api/{apiVersion}/contests/{contestId}");

            Request.AddUrlSegment("apiVersion", ApiVersion)
                .AddUrlSegment("contestId", myContest.Id);
        }

        public void Then_the_retrieved_contest_should_be_my_contest()
        {
            Assert.NotNull(ResponseObject);
            Contest myContest = Assert.Single(GivenContests);

            Assert.Equal(myContest, ResponseObject.Contest, ContestEquality.Compare);
        }
    }
}

using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Mixins.Contests;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;
using Eurocentric.Features.AdminApi.V0.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Contests;

public static class GetContestTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v0.1")]
        [InlineData("v0.2")]
        public async Task Should_retrieve_requested_contest(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            await admin.Given_I_have_created_a_Liverpool_format_contest(contestYear: 2025, cityName: "Basel");
            admin.Given_I_want_to_retrieve_my_contest_by_its_ID();

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_contest_should_be_my_contest();
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

            Assert.Equal(myContest, ResponseObject.Contest, ContestEquality.Compare);
        }
    }
}

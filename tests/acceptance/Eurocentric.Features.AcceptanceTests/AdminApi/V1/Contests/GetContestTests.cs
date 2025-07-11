using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public static class GetContestTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v1.0")]
        public async Task Should_retrieve_dummy_contest(string apiVersion)
        {
            Admin admin = new(RestClient, BackDoor, apiVersion);

            // Given
            admin.Given_I_want_to_retrieve_the_contest_with_the_ID("3e2369e4-c120-4bcc-a1ca-270ec768a4a1");

            // When
            await admin.When_I_send_my_request();

            // Then
            admin.Then_my_request_should_succeed_with_status_code(200);
            admin.Then_the_retrieved_contest_should_have_the_ID("3e2369e4-c120-4bcc-a1ca-270ec768a4a1");
        }
    }

    private sealed class Admin : AdminActor<GetContestResponse>
    {
        public Admin(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_the_contest_with_the_ID(string contestId)
        {
            Guid targetContestId = Guid.Parse(contestId);

            Request = RequestFactory.Contests.GetContest(targetContestId);
        }

        public void Then_the_retrieved_contest_should_have_the_ID(string contestId)
        {
            Assert.NotNull(ResponseObject);

            Guid expectedContestId = Guid.Parse(contestId);

            Assert.Equal(expectedContestId, ResponseObject.Contest.Id);
        }
    }
}

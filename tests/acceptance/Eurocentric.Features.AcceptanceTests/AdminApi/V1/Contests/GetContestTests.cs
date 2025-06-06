using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_dummy_contest_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_contest_with_the_ID("ffe68f17-0c99-464c-ad5e-0c2709a9f2ec");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_contest_should_have_the_ID("ffe68f17-0c99-464c-ad5e-0c2709a9f2ec");
    }

    private sealed class AdminActor : ActorWithResponse<GetContestResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        public void Given_I_want_to_retrieve_the_contest_with_the_ID(string contestId)
        {
            Guid targetId = Guid.Parse(contestId);

            SendMyRequest = apiDriver => apiDriver.Contests.GetContest(targetId, TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_contest_should_have_the_ID(string contestId)
        {
            Assert.NotNull(ResponseObject);

            Guid expectedId = Guid.Parse(contestId);

            Assert.Equal(expectedId, ResponseObject.Contest.Id);
        }
    }
}

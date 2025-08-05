using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AdminApi.V1.Contests.GetContest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestTests : SerialCleanAcceptanceTest
{
    [Test]
    [Arguments("v1.0")]
    public async Task Endpoint_should_return_dummy_contest(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_contest_with_ID("f27b02e5-346c-4fbf-a5bd-667c51820aa1");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_contest_ID_should_be("f27b02e5-346c-4fbf-a5bd-667c51820aa1");
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<GetContestResponse>(apiDriver)
    {
        public void Given_I_want_to_retrieve_the_contest_with_ID(string contestId) =>
            Request = ApiDriver.RequestFactory.Contests.GetContest(Guid.Parse(contestId));

        public async Task Then_the_retrieved_contest_ID_should_be(string contestId)
        {
            GetContestResponse responseBody = await Assert.That(ResponseBody).IsNotNull();
            Guid expectedContestId = Guid.Parse(contestId);

            await Assert.That(responseBody.Contest)
                .HasMember(contest => contest.Id).EqualTo(expectedContestId);
        }
    }
}

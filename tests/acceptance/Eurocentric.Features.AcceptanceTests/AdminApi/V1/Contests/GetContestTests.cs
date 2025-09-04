using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.DataSourceGenerators;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Contests.GetContest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestTests : SerialCleanAcceptanceTest
{
    [Test]
    [AdminApiV1Point0AndUp]
    public async Task Endpoint_should_retrieve_dummy_contest(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_contest_with_ID("7e0b3a7b-9fcd-40ad-8662-37f2b1f022f5");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_contest_should_have_the_ID("7e0b3a7b-9fcd-40ad-8662-37f2b1f022f5");
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<GetContestResponse>(apiDriver)
    {
        public void Given_I_want_to_retrieve_the_contest_with_ID(string contestId)
        {
            Guid targetId = Guid.Parse(contestId);

            Request = ApiDriver.RequestFactory.Contests.GetContest(targetId);
        }

        public async Task Then_the_retrieved_contest_should_have_the_ID(string contestId)
        {
            GetContestResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            Guid expectedContestId = Guid.Parse(contestId);

            Guid actualContestId = responseBody.Contest.Id;

            await Assert.That(actualContestId).IsEqualTo(expectedContestId);
        }
    }
}

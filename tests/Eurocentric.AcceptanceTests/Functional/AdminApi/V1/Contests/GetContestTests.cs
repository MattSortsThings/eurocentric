using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Features.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests;

[Category("admin-api")]
public sealed class GetContestTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_dummy_contest(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_contest_with_ID("00bb7d00-c0ed-41f1-b2a6-710806cb46b4");

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_contest_should_have_ID("00bb7d00-c0ed-41f1-b2a6-710806cb46b4");
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetContestResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_the_contest_with_ID(string contestId)
        {
            Guid targetId = Guid.Parse(contestId);

            Request = Kernel.Requests.Contests.GetContest(targetId);
        }

        public async Task Then_the_retrieved_contest_should_have_ID(string contestId)
        {
            Guid expectedId = Guid.Parse(contestId);

            await Assert
                .That(SuccessResponse?.Data?.Contest)
                .IsNotNull()
                .And.HasProperty(contest => contest.Id, expectedId);
        }
    }
}

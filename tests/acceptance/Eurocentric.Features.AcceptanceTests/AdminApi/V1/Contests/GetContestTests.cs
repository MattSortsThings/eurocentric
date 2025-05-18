using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests;

public sealed class GetContestTests : AcceptanceTestBase
{
    public GetContestTests(WebAppFixture webAppFixture) : base(webAppFixture) { }

    private protected override int ApiMajorVersion => 1;

    private protected override int ApiMinorVersion => 0;

    [Fact]
    public async Task Should_be_able_to_retrieve_dummy_contest()
    {
        AdminActor admin = AdminActor.WithDriver(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_contest_with_ID("0c0d09b1-b357-44fb-ae4a-2504341937f2");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_contest_should_have_the_target_contest_ID();
    }

    private sealed class AdminActor : ActorWithResponse<GetContestResponse>
    {
        private readonly AdminApiV1Driver _driver;

        private AdminActor(AdminApiV1Driver driver)
        {
            _driver = driver;
        }

        private Guid TargetContestId { get; set; }

        private protected override Func<Task<ResponseOrProblem<GetContestResponse>>> SendMyRequest { get; set; } = null!;

        public void Given_I_want_to_retrieve_the_contest_with_ID(string contestId)
        {
            TargetContestId = Guid.Parse(contestId);
            SendMyRequest = () => _driver.GetContestAsync(TargetContestId, TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_contest_should_have_the_target_contest_ID()
        {
            Assert.NotNull(Response);

            Assert.Equal(TargetContestId, Response.Contest.Id);
        }

        public static AdminActor WithDriver(AdminApiV1Driver driver) => new(driver);
    }
}

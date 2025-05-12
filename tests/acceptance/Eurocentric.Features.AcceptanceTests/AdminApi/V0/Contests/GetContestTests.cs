using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V0.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Contests;

public abstract class GetContestTests : AcceptanceTestBase
{
    protected GetContestTests(WebAppFixture webAppFixture) : base(webAppFixture) { }

    [Fact]
    public async Task Should_be_able_to_retrieve_a_contest_by_its_ID()
    {
        AdminActor admin = new(AdminApiV0Driver.Create(Client, MajorApiVersion, MinorApiVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_contest_with_id("9fe06015-eeeb-4e0b-9446-767b161caeed");

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_contest_should_have_my_target_contest_ID();
    }

    public sealed class V0Point1 : GetContestTests
    {
        public V0Point1(WebAppFixture webAppFixture) : base(webAppFixture) { }

        private protected override int MajorApiVersion => 0;

        private protected override int MinorApiVersion => 1;
    }

    public sealed class V0Point2 : GetContestTests
    {
        public V0Point2(WebAppFixture webAppFixture) : base(webAppFixture) { }

        private protected override int MajorApiVersion => 0;

        private protected override int MinorApiVersion => 2;
    }

    private sealed class AdminActor : ActorWithResponse<GetContest.Response>
    {
        private readonly AdminApiV0Driver _driver;

        public AdminActor(AdminApiV0Driver driver)
        {
            _driver = driver;
        }

        private Guid MyTargetContestId { get; set; }

        private protected override Func<Task<ResponseOrProblem<GetContest.Response>>> SendMyRequest { get; set; } = null!;

        public void Given_I_want_to_retrieve_the_contest_with_id(string contestId)
        {
            MyTargetContestId = Guid.Parse(contestId);

            SendMyRequest = () => _driver.GetContestAsync(MyTargetContestId, TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_contest_should_have_my_target_contest_ID()
        {
            Assert.NotNull(Response);

            Assert.Equal(MyTargetContestId, Response.Contest.Id);
        }
    }
}

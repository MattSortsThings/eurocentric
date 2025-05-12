using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V0.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Contests;

public abstract class GetContestsTests : AcceptanceTestBase
{
    protected GetContestsTests(WebAppFixture webAppFixture) : base(webAppFixture) { }

    [Fact]
    public async Task Should_be_able_to_retrieve_all_existing_contests()
    {
        AdminActor admin = new(AdminApiV0Driver.Create(Client, MajorApiVersion, MinorApiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
    }

    public sealed class V0Point1 : GetContestsTests
    {
        public V0Point1(WebAppFixture webAppFixture) : base(webAppFixture) { }

        private protected override int MajorApiVersion => 0;

        private protected override int MinorApiVersion => 1;
    }

    public sealed class V0Point2 : GetContestsTests
    {
        public V0Point2(WebAppFixture webAppFixture) : base(webAppFixture) { }

        private protected override int MajorApiVersion => 0;

        private protected override int MinorApiVersion => 2;
    }

    private sealed class AdminActor : ActorWithResponse<GetContests.Response>
    {
        private readonly AdminApiV0Driver _driver;

        public AdminActor(AdminApiV0Driver driver)
        {
            _driver = driver;
        }

        private protected override Func<Task<ResponseOrProblem<GetContests.Response>>> SendMyRequest { get; set; } = null!;

        public void Given_I_want_to_retrieve_all_existing_contests() =>
            SendMyRequest = () => _driver.GetContestsAsync(TestContext.Current.CancellationToken);
    }
}

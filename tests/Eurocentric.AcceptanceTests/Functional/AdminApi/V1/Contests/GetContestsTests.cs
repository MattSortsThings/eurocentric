using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Features.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests;

[Category("admin-api")]
public sealed class GetContestsTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_dummy_contests(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_contests_should_be_in_contest_year_order();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetContestsResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_all_existing_contests() =>
            Request = Kernel.Requests.Contests.GetContests();

        public async Task Then_the_retrieved_contests_should_be_in_contest_year_order()
        {
            await Assert
                .That(SuccessResponse?.Data?.Contests)
                .IsNotEmpty()
                .And.IsOrderedBy(contest => contest.ContestYear);
        }
    }
}

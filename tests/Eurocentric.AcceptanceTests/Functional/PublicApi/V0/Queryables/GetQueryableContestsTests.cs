using Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils;
using Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils.Attributes;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V0.Features.Queryables;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V0.Queryables;

[Category("public-api")]
public sealed class GetQueryableContestsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion0Point2AndUp]
    public async Task Should_retrieve_all_queryable_contests_in_contest_year_order(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_the_queryable_contests();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_queryable_contests_should_have_count(2);
        await euroFan.Then_the_queryable_contests_should_be_in_contest_year_order();
    }

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetQueryableContestsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_all_the_queryable_contests() =>
            Request = Kernel.Requests.Queryables.GetQueryableContests();

        public async Task Then_the_queryable_contests_should_have_count(int count)
        {
            GetQueryableContestsResponse response = await Assert.That(SuccessResponse?.Data).IsNotNull();

            await Assert.That(response.QueryableContests).HasCount(count);
        }

        public async Task Then_the_queryable_contests_should_be_in_contest_year_order()
        {
            GetQueryableContestsResponse response = await Assert.That(SuccessResponse?.Data).IsNotNull();

            await Assert.That(response.QueryableContests).IsOrderedBy(contest => contest.ContestYear);
        }
    }
}

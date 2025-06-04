using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Contests;
using Eurocentric.Infrastructure.InMemoryRepositories;
using Microsoft.Extensions.DependencyInjection;
using PlaceholderContest = Eurocentric.Domain.Placeholders.Contest;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Contests;

public sealed class GetContestsTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v0.1")]
    [InlineData("v0.2")]
    public async Task Should_be_able_to_retrieve_all_existing_contests_in_contest_year_order(string apiVersion)
    {
        AdminActor admin = new(AdminApiV0Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        admin.Given_I_have_created_a_contest(contestYear: 2024, cityName: "Malmö");
        admin.Given_I_have_created_a_contest(contestYear: 2023, cityName: "Liverpool");
        admin.Given_I_have_created_a_contest(contestYear: 2025, cityName: "Basel");
        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order();
    }

    private sealed class AdminActor : ActorWithResponse<GetContestsResponse>
    {
        public AdminActor(IAdminApiV0Driver apiDriver, IWebAppFixtureBackDoor backDoor) : base(apiDriver)
        {
            BackDoor = backDoor;
        }

        private IWebAppFixtureBackDoor BackDoor { get; }

        private List<Contest> MyContests { get; } = new(3);

        public void Given_I_have_created_a_contest(string cityName = "", int contestYear = 0)
        {
            PlaceholderContest contest = PlaceholderContest.CreateStockholmFormat(contestYear, cityName);

            Action<IServiceProvider> add = sp =>
            {
                InMemoryContestRepository repository = sp.GetRequiredService<InMemoryContestRepository>();

                repository.Contests.Add(contest);
            };

            BackDoor.ExecuteScoped(add);
            MyContests.Add(contest.ToContestDto());
        }

        public void Given_I_want_to_retrieve_all_existing_contests() =>
            SendMyRequest = apiDriver => apiDriver.Contests.GetContests(TestContext.Current.CancellationToken);

        public void Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order()
        {
            Assert.NotNull(ResponseObject);

            IOrderedEnumerable<Contest> expected = MyContests.OrderBy(contest => contest.ContestYear);

            Assert.Equal(expected, ResponseObject.Contests);
        }
    }
}

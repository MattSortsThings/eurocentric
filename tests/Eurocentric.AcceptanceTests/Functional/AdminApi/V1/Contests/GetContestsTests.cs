using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests.TestUtils;
using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Features.Contests;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests;

[Category("admin-api")]
public sealed class GetContestsTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_all_existing_contests_in_contest_year_order(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_Liverpool_rules_contest_with_dummy_countries(
            contestYear: 2023,
            cityName: "Liverpool"
        );
        await admin.Given_I_have_created_a_Stockholm_rules_contest_with_dummy_countries(
            contestYear: 2016,
            cityName: "Stockholm"
        );
        await admin.Given_I_have_created_a_Stockholm_rules_contest_with_dummy_countries(
            contestYear: 2017,
            cityName: "Kyiv"
        );
        await admin.Given_I_have_created_a_Liverpool_rules_contest_with_dummy_countries(
            contestYear: 2021,
            cityName: "Rotterdam"
        );
        await admin.Given_I_have_created_a_Liverpool_rules_contest_with_dummy_countries(
            contestYear: 2024,
            cityName: "Malmö"
        );

        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_list_when_no_contests_exist(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_contests();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_contests_should_be_an_empty_list();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetContestsResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private List<Contest> ExistingContests { get; } = [];

        public async Task Given_I_have_created_a_Liverpool_rules_contest_with_dummy_countries(
            string cityName = "",
            int contestYear = 0
        )
        {
            Contest contest = await Kernel.CreateADummyLiverpoolRulesContestAsync(contestYear, cityName);

            ExistingContests.Add(contest);
        }

        public async Task Given_I_have_created_a_Stockholm_rules_contest_with_dummy_countries(
            string cityName = "",
            int contestYear = 0
        )
        {
            Contest contest = await Kernel.CreateADummyStockholmRulesContestAsync(contestYear, cityName);

            ExistingContests.Add(contest);
        }

        public void Given_I_want_to_retrieve_all_existing_contests() =>
            Request = Kernel.Requests.Contests.GetContests();

        public async Task Then_the_retrieved_contests_should_be_my_contests_in_contest_year_order()
        {
            IOrderedEnumerable<Contest> expectedContests = ExistingContests.OrderBy(contest => contest.ContestYear);

            await Assert
                .That(SuccessResponse?.Data?.Contests)
                .IsEquivalentTo(expectedContests, new ContestEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_contests_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Contests).IsEmpty();
    }
}

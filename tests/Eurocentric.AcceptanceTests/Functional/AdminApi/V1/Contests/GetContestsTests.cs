using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
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
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "GB", "HR", "XX");

        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2024,
            cityName: "Malmö",
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2016,
            cityName: "Stockholm",
            semiFinal1Countries: ["AT", "BE", "CZ"],
            semiFinal2Countries: ["DK", "EE", "FI"]
        );
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            cityName: "Basel",
            globalTelevoteCountry: "XX",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
        );
        await admin.Given_I_have_created_a_Stockholm_format_contest(
            contestYear: 2021,
            cityName: "Rotterdam",
            semiFinal1Countries: ["AT", "BE", "CZ", "DK"],
            semiFinal2Countries: ["EE", "FI", "GB", "HR"]
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

        private CountryIdLookup ExistingCountryIds { get; } = new();

        private List<Contest> ExistingContests { get; } = [];

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            ExistingCountryIds.EnsureCapacity(countryCodes.Length);

            await foreach (Country country in Kernel.CreateMultipleCountriesAsync(countryCodes))
            {
                ExistingCountryIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Liverpool_format_contest(
            string globalTelevoteCountry,
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            string cityName = "",
            int contestYear = 0
        )
        {
            Contest contest = await Kernel.CreateALiverpoolRulesContestAsync(
                contestYear: contestYear,
                cityName: cityName,
                semiFinal1CountryIds: semiFinal1Countries.Select(ExistingCountryIds.GetId).ToArray(),
                semiFinal2CountryIds: semiFinal2Countries.Select(ExistingCountryIds.GetId).ToArray(),
                globalTelevoteVotingCountryId: ExistingCountryIds.GetId(globalTelevoteCountry)
            );

            ExistingContests.Add(contest);
        }

        public async Task Given_I_have_created_a_Stockholm_format_contest(
            string[] semiFinal2Countries = null!,
            string[] semiFinal1Countries = null!,
            string cityName = "",
            int contestYear = 0
        )
        {
            Contest contest = await Kernel.CreateAStockholmRulesContestAsync(
                contestYear: contestYear,
                cityName: cityName,
                semiFinal1CountryIds: semiFinal1Countries.Select(ExistingCountryIds.GetId).ToArray(),
                semiFinal2CountryIds: semiFinal2Countries.Select(ExistingCountryIds.GetId).ToArray()
            );

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

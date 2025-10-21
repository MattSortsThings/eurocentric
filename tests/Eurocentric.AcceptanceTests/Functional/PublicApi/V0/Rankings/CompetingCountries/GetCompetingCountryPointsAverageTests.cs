using Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils;
using Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils.Attributes;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V0.Dtos.Rankings.CompetingCountries;
using Eurocentric.Apis.Public.V0.Enums;
using Eurocentric.Apis.Public.V0.Features.Rankings.CompetingCountries;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V0.Rankings.CompetingCountries;

[Category("public-api")]
public sealed class GetCompetingCountryPointsAverageTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion0Point2AndUp]
    public async Task Should_retrieve_competing_country_points_average_rankings_page_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_response_rankings_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden         | 7.425837      | 1552        | 209          | 4          | 2        | 40              |
            | 2    | UA          | Ukraine        | 6.475936      | 1211        | 187          | 3          | 2        | 40              |
            | 3    | FI          | Finland        | 4.320574      | 903         | 209          | 4          | 2        | 40              |
            | 4    | IL          | Israel         | 4.198473      | 550         | 131          | 3          | 2        | 39              |
            | 5    | IT          | Italy          | 4.092715      | 618         | 151          | 2          | 2        | 40              |
            | 6    | ES          | Spain          | 3.701987      | 559         | 151          | 2          | 2        | 40              |
            | 7    | NO          | Norway         | 3.556098      | 729         | 205          | 4          | 2        | 40              |
            | 8    | GR          | Greece         | 3.308271      | 440         | 133          | 3          | 2        | 40              |
            | 9    | GB          | United Kingdom | 3.245033      | 490         | 151          | 2          | 2        | 40              |
            | 10   | AU          | Australia      | 3.180952      | 668         | 210          | 4          | 2        | 40              |
            """
        );
        await euroFan.Then_the_response_metadata_should_match(
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 40,
            totalPages: 4
        );
    }

    [Test]
    [ApiVersion0Point2AndUp]
    public async Task Should_retrieve_competing_country_points_average_rankings_page_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(
            minYear: 2022,
            maxYear: 2022,
            contestStage: "Any",
            votingMethod: "Jury",
            pageSize: 2,
            pageIndex: 1
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_response_rankings_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 3    | GB          | United Kingdom | 5.974359      | 466         | 78           | 1          | 1        | 39              |
            | 4    | ES          | Spain          | 5.884615      | 459         | 78           | 1          | 1        | 39              |
            """
        );
        await euroFan.Then_the_response_metadata_should_match(
            minYear: 2022,
            maxYear: 2022,
            contestStage: "Any",
            votingMethod: "Jury",
            pageIndex: 1,
            pageSize: 2,
            descending: false,
            totalItems: 40,
            totalPages: 20
        );
    }

    [Test]
    [ApiVersion0Point2AndUp]
    public async Task Should_retrieve_empty_rankings_page_when_no_data_matches_query(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(
            minYear: 1066,
            maxYear: 1666
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_response_rankings_should_be_an_empty_list();
        await euroFan.Then_the_response_metadata_should_match(
            minYear: 1066,
            maxYear: 1666,
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 0,
            totalPages: 0
        );
    }

    [Test]
    [ApiVersion0Point2AndUp]
    public async Task Should_fail_on_illegal_page_index_value(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(pageIndex: -1);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal page index value",
            detail: "Page index value must be a non-negative integer."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "pageIndex", value: -1);
    }

    [Test]
    [ApiVersion0Point2AndUp]
    public async Task Should_fail_on_illegal_page_size_value_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(pageSize: 0);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal page size value",
            detail: "Page size value must be an integer between 1 and 100."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "pageSize", value: 0);
    }

    [Test]
    [ApiVersion0Point2AndUp]
    public async Task Should_fail_on_illegal_page_size_value_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(pageSize: 101);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal page size value",
            detail: "Page size value must be an integer between 1 and 100."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "pageSize", value: 101);
    }

    [Test]
    [ApiVersion0Point2AndUp]
    public async Task Should_fail_on_illegal_contest_year_range(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(
            minYear: 2023,
            maxYear: 2022
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal contest year range",
            detail: "Maximum contest year must be greater than or equal to minimum contest year."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "minYear", value: 2023);
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "maxYear", value: 2022);
    }

    [Test]
    [ApiVersion0Point2AndUp]
    public async Task Should_fail_on_illegal_voting_country_code_value(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(votingCountryCode: "!");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal voting country code value",
            detail: "Voting country code value must be a string of 2 upper-case letters."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "votingCountryCode", value: "!");
    }

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetCompetingCountryPointsAverageRankingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(
            bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingMethod = null,
            string? votingCountryCode = null,
            string? contestStage = null,
            int? maxYear = null,
            int? minYear = null
        )
        {
            Dictionary<string, object?> queryParameters = new()
            {
                { nameof(descending), descending },
                { nameof(pageSize), pageSize },
                { nameof(pageIndex), pageIndex },
                { nameof(votingMethod), votingMethod is null ? null : Enum.Parse<VotingMethodFilter>(votingMethod) },
                { nameof(votingCountryCode), votingCountryCode },
                { nameof(contestStage), contestStage is null ? null : Enum.Parse<ContestStageFilter>(contestStage) },
                { nameof(maxYear), maxYear },
                { nameof(minYear), minYear },
            };

            Request = Kernel.Requests.CompetingCountryRankings.GetCompetingCountryPointsAverageRankings(
                queryParameters
            );
        }

        public async Task Then_the_response_rankings_should_match(string table)
        {
            CompetingCountryPointsAverageRanking[] expectedRankings = MarkdownParser
                .ParseTable(table, MapRow)
                .ToArray();

            await Assert.That(SuccessResponse?.Data?.Rankings).IsNotNull().And.IsEquivalentTo(expectedRankings);
        }

        public async Task Then_the_response_rankings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Rankings).IsNotNull().And.IsEmpty();

        public async Task Then_the_response_metadata_should_match(
            int totalPages = 0,
            int totalItems = 0,
            bool descending = false,
            int pageSize = 0,
            int pageIndex = 0,
            string? votingMethod = null,
            string? votingCountryCode = null,
            string? contestStage = null,
            int? maxYear = null,
            int? minYear = null
        )
        {
            CompetingCountryPointsAverageMetadata expectedMetadata = new()
            {
                MinYear = minYear,
                MaxYear = maxYear,
                ContestStage = contestStage is null ? null : Enum.Parse<ContestStageFilter>(contestStage),
                VotingCountryCode = votingCountryCode,
                VotingMethod = votingMethod is null ? null : Enum.Parse<VotingMethodFilter>(votingMethod),
                PageIndex = pageIndex,
                PageSize = pageSize,
                Descending = descending,
                TotalItems = totalItems,
                TotalPages = totalPages,
            };

            await Assert.That(SuccessResponse?.Data?.Metadata).IsNotNull().And.IsEqualTo(expectedMetadata);
        }

        private static CompetingCountryPointsAverageRanking MapRow(IDictionary<string, string> row)
        {
            return new CompetingCountryPointsAverageRanking
            {
                Rank = int.Parse(row["Rank"]),
                CountryCode = row["CountryCode"],
                CountryName = row["CountryName"],
                PointsAverage = decimal.Parse(row["PointsAverage"]),
                TotalPoints = int.Parse(row["TotalPoints"]),
                PointsAwards = int.Parse(row["PointsAwards"]),
                Broadcasts = int.Parse(row["Broadcasts"]),
                Contests = int.Parse(row["Contests"]),
                VotingCountries = int.Parse(row["VotingCountries"]),
            };
        }
    }
}

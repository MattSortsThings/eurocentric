using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.Rankings.CompetingCountries;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.Rankings.CompetingCountries;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.Rankings.CompetingCountries;

[Category("public-api")]
public sealed class GetCompetingCountryPointsAverageRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
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
        await euroFan.Then_the_retrieved_metadata_should_match(
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 40,
            totalPages: 4
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(pageIndex: 2, pageSize: 3);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 7    | NO          | Norway         | 3.556098      | 729         | 205          | 4          | 2        | 40              |
            | 8    | GR          | Greece         | 3.308271      | 440         | 133          | 3          | 2        | 40              |
            | 9    | GB          | United Kingdom | 3.245033      | 490         | 151          | 2          | 2        | 40              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            pageIndex: 2,
            pageSize: 3,
            descending: false,
            totalItems: 40,
            totalPages: 14
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_3(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(
            pageIndex: 0,
            pageSize: 5,
            descending: true
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 40   | DE          | Germany     | 0.15894       | 24          | 151          | 2          | 2        | 40              |
            | 39   | FR          | France      | 0.801325      | 121         | 151          | 2          | 2        | 40              |
            | 38   | BG          | Bulgaria    | 0.805556      | 29          | 36           | 1          | 1        | 18              |
            | 37   | ME          | Montenegro  | 0.825         | 33          | 40           | 1          | 1        | 20              |
            | 36   | SM          | San Marino  | 0.847458      | 50          | 59           | 2          | 2        | 30              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 40,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_4(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(
            minYear: 2022,
            maxYear: 2022,
            contestStage: "Any",
            votingMethod: "Any",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine        | 8.491228      | 968         | 114          | 2          | 1        | 39              |
            | 2    | SE          | Sweden         | 7.067797      | 834         | 118          | 2          | 1        | 39              |
            | 3    | GB          | United Kingdom | 5.974359      | 466         | 78           | 1          | 1        | 39              |
            | 4    | ES          | Spain          | 5.884615      | 459         | 78           | 1          | 1        | 39              |
            | 5    | RS          | Serbia         | 4.652542      | 549         | 118          | 2          | 1        | 39              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minYear: 2022,
            maxYear: 2022,
            contestStage: "Any",
            votingMethod: "Any",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_5(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(
            contestStage: "SemiFinals",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine     | 9.361111      | 337         | 36           | 1          | 1        | 18              |
            | 2    | SE          | Sweden      | 9.155172      | 531         | 58           | 2          | 2        | 30              |
            | 3    | AU          | Australia   | 6.644068      | 392         | 59           | 2          | 2        | 30              |
            | 4    | FI          | Finland     | 5.844828      | 339         | 58           | 2          | 2        | 30              |
            | 5    | CZ          | Czechia     | 5.810345      | 337         | 58           | 2          | 2        | 30              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestStage: "SemiFinals",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 35,
            totalPages: 7
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_6(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(
            votingMethod: "Jury",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 8.631579      | 820         | 95           | 3          | 2        | 39              |
            | 2    | GR          | Greece      | 5.421053      | 309         | 57           | 2          | 1        | 39              |
            | 3    | NL          | Netherlands | 4.754386      | 271         | 57           | 2          | 1        | 39              |
            | 4    | IT          | Italy       | 4.453333      | 334         | 75           | 2          | 2        | 39              |
            | 5    | AU          | Australia   | 4.442105      | 422         | 95           | 3          | 2        | 39              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            votingMethod: "Jury",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_7(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(
            votingMethod: "Jury",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 8.631579      | 820         | 95           | 3          | 2        | 39              |
            | 2    | GR          | Greece      | 5.421053      | 309         | 57           | 2          | 1        | 39              |
            | 3    | NL          | Netherlands | 4.754386      | 271         | 57           | 2          | 1        | 39              |
            | 4    | IT          | Italy       | 4.453333      | 334         | 75           | 2          | 2        | 39              |
            | 5    | AU          | Australia   | 4.442105      | 422         | 95           | 3          | 2        | 39              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            votingMethod: "Jury",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_8(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(
            votingMethod: "Televote",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine     | 8.829787      | 830         | 94           | 3          | 2        | 40              |
            | 2    | SE          | Sweden      | 6.421053      | 732         | 114          | 4          | 2        | 40              |
            | 3    | FI          | Finland     | 5.947368      | 678         | 114          | 4          | 2        | 40              |
            | 4    | NO          | Norway      | 5.071429      | 568         | 112          | 4          | 2        | 40              |
            | 5    | MD          | Moldova     | 4.991071      | 559         | 112          | 4          | 2        | 40              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            votingMethod: "Televote",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_9(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_average_rankings(
            votingCountryCode: "GB",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 8.333333      | 50          | 6            | 3          | 2        | 1               |
            | 2    | PL          | Poland      | 7.428571      | 52          | 7            | 4          | 2        | 1               |
            | 3    | LT          | Lithuania   | 7.4           | 37          | 5            | 3          | 2        | 1               |
            | 4    | IE          | Ireland     | 6             | 12          | 2            | 1          | 1        | 1               |
            | 5    | ES          | Spain       | 5             | 20          | 4            | 2          | 2        | 1               |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            votingCountryCode: "GB",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 37,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
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
        await euroFan.Then_the_retrieved_rankings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_metadata_should_match(
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
    [ApiVersion1Point0AndUp]
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
    [ApiVersion1Point0AndUp]
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
    [ApiVersion1Point0AndUp]
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
    [ApiVersion1Point0AndUp]
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
    [ApiVersion1Point0AndUp]
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
            Dictionary<string, object?> queryParams = new()
            {
                { nameof(descending), descending },
                { nameof(pageSize), pageSize },
                { nameof(pageIndex), pageIndex },
                { nameof(votingMethod), votingMethod },
                { nameof(votingCountryCode), votingCountryCode },
                { nameof(contestStage), contestStage },
                { nameof(maxYear), maxYear },
                { nameof(minYear), minYear },
            };

            Request = Kernel.Requests.CompetingCountryRankings.GetCompetingCountryPointsAverageRankings(queryParams);
        }

        public async Task Then_the_retrieved_rankings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Rankings).IsEmpty();

        public async Task Then_the_retrieved_rankings_in_order_should_match(string table)
        {
            CompetingCountryPointsAverageRanking[] expected = MarkdownParser.ParseTable(table, MapToRanking);

            await Assert
                .That(SuccessResponse?.Data?.Rankings)
                .IsEquivalentTo(expected, new RankingEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_metadata_should_match(
            int totalPages = -1,
            int totalItems = -1,
            bool descending = true,
            int pageSize = -1,
            int pageIndex = -1,
            string? votingMethod = null,
            string? votingCountryCode = null,
            string? contestStage = null,
            int? maxYear = null,
            int? minYear = null
        )
        {
            ContestStageFilter? expectedContestStage = contestStage.ToNullableContestStageFilter();
            VotingMethodFilter? expectedVotingMethod = votingMethod.ToNullableVotingMethodFilter();

            await Assert
                .That(SuccessResponse?.Data?.Metadata)
                .HasProperty(cc => cc.MinYear, minYear)
                .And.HasProperty(cc => cc.MaxYear, maxYear)
                .And.HasProperty(cc => cc.ContestStage, expectedContestStage)
                .And.HasProperty(cc => cc.VotingCountryCode, votingCountryCode)
                .And.HasProperty(cc => cc.VotingMethod, expectedVotingMethod)
                .And.HasProperty(cc => cc.PageIndex, pageIndex)
                .And.HasProperty(cc => cc.PageSize, pageSize)
                .And.HasProperty(cc => cc.Descending, descending)
                .And.HasProperty(cc => cc.TotalItems, totalItems)
                .And.HasProperty(cc => cc.TotalPages, totalPages);
        }

        private static CompetingCountryPointsAverageRanking MapToRanking(Dictionary<string, string> row)
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

        private sealed class RankingEqualityComparer : IEqualityComparer<CompetingCountryPointsAverageRanking>
        {
            public bool Equals(CompetingCountryPointsAverageRanking? x, CompetingCountryPointsAverageRanking? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x is null)
                {
                    return false;
                }

                if (y is null)
                {
                    return false;
                }

                if (x.GetType() != y.GetType())
                {
                    return false;
                }

                return x.Rank == y.Rank
                    && x.CountryCode.Equals(y.CountryCode, StringComparison.Ordinal)
                    && x.CountryName.Equals(y.CountryName, StringComparison.Ordinal)
                    && x.PointsAverage == y.PointsAverage
                    && x.TotalPoints == y.TotalPoints
                    && x.PointsAwards == y.PointsAwards
                    && x.Broadcasts == y.Broadcasts
                    && x.Contests == y.Contests
                    && x.VotingCountries == y.VotingCountries;
            }

            public int GetHashCode(CompetingCountryPointsAverageRanking obj)
            {
                HashCode hashCode = new();
                hashCode.Add(obj.Rank);
                hashCode.Add(obj.CountryCode);
                hashCode.Add(obj.CountryName);
                hashCode.Add(obj.PointsAverage);
                hashCode.Add(obj.TotalPoints);
                hashCode.Add(obj.PointsAwards);
                hashCode.Add(obj.Broadcasts);
                hashCode.Add(obj.Contests);
                hashCode.Add(obj.VotingCountries);

                return hashCode.ToHashCode();
            }
        }
    }
}

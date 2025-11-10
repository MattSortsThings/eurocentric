using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.CompetingCountryRankings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.CompetingCountryRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.CompetingCountryRankings;

[Category("public-api")]
public sealed class GetCompetingCountryPointsShareRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden         | 0.61882     | 1552        | 2508            | 209          | 4          | 2        | 40              |
            | 2    | UA          | Ukraine        | 0.539661    | 1211        | 2244            | 187          | 3          | 2        | 40              |
            | 3    | FI          | Finland        | 0.360048    | 903         | 2508            | 209          | 4          | 2        | 40              |
            | 4    | IL          | Israel         | 0.349873    | 550         | 1572            | 131          | 3          | 2        | 39              |
            | 5    | IT          | Italy          | 0.34106     | 618         | 1812            | 151          | 2          | 2        | 40              |
            | 6    | ES          | Spain          | 0.308499    | 559         | 1812            | 151          | 2          | 2        | 40              |
            | 7    | NO          | Norway         | 0.296341    | 729         | 2460            | 205          | 4          | 2        | 40              |
            | 8    | GR          | Greece         | 0.275689    | 440         | 1596            | 133          | 3          | 2        | 40              |
            | 9    | GB          | United Kingdom | 0.270419    | 490         | 1812            | 151          | 2          | 2        | 40              |
            | 10   | AU          | Australia      | 0.265079    | 668         | 2520            | 210          | 4          | 2        | 40              |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(pageIndex: 2, pageSize: 3);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 7    | NO          | Norway         | 0.296341    | 729         | 2460            | 205          | 4          | 2        | 40              |
            | 8    | GR          | Greece         | 0.275689    | 440         | 1596            | 133          | 3          | 2        | 40              |
            | 9    | GB          | United Kingdom | 0.270419    | 490         | 1812            | 151          | 2          | 2        | 40              |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(
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
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 40   | DE          | Germany     | 0.013245    | 24          | 1812            | 151          | 2          | 2        | 40              |
            | 39   | FR          | France      | 0.066777    | 121         | 1812            | 151          | 2          | 2        | 40              |
            | 38   | BG          | Bulgaria    | 0.06713     | 29          | 432             | 36           | 1          | 1        | 18              |
            | 37   | ME          | Montenegro  | 0.06875     | 33          | 480             | 40           | 1          | 1        | 20              |
            | 36   | SM          | San Marino  | 0.070621    | 50          | 708             | 59           | 2          | 2        | 30              |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(
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
            | Rank | CountryCode | CountryName    | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine        | 0.707602    | 968         | 1368            | 114          | 2          | 1        | 39              |
            | 2    | SE          | Sweden         | 0.588983    | 834         | 1416            | 118          | 2          | 1        | 39              |
            | 3    | GB          | United Kingdom | 0.497863    | 466         | 936             | 78           | 1          | 1        | 39              |
            | 4    | ES          | Spain          | 0.490385    | 459         | 936             | 78           | 1          | 1        | 39              |
            | 5    | RS          | Serbia         | 0.387712    | 549         | 1416            | 118          | 2          | 1        | 39              |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(
            contestStage: "SemiFinals",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine     | 0.780093    | 337         | 432             | 36           | 1          | 1        | 18              |
            | 2    | SE          | Sweden      | 0.762931    | 531         | 696             | 58           | 2          | 2        | 30              |
            | 3    | AU          | Australia   | 0.553672    | 392         | 708             | 59           | 2          | 2        | 30              |
            | 4    | FI          | Finland     | 0.487069    | 339         | 696             | 58           | 2          | 2        | 30              |
            | 5    | CZ          | Czechia     | 0.484195    | 337         | 696             | 58           | 2          | 2        | 30              |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(
            votingMethod: "Jury",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.719298    | 820         | 1140            | 95           | 3          | 2        | 39              |
            | 2    | GR          | Greece      | 0.451754    | 309         | 684             | 57           | 2          | 1        | 39              |
            | 3    | NL          | Netherlands | 0.396199    | 271         | 684             | 57           | 2          | 1        | 39              |
            | 4    | IT          | Italy       | 0.371111    | 334         | 900             | 75           | 2          | 2        | 39              |
            | 5    | AU          | Australia   | 0.370175    | 422         | 1140            | 95           | 3          | 2        | 39              |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(
            votingMethod: "Televote",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine     | 0.735816    | 830         | 1128            | 94           | 3          | 2        | 40              |
            | 2    | SE          | Sweden      | 0.535088    | 732         | 1368            | 114          | 4          | 2        | 40              |
            | 3    | FI          | Finland     | 0.495614    | 678         | 1368            | 114          | 4          | 2        | 40              |
            | 4    | NO          | Norway      | 0.422619    | 568         | 1344            | 112          | 4          | 2        | 40              |
            | 5    | MD          | Moldova     | 0.415923    | 559         | 1344            | 112          | 4          | 2        | 40              |
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
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_8(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(
            votingCountryCode: "GB",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.694444    | 50          | 72              | 6            | 3          | 2        | 1               |
            | 2    | PL          | Poland      | 0.619048    | 52          | 84              | 7            | 4          | 2        | 1               |
            | 3    | LT          | Lithuania   | 0.616667    | 37          | 60              | 5            | 3          | 2        | 1               |
            | 4    | IE          | Ireland     | 0.5         | 12          | 24              | 2            | 1          | 1        | 1               |
            | 5    | ES          | Spain       | 0.416667    | 20          | 48              | 4            | 2          | 2        | 1               |
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
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_9(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(
            minYear: 2023,
            contestStage: "Any",
            votingMethod: "Televote",
            votingCountryCode: "GB",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | FI          | Finland     | 1           | 12          | 12              | 1            | 1          | 1        | 1               |
            | 2    | LT          | Lithuania   | 0.916667    | 22          | 24              | 2            | 2          | 1        | 1               |
            | 3    | PL          | Poland      | 0.75        | 18          | 24              | 2            | 2          | 1        | 1               |
            | 4    | NO          | Norway      | 0.583333    | 7           | 12              | 1            | 1          | 1        | 1               |
            | 5    | BE          | Belgium     | 0.5         | 12          | 24              | 2            | 2          | 1        | 1               |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minYear: 2023,
            contestStage: "Any",
            votingMethod: "Televote",
            votingCountryCode: "GB",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 31,
            totalPages: 7
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_rankings_page_when_no_data_matches_query(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(
            minYear: 1066,
            maxYear: 1963,
            votingCountryCode: "ZZ"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_metadata_should_match(
            minYear: 1066,
            maxYear: 1963,
            votingCountryCode: "ZZ",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(pageIndex: -1);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(pageSize: 0);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(pageSize: 101);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(votingCountryCode: "!");

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

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetCompetingCountryPointsShareRankingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_a_page_of_competing_country_points_share_rankings(
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

            Request = Kernel.Requests.CompetingCountryRankings.GetCompetingCountryPointsShareRankings(queryParams);
        }

        public async Task Then_the_retrieved_rankings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Rankings).IsEmpty();

        public async Task Then_the_retrieved_rankings_in_order_should_match(string table)
        {
            CompetingCountryPointsShareRanking[] expected = MarkdownParser.ParseTable(table, MapToRanking);

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

        private static CompetingCountryPointsShareRanking MapToRanking(Dictionary<string, string> row)
        {
            return new CompetingCountryPointsShareRanking
            {
                Rank = int.Parse(row["Rank"]),
                CountryCode = row["CountryCode"],
                CountryName = row["CountryName"],
                PointsShare = decimal.Parse(row["PointsShare"]),
                TotalPoints = int.Parse(row["TotalPoints"]),
                AvailablePoints = int.Parse(row["AvailablePoints"]),
                PointsAwards = int.Parse(row["PointsAwards"]),
                Broadcasts = int.Parse(row["Broadcasts"]),
                Contests = int.Parse(row["Contests"]),
                VotingCountries = int.Parse(row["VotingCountries"]),
            };
        }

        private sealed class RankingEqualityComparer : IEqualityComparer<CompetingCountryPointsShareRanking>
        {
            public bool Equals(CompetingCountryPointsShareRanking? x, CompetingCountryPointsShareRanking? y)
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
                    && x.PointsShare == y.PointsShare
                    && x.TotalPoints == y.TotalPoints
                    && x.AvailablePoints == y.AvailablePoints
                    && x.PointsAwards == y.PointsAwards
                    && x.Broadcasts == y.Broadcasts
                    && x.Contests == y.Contests
                    && x.VotingCountries == y.VotingCountries;
            }

            public int GetHashCode(CompetingCountryPointsShareRanking obj)
            {
                HashCode hashCode = new();
                hashCode.Add(obj.Rank);
                hashCode.Add(obj.CountryCode);
                hashCode.Add(obj.CountryName);
                hashCode.Add(obj.PointsShare);
                hashCode.Add(obj.TotalPoints);
                hashCode.Add(obj.AvailablePoints);
                hashCode.Add(obj.PointsAwards);
                hashCode.Add(obj.Broadcasts);
                hashCode.Add(obj.Contests);
                hashCode.Add(obj.VotingCountries);

                return hashCode.ToHashCode();
            }
        }
    }
}

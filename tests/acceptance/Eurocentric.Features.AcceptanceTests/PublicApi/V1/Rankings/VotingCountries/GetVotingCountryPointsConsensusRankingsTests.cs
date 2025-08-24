using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsConsensusRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Rankings.VotingCountries;

public sealed class GetVotingCountryPointsConsensusRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_minimum_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName     | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-----------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 1    | IS          | Iceland         | 0.77498         | 72359.333333     | 305.563959       | 305.563959           | 66               | 3          | 2        | 33                 |
            | 2    | AM          | Armenia         | 0.74028         | 69118.666667     | 305.562323       | 305.562323           | 65               | 3          | 2        | 33                 |
            | 3    | ME          | Montenegro      | 0.728125        | 45321.555556     | 249.48792        | 249.48792            | 42               | 2          | 1        | 32                 |
            | 4    | IL          | Israel          | 0.723842        | 67585.333333     | 305.565596       | 305.565596           | 67               | 3          | 2        | 36                 |
            | 5    | MK          | North Macedonia | 0.719747        | 44800.111111     | 249.48792        | 249.48792            | 42               | 2          | 1        | 32                 |
            | 6    | EE          | Estonia         | 0.696487        | 65030.555556     | 305.563959       | 305.563959           | 66               | 3          | 2        | 36                 |
            | 7    | MT          | Malta           | 0.690918        | 64511.888889     | 305.567232       | 305.567232           | 68               | 3          | 2        | 36                 |
            | 8    | NO          | Norway          | 0.680351        | 63523.222222     | 305.562323       | 305.562323           | 65               | 3          | 2        | 33                 |
            | 9    | FR          | France          | 0.655513        | 61204.777778     | 305.563959       | 305.563959           | 66               | 3          | 2        | 33                 |
            | 10   | AZ          | Azerbaijan      | 0.65037         | 60725.222222     | 305.565596       | 305.565596           | 67               | 3          | 2        | 36                 |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            contestStage: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 40,
            totalPages: 4);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_requested_pagination(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(
            pageIndex: 1,
            pageSize: 3,
            descending: true);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDotProduct  | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-------------|-----------------|-------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 37   | GR          | Greece      | 0.415581        | 38802.555556      | 305.563959       | 305.563959           | 66               | 3          | 2        | 33                 |
            | 36   | ES          | Spain       | 0.439974        | 41080.555556      | 305.565596       | 305.565596           | 67               | 3          | 2        | 36                 |
            | 35   | IT          | Italy       | 0.455417        | 42522             | 305.563959       | 305.563959           | 66               | 3          | 2        | 33                 |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            contestStage: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 1,
            pageSize: 3,
            descending: true,
            totalItems: 40,
            totalPages: 14);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_contest_stage(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(
            pageSize: 5,
            contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 1    | ME          | Montenegro  | 0.799611        | 24888.777778     | 176.425937       | 176.425937           | 25               | 1          | 1        | 25                 |
            | 2    | MT          | Malta       | 0.755274        | 47018.222222     | 249.505956       | 249.505956           | 51               | 2          | 2        | 31                 |
            | 3    | IS          | Iceland     | 0.749501        | 46658.111111     | 249.503952       | 249.503952           | 50               | 2          | 2        | 30                 |
            | 4    | AM          | Armenia     | 0.749099        | 46632.333333     | 249.501948       | 249.501948           | 49               | 2          | 2        | 30                 |
            | 5    | HR          | Croatia     | 0.720692        | 44864.666667     | 249.503952       | 249.503952           | 50               | 2          | 2        | 30                 |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            contestStage: "GrandFinal");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_min_year(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(
            pageSize: 5,
            minYear: 2023);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 1    | AM          | Armenia     | 0.887401        | 27621.333333     | 176.425937       | 176.425937           | 25               | 1          | 1        | 25                 |
            | 2    | RS          | Serbia      | 0.872708        | 27164.           | 176.425937       | 176.425937           | 25               | 1          | 1        | 25                 |
            | 3    | MT          | Malta       | 0.812614        | 25294.333333     | 176.428771       | 176.428771           | 26               | 1          | 1        | 26                 |
            | 4    | EE          | Estonia     | 0.809338        | 25191.555556     | 176.425937       | 176.425937           | 25               | 1          | 1        | 25                 |
            | 5    | SE          | Sweden      | 0.773374        | 24072.111111     | 176.425937       | 176.425937           | 25               | 1          | 1        | 25                 |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            contestStage: "Any",
            minYear: 2023);
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 37,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_max_year(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(
            pageSize: 5,
            maxYear: 2022);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName     | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-----------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 1    | IS          | Iceland         | 0.835821        | 52023.333333     | 249.483912       | 249.483912           | 40               | 2          | 1        | 31                 |
            | 2    | IL          | Israel          | 0.736798        | 45861.444444     | 249.48792        | 249.48792            | 42               | 2          | 1        | 32                 |
            | 3    | LT          | Lithuania       | 0.731272        | 45516            | 249.483912       | 249.483912           | 40               | 2          | 1        | 31                 |
            | 4    | ME          | Montenegro      | 0.728125        | 45321.555556     | 249.48792        | 249.48792            | 42               | 2          | 1        | 32                 |
            | 5    | MK          | North Macedonia | 0.719747        | 44800.111111     | 249.48792        | 249.48792            | 42               | 2          | 1        | 32                 |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            contestStage: "Any",
            maxYear: 2022);
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_competing_country_code(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(
            pageSize: 5,
            competingCountryCode: "FI");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 1    | BG          | Bulgaria    | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1                  |
            | 1    | NO          | Norway      | 1               | 10001            | 100.005          | 100.005              | 2                | 2          | 2        | 1                  |
            | 3    | IS          | Iceland     | 0.999998        | 8334.333333      | 83.339333        | 100.005              | 2                | 2          | 2        | 1                  |
            | 3    | NL          | Netherlands | 0.999998        | 8334.333333      | 83.339333        | 100.005              | 2                | 2          | 2        | 1                  |
            | 5    | AM          | Armenia     | 0.999988        | 3334.333333      | 66.674166        | 50.009999            | 2                | 2          | 2        | 1                  |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            contestStage: "Any",
            competingCountryCode: "FI");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 39,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_multiple_facets_scenario_1(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(
            pageSize: 5,
            minYear: 2016,
            maxYear: 2023,
            descending: true,
            contestStage: "SemiFinals");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 40   | DE          | Germany     | 0.340163        | 10585.555556     | 176.406097       | 176.406097           | 18               | 1          | 1        | 18                 |
            | 39   | ES          | Spain       | 0.374643        | 11658.555556     | 176.406097       | 176.406097           | 18               | 1          | 1        | 18                 |
            | 38   | CH          | Switzerland | 0.397041        | 12354.777778     | 176.400428       | 176.400428           | 16               | 1          | 1        | 16                 |
            | 37   | UA          | Ukraine     | 0.433227        | 13480.777778     | 176.400428       | 176.400428           | 16               | 1          | 1        | 16                 |
            | 36   | HR          | Croatia     | 0.473812        | 14743.666667     | 176.400428       | 176.400428           | 16               | 1          | 1        | 16                 |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            minYear: 2016,
            maxYear: 2023,
            contestStage: "SemiFinals");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 40,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_multiple_facets_scenario_2(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(
            pageIndex: 7,
            pageSize: 5,
            descending: false,
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any",
            competingCountryCode: "SI");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 36   | UA          | Ukraine     | 0.734803        | 26               | 25.019992        | 1.414214             | 2                | 2          | 2        | 1                  |
            | 37   | AT          | Austria     | 0.721105        | 51               | 50.009999        | 1.414214             | 2                | 2          | 2        | 1                  |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any",
            competingCountryCode: "SI");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 7,
            pageSize: 5,
            descending: false,
            totalItems: 37,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_empty_rankings_page_when_no_data_matches_filtering(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(
            minYear: 2050,
            maxYear: 2016,
            contestStage: "Any");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_be_empty();
        await euroFan.Then_the_response_query_metadata_should_match(
            minYear: 2050,
            maxYear: 2016,
            contestStage: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 0,
            totalPages: 0);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_pageIndex_query_param_value_less_than_0(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(pageIndex: -1);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Page index out of range",
            detail: "Query parameter 'pageIndex' value must be an integer greater than or equal to 0.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("pageIndex", -1);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_pageSize_query_param_value_less_than_1(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(pageSize: 0);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Page size out of range",
            detail: "Query parameter 'pageSize' value must be an integer between 1 and 100.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("pageSize", 0);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_pageSize_query_param_value_greater_than_100(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(pageSize: 101);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Page size out of range",
            detail: "Query parameter 'pageSize' value must be an integer between 1 and 100.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("pageSize", 101);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_competingCountryCode_query_param_value_that_is_not_string_of_2_upper_case_letters(
        string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(
            competingCountryCode: "NOT_A_COUNTRY_CODE");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Invalid competing country code",
            detail: "Query parameter 'competingCountryCode' value must be a string of 2 upper-case letters.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("competingCountryCode", "NOT_A_COUNTRY_CODE");
    }

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetVotingCountryPointsConsensusRankingsResponse>(apiDriver)
    {
        public void Given_I_want_to_obtain_a_page_of_voting_country_points_consensus_rankings(bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? competingCountryCode = null,
            int? maxYear = null,
            int? minYear = null,
            string? contestStage = null)
        {
            Dictionary<string, object?> queryParams = new()
            {
                [nameof(descending)] = descending,
                [nameof(pageSize)] = pageSize,
                [nameof(pageIndex)] = pageIndex,
                [nameof(competingCountryCode)] = competingCountryCode,
                [nameof(maxYear)] = maxYear,
                [nameof(minYear)] = minYear,
                [nameof(contestStage)] = contestStage is not null ? Enum.Parse<QueryableContestStage>(contestStage) : null
            };

            Request = ApiDriver.RequestFactory.VotingCountryRankings.GetVotingCountryPointsConsensusRankings(queryParams);
        }

        public async Task Then_the_response_rankings_page_should_match(string page)
        {
            (VotingCountryPointsConsensusRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            VotingCountryPointsConsensusRanking[] expected = MarkdownParser.ParseTable(page, MapRowToRanking).ToArray();

            await Assert.That(rankings).IsEquivalentTo(expected, CollectionOrdering.Matching);
        }

        public async Task Then_the_response_rankings_page_should_be_empty()
        {
            (VotingCountryPointsConsensusRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(rankings).IsEmpty();
        }

        public async Task Then_the_response_query_metadata_should_match(
            string? competingCountryCode = null,
            int? maxYear = null,
            int? minYear = null,
            string contestStage = "")
        {
            (_, VotingCountryPointsConsensusQueryMetadata query, _) = await Assert.That(ResponseBody).IsNotNull();

            VotingCountryPointsConsensusQueryMetadata expectedQuery = new()
            {
                ContestStage = Enum.Parse<QueryableContestStage>(contestStage),
                MinYear = minYear,
                MaxYear = maxYear,
                CompetingCountryCode = competingCountryCode
            };

            await Assert.That(query).IsEqualTo(expectedQuery);
        }

        public async Task Then_the_response_pagination_metadata_should_match(int totalPages = 0,
            int totalItems = 0,
            bool descending = true,
            int pageSize = 0,
            int pageIndex = 0)
        {
            (_, _, PaginationMetadata pagination) = await Assert.That(ResponseBody).IsNotNull();

            PaginationMetadata expectedPagination = new()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Descending = descending,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            await Assert.That(pagination).IsEqualTo(expectedPagination);
        }

        private static VotingCountryPointsConsensusRanking MapRowToRanking(Dictionary<string, string> row) => new()
        {
            Rank = int.Parse(row["Rank"]),
            CountryCode = row["CountryCode"],
            CountryName = row["CountryName"],
            PointsConsensus = decimal.Parse(row["PointsConsensus"]),
            VectorDotProduct = decimal.Parse(row["VectorDotProduct"]),
            JuryVectorLength = decimal.Parse(row["JuryVectorLength"]),
            TelevoteVectorLength = decimal.Parse(row["TelevoteVectorLength"]),
            PointsAwardPairs = int.Parse(row["PointsAwardPairs"]),
            Broadcasts = int.Parse(row["Broadcasts"]),
            Contests = int.Parse(row["Contests"]),
            CompetingCountries = int.Parse(row["CompetingCountries"])
        };
    }
}

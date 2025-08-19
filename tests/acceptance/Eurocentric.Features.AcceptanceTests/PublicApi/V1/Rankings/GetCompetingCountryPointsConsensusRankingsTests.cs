using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsConsensusRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Rankings;

public sealed class GetCompetingCountryPointsConsensusRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_minimum_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 1    | ME          | Montenegro     | 0.950696        | 7942             | 64.142203        | 130.239949           | 20               | 1          | 1        | 20              |
            | 2    | SE          | Sweden         | 0.890901        | 381874.222222    | 758.383075       | 565.200358           | 95               | 3          | 2        | 39              |
            | 3    | GB          | United Kingdom | 0.795887        | 99911.555556     | 432.25057        | 290.421341           | 75               | 2          | 2        | 39              |
            | 4    | UA          | Ukraine        | 0.777702        | 292900.666667    | 479.84303        | 784.888952           | 93               | 3          | 2        | 39              |
            | 5    | ES          | Spain          | 0.750112        | 109341           | 422.812015       | 344.75418            | 75               | 2          | 2        | 39              |
            | 6    | IT          | Italy          | 0.739763        | 118607.222222    | 440.195411       | 364.227798           | 75               | 2          | 2        | 39              |
            | 7    | FI          | Finland        | 0.73974         | 140801.222222    | 330.799033       | 575.390882           | 95               | 3          | 2        | 39              |
            | 8    | MT          | Malta          | 0.739316        | 4195.888889      | 96.161901        | 59.018829            | 20               | 1          | 1        | 20              |
            | 9    | IL          | Israel         | 0.695694        | 79058.222222     | 348.138702       | 326.419634           | 56               | 2          | 2        | 38              |
            | 10   | AM          | Armenia        | 0.681803        | 60028.333333     | 296.47803        | 296.964644           | 93               | 3          | 2        | 39              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(
            pageIndex: 1,
            pageSize: 3,
            descending: true);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 37   | DE          | Germany     | 0.164758        | 267              | 20.499322        | 79.054131            | 75               | 2          | 2        | 39              |
            | 36   | BG          | Bulgaria    | 0.173814        | 1584.444444      | 83.844433        | 108.722378           | 18               | 1          | 1        | 18              |
            | 35   | AZ          | Azerbaijan  | 0.205138        | 1704.333333      | 317.905122       | 26.134269            | 59               | 2          | 1        | 39              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(
            pageSize: 5,
            contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 1    | SE          | Sweden         | 0.85727         | 246804.777778    | 631.581261       | 455.834156           | 75               | 2          | 2        | 39              |
            | 2    | GB          | United Kingdom | 0.795887        | 99911.555556     | 432.25057        | 290.421341           | 75               | 2          | 2        | 39              |
            | 3    | IL          | Israel         | 0.781248        | 77696.666667     | 328.840387       | 302.432362           | 36               | 1          | 1        | 36              |
            | 4    | ES          | Spain          | 0.750112        | 109341           | 422.812015       | 344.75418            | 75               | 2          | 2        | 39              |
            | 5    | IT          | Italy          | 0.739763        | 118607.222222    | 440.195411       | 364.227798           | 75               | 2          | 2        | 39              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "GrandFinal");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 31,
            totalPages: 7);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_min_year(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(
            pageSize: 5,
            minYear: 2023);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.878678        | 153433.333333    | 491.172068       | 355.513557           | 36               | 1          | 1        | 36              |
            | 2    | IT          | Italy       | 0.789397        | 76076            | 322.977639       | 298.38696            | 36               | 1          | 1        | 36              |
            | 3    | IL          | Israel      | 0.781248        | 77696.666667     | 328.840387       | 302.432362           | 36               | 1          | 1        | 36              |
            | 4    | FI          | Finland     | 0.744164        | 113416.666667    | 293.234074       | 519.748871           | 36               | 1          | 1        | 36              |
            | 5    | SI          | Slovenia    | 0.638793        | 12372            | 132.138395       | 146.571939           | 36               | 1          | 1        | 36              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "Any",
            minYear: 2023);
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 26,
            totalPages: 6);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_max_year(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(
            pageSize: 5,
            maxYear: 2022);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 1    | SI          | Slovenia    | 0.967502        | 2809.444444      | 43.47413         | 66.794045            | 18               | 1          | 1        | 18              |
            | 2    | ME          | Montenegro  | 0.950696        | 7942             | 64.142203        | 130.239949           | 20               | 1          | 1        | 20              |
            | 3    | SE          | Sweden      | 0.899748        | 228440.888889    | 577.836386       | 439.387705           | 59               | 2          | 1        | 39              |
            | 4    | ES          | Spain       | 0.827551        | 106534.333333    | 374.450709       | 343.795643           | 39               | 1          | 1        | 39              |
            | 5    | UA          | Ukraine     | 0.809696        | 258875           | 449.012497       | 712.049078           | 57               | 2          | 1        | 39              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
    public async Task Endpoint_should_retrieve_rankings_filtering_by_voting_country_code(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(
            pageSize: 5,
            votingCountryCode: "GB");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 1    | AL          | Albania     | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1               |
            | 1    | AT          | Austria     | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1               |
            | 1    | GE          | Georgia     | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1               |
            | 1    | HR          | Croatia     | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1               |
            | 1    | IE          | Ireland     | 1               | 100              | 1                | 100                  | 1                | 1          | 1        | 1               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "Any",
            votingCountryCode: "GB");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 36,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_multiple_facets_scenario_1(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(
            pageSize: 5,
            minYear: 2016,
            maxYear: 2022,
            contestStage: "SemiFinals");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 1    | SI          | Slovenia    | 0.967502        | 2809.444444      | 43.47413         | 66.794045            | 18               | 1          | 1        | 18              |
            | 2    | SE          | Sweden      | 0.962788        | 135069.444444    | 419.821391       | 334.165628           | 20               | 1          | 1        | 20              |
            | 3    | ME          | Montenegro  | 0.950696        | 7942             | 64.142203        | 130.239949           | 20               | 1          | 1        | 20              |
            | 4    | UA          | Ukraine     | 0.892752        | 104644.444444    | 293.805113       | 398.956973           | 18               | 1          | 1        | 18              |
            | 5    | AU          | Australia   | 0.873658        | 47736.111111     | 330.298686       | 165.424034           | 20               | 1          | 1        | 20              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minYear: 2016,
            maxYear: 2022,
            contestStage: "SemiFinals");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 35,
            totalPages: 7);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_multiple_facets_scenario_2(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any",
            votingCountryCode: "SM");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 36   | FI          | Finland     | 0.281266        | 726              | 25.039968        | 103.082491           | 3                | 3          | 2        | 1               |
            | 35   | MD          | Moldova     | 0.631495        | 658.333333       | 25.019992        | 41.666667            | 2                | 2          | 2        | 1               |
            | 34   | BE          | Belgium     | 0.670902        | 406.555556       | 71.693018        | 8.452482             | 3                | 3          | 2        | 1               |
            | 33   | CH          | Switzerland | 0.786318        | 9.333333         | 8.393119         | 1.414214             | 2                | 2          | 2        | 1               |
            | 32   | RO          | Romania     | 0.789352        | 75               | 1.414214         | 67.185481            | 2                | 2          | 1        | 1               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any",
            votingCountryCode: "SM");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 36,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_empty_rankings_page_when_no_data_matches_filtering(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(
            minYear: 2050,
            maxYear: 2016,
            contestStage: "Any");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_be_empty();
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(pageIndex: -1);

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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(pageSize: 0);

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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(pageSize: 101);

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
    public async Task Endpoint_should_fail_on_votingCountryCode_query_param_value_that_is_not_string_of_2_upper_case_letters(
        string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(
            votingCountryCode: "NOT_A_COUNTRY_CODE");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Invalid voting country code",
            detail: "Query parameter 'votingCountryCode' value must be a string of 2 upper-case letters.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("votingCountryCode", "NOT_A_COUNTRY_CODE");
    }

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetCompetingCountryPointsConsensusRankingsResponse>(apiDriver)
    {
        public void Given_I_want_to_obtain_a_page_of_competing_country_points_consensus_rankings(bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingCountryCode = null,
            int? maxYear = null,
            int? minYear = null,
            string? contestStage = null)
        {
            Dictionary<string, object?> queryParams = new()
            {
                [nameof(descending)] = descending,
                [nameof(pageSize)] = pageSize,
                [nameof(pageIndex)] = pageIndex,
                [nameof(votingCountryCode)] = votingCountryCode,
                [nameof(maxYear)] = maxYear,
                [nameof(minYear)] = minYear,
                [nameof(contestStage)] = contestStage is not null ? Enum.Parse<QueryableContestStage>(contestStage) : null
            };

            Request = ApiDriver.RequestFactory.Rankings.GetCompetingCountryPointsConsensusRankings(queryParams);
        }

        public async Task Then_the_response_rankings_page_should_match(string page)
        {
            (CompetingCountryPointsConsensusRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetingCountryPointsConsensusRanking[] expected = MarkdownParser.ParseTable(page, MapRowToRanking).ToArray();

            await Assert.That(rankings).IsEquivalentTo(expected, CollectionOrdering.Matching);
        }

        public async Task Then_the_response_rankings_page_should_be_empty()
        {
            (CompetingCountryPointsConsensusRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(rankings).IsEmpty();
        }

        public async Task Then_the_response_filtering_metadata_should_match(
            string? votingCountryCode = null,
            int? maxYear = null,
            int? minYear = null,
            string contestStage = "")
        {
            (_, CompetingCountryPointsConsensusFilteringMetadata filtering, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetingCountryPointsConsensusFilteringMetadata expectedFiltering = new()
            {
                ContestStage = Enum.Parse<QueryableContestStage>(contestStage),
                MinYear = minYear,
                MaxYear = maxYear,
                VotingCountryCode = votingCountryCode
            };

            await Assert.That(filtering).IsEqualTo(expectedFiltering);
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

        private static CompetingCountryPointsConsensusRanking MapRowToRanking(Dictionary<string, string> row) => new()
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
            VotingCountries = int.Parse(row["VotingCountries"])
        };
    }
}

using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetitorPointsConsensusRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Rankings;

public sealed class GetCompetitorPointsConsensusRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_minimum_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle      | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|------------------|----------------|-----------------|------------------|------------------|----------------------|------------------|
            | 1    | 2022        | SemiFinal1   | 5                    | SI          | Slovenia    | 17                | LPS              | Disko          | 0.967502        | 2809.444444      | 43.47413         | 66.794045            | 18               |
            | 2    | 2022        | SemiFinal2   | 17                   | SE          | Sweden      | 1                 | Cornelia Jakobs  | Hold Me Closer | 0.962788        | 135069.444444    | 419.821391       | 334.165628           | 20               |
            | 3    | 2022        | SemiFinal2   | 15                   | ME          | Montenegro  | 17                | Vladana          | Breathe        | 0.950696        | 7942             | 64.142203        | 130.239949           | 20               |
            | 4    | 2022        | SemiFinal1   | 6                    | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania       | 0.892752        | 104644.444444    | 293.805113       | 398.956973           | 18               |
            | 5    | 2023        | GrandFinal   | 9                    | SE          | Sweden      | 1                 | Loreen           | Tattoo         | 0.878678        | 153433.333333    | 491.172068       | 355.513557           | 36               |
            | 6    | 2022        | SemiFinal2   | 8                    | AU          | Australia   | 2                 | Sheldon Riley    | Not The Same   | 0.873658        | 47736.111111     | 330.298686       | 165.424034           | 20               |
            | 7    | 2022        | SemiFinal2   | 4                    | AZ          | Azerbaijan  | 10                | Nadir Rustamli   | Fade To Black  | 0.847985        | 802              | 211.481021       | 4.472136             | 20               |
            | 8    | 2022        | GrandFinal   | 10                   | ES          | Spain       | 3                 | Chanel           | SloMo          | 0.827551        | 106534.333333    | 374.450709       | 343.795643           | 39               |
            | 9    | 2022        | SemiFinal2   | 18                   | CZ          | Czechia     | 4                 | We Are Domi      | Lights Off     | 0.825789        | 44766.666667     | 220.170893       | 246.221445           | 20               |
            | 10   | 2022        | GrandFinal   | 20                   | SE          | Sweden      | 4                 | Cornelia Jakobs  | Hold Me Closer | 0.824282        | 93371.444444     | 397.045198       | 285.297895           | 39               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 86,
            totalPages: 9);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_requested_pagination(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings(
            pageIndex: 1,
            pageSize: 3,
            descending: true);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName                   | SongTitle | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|---------------------------|-----------|-----------------|------------------|------------------|----------------------|------------------|
            | 83   | 2022        | SemiFinal2   | 2                    | IL          | Israel      | 13                | Michael Ben David         | I.M       | 0.096989        | 1361.555556      | 114.300287       | 122.818746           | 20               |
            | 82   | 2022        | GrandFinal   | 6                    | FR          | France      | 24                | Alvan & Ahez              | Fulenn    | 0.097509        | 171.666667       | 59.813599        | 29.433541            | 39               |
            | 81   | 2023        | GrandFinal   | 10                   | AL          | Albania     | 22                | Albina & Familja Kelmendi | Duje      | 0.101672        | 1382.333333      | 83.108363        | 163.594349           | 36               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 1,
            pageSize: 3,
            descending: true,
            totalItems: 86,
            totalPages: 29);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_contest_stage(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings(
            pageSize: 5,
            contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName    | FinishingPosition | ActName         | SongTitle      | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs |
            |------|-------------|--------------|----------------------|-------------|----------------|-------------------|-----------------|----------------|-----------------|------------------|------------------|----------------------|------------------|
            | 1    | 2023        | GrandFinal   | 9                    | SE          | Sweden         | 1                 | Loreen          | Tattoo         | 0.878678        | 153433.333333    | 491.172068       | 355.513557           | 36               |
            | 2    | 2022        | GrandFinal   | 10                   | ES          | Spain          | 3                 | Chanel          | SloMo          | 0.827551        | 106534.333333    | 374.450709       | 343.795643           | 39               |
            | 3    | 2022        | GrandFinal   | 20                   | SE          | Sweden         | 4                 | Cornelia Jakobs | Hold Me Closer | 0.824282        | 93371.444444     | 397.045198       | 285.297895           | 39               |
            | 4    | 2022        | GrandFinal   | 22                   | GB          | United Kingdom | 2                 | Sam Ryder       | SPACE MAN      | 0.805365        | 98367.666667     | 427.935743       | 285.417822           | 39               |
            | 5    | 2023        | GrandFinal   | 11                   | IT          | Italy          | 4                 | Marco Mengoni   | Due Vite       | 0.789397        | 76076            | 322.977639       | 298.38696            | 36               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "GrandFinal");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 51,
            totalPages: 11);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_min_year(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings(
            pageSize: 5,
            minYear: 2023);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName       | SongTitle   | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|---------------|-------------|-----------------|------------------|------------------|----------------------|------------------|
            | 1    | 2023        | GrandFinal   | 9                    | SE          | Sweden      | 1                 | Loreen        | Tattoo      | 0.878678        | 153433.333333    | 491.172068       | 355.513557           | 36               |
            | 2    | 2023        | GrandFinal   | 11                   | IT          | Italy       | 4                 | Marco Mengoni | Due Vite    | 0.789397        | 76076            | 322.977639       | 298.38696            | 36               |
            | 3    | 2023        | GrandFinal   | 23                   | IL          | Israel      | 3                 | Noa Kirel     | Unicorn     | 0.781248        | 77696.666667     | 328.840387       | 302.432362           | 36               |
            | 4    | 2023        | GrandFinal   | 13                   | FI          | Finland     | 2                 | Käärijä       | Cha Cha Cha | 0.744164        | 113416.666667    | 293.234074       | 519.748871           | 36               |
            | 5    | 2023        | GrandFinal   | 24                   | SI          | Slovenia    | 21                | Joker Out     | Carpe Diem  | 0.638793        | 12372            | 132.138395       | 146.571939           | 36               |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings(
            pageSize: 5,
            maxYear: 2022);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle      | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|------------------|----------------|-----------------|------------------|------------------|----------------------|------------------|
            | 1    | 2022        | SemiFinal1   | 5                    | SI          | Slovenia    | 17                | LPS              | Disko          | 0.967502        | 2809.444444      | 43.47413         | 66.794045            | 18               |
            | 2    | 2022        | SemiFinal2   | 17                   | SE          | Sweden      | 1                 | Cornelia Jakobs  | Hold Me Closer | 0.962788        | 135069.444444    | 419.821391       | 334.165628           | 20               |
            | 3    | 2022        | SemiFinal2   | 15                   | ME          | Montenegro  | 17                | Vladana          | Breathe        | 0.950696        | 7942.            | 64.142203        | 130.239949           | 20               |
            | 4    | 2022        | SemiFinal1   | 6                    | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania       | 0.892752        | 104644.444444    | 293.805113       | 398.956973           | 18               |
            | 5    | 2022        | SemiFinal2   | 8                    | AU          | Australia   | 2                 | Sheldon Riley    | Not The Same   | 0.873658        | 47736.111111     | 330.298686       | 165.424034           | 20               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "Any",
            maxYear: 2022);
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 60,
            totalPages: 12);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_multiple_facets_scenario_1(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings(
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
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle      | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|------------------|----------------|-----------------|------------------|------------------|----------------------|------------------|
            | 1    | 2022        | SemiFinal1   | 5                    | SI          | Slovenia    | 17                | LPS              | Disko          | 0.967502        | 2809.444444      | 43.47413         | 66.794045            | 18               |
            | 2    | 2022        | SemiFinal2   | 17                   | SE          | Sweden      | 1                 | Cornelia Jakobs  | Hold Me Closer | 0.962788        | 135069.444444    | 419.821391       | 334.165628           | 20               |
            | 3    | 2022        | SemiFinal2   | 15                   | ME          | Montenegro  | 17                | Vladana          | Breathe        | 0.950696        | 7942             | 64.142203        | 130.239949           | 20               |
            | 4    | 2022        | SemiFinal1   | 6                    | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania       | 0.892752        | 104644.444444    | 293.805113       | 398.956973           | 18               |
            | 5    | 2022        | SemiFinal2   | 8                    | AU          | Australia   | 2                 | Sheldon Riley    | Not The Same   | 0.873658        | 47736.111111     | 330.298686       | 165.424034           | 20               |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings(
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName           | SongTitle         | PointsConsensus | VectorDotProduct | JuryVectorLength | TelevoteVectorLength | PointsAwardPairs |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|-------------------|-------------------|-----------------|------------------|------------------|----------------------|------------------|
            | 86   | 2022        | GrandFinal   | 18                   | IS          | Iceland     | 23                | Systur            | Með Hækkandi Sól  | 0.083926        | 314.555556       | 54.329243        | 68.987116            | 39               |
            | 85   | 2022        | SemiFinal2   | 9                    | CY          | Cyprus      | 12                | Andromache        | Ela               | 0.084589        | 644.888889       | 45.065385        | 169.171839           | 20               |
            | 84   | 2022        | GrandFinal   | 1                    | CZ          | Czechia     | 22                | We Are Domi       | Lights Off        | 0.086071        | 343.666667       | 94.796273        | 42.120198            | 39               |
            | 83   | 2022        | SemiFinal2   | 2                    | IL          | Israel      | 13                | Michael Ben David | I.M               | 0.096989        | 1361.555556      | 114.300287       | 122.818746           | 20               |
            | 82   | 2022        | GrandFinal   | 6                    | FR          | France      | 24                | Alvan & Ahez      | Fulenn            | 0.097509        | 171.666667       | 59.813599        | 29.433541            | 39               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 86,
            totalPages: 18);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_empty_rankings_page_when_no_data_matches_filtering(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings(
            minYear: 2023,
            maxYear: 2023,
            contestStage: "SemiFinals");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_be_empty();
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minYear: 2023,
            maxYear: 2023,
            contestStage: "SemiFinals");
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings(pageIndex: -1);

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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings(pageSize: 0);

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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings(pageSize: 101);

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
    public async Task Endpoint_should_fail_on_minYear_query_param_value_greater_than_maxYear_query_param_value(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings(minYear: 2050, maxYear: 2016);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Invalid contest year range",
            detail: "Query parameter 'minYear' integer value must not be greater than query parameter 'maxYear' integer value.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("minYear", 2050);
        await euroFan.Then_the_response_problem_details_extensions_should_include("maxYear", 2016);
    }

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetCompetitorPointsConsensusRankingsResponse>(apiDriver)
    {
        public void Given_I_want_to_obtain_a_page_of_competitor_points_consensus_rankings(bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            int? maxYear = null,
            int? minYear = null,
            string? contestStage = null)
        {
            Dictionary<string, object?> queryParams = new()
            {
                [nameof(descending)] = descending,
                [nameof(pageSize)] = pageSize,
                [nameof(pageIndex)] = pageIndex,
                [nameof(maxYear)] = maxYear,
                [nameof(minYear)] = minYear,
                [nameof(contestStage)] = contestStage is not null ? Enum.Parse<QueryableContestStage>(contestStage) : null
            };

            Request = ApiDriver.RequestFactory.Rankings.GetCompetitorPointsConsensusRankings(queryParams);
        }

        public async Task Then_the_response_rankings_page_should_match(string page)
        {
            (CompetitorPointsConsensusRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetitorPointsConsensusRanking[] expected = MarkdownParser.ParseTable(page, MapRowToRanking).ToArray();

            await Assert.That(rankings).IsEquivalentTo(expected, CollectionOrdering.Matching);
        }

        public async Task Then_the_response_rankings_page_should_be_empty()
        {
            (CompetitorPointsConsensusRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(rankings).IsEmpty();
        }

        public async Task Then_the_response_filtering_metadata_should_match(
            int? maxYear = null,
            int? minYear = null,
            string contestStage = "")
        {
            (_, CompetitorPointsConsensusFilteringMetadata filtering, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetitorPointsConsensusFilteringMetadata expectedFiltering = new()
            {
                ContestStage = Enum.Parse<QueryableContestStage>(contestStage), MinYear = minYear, MaxYear = maxYear
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

        private static CompetitorPointsConsensusRanking MapRowToRanking(Dictionary<string, string> row) => new()
        {
            Rank = int.Parse(row["Rank"]),
            ContestYear = int.Parse(row["ContestYear"]),
            ContestStage = Enum.Parse<ContestStage>(row["ContestStage"]),
            RunningOrderPosition = int.Parse(row["RunningOrderPosition"]),
            CountryCode = row["CountryCode"],
            CountryName = row["CountryName"],
            FinishingPosition = int.Parse(row["FinishingPosition"]),
            ActName = row["ActName"],
            SongTitle = row["SongTitle"],
            PointsConsensus = decimal.Parse(row["PointsConsensus"]),
            VectorDotProduct = decimal.Parse(row["VectorDotProduct"]),
            JuryVectorLength = decimal.Parse(row["JuryVectorLength"]),
            TelevoteVectorLength = decimal.Parse(row["TelevoteVectorLength"]),
            PointsAwardPairs = int.Parse(row["PointsAwardPairs"])
        };
    }
}

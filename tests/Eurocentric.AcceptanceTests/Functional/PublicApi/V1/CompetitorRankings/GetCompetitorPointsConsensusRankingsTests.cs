using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.CompetitorRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.CompetitorRankings;

[Category("public-api")]
public sealed class GetCompetitorPointsConsensusRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle      | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------------|----------------|-----------------|------------------|------------------|----------------------|------------------|
            | 1    | 2022        | SemiFinal2   | 17               | SE          | Sweden      | 1                 | Cornelia Jakobs  | Hold Me Closer | 0.971201        | 20               | 42.197453        | 34.431091            | 1411.0625        |
            | 2    | 2022        | SemiFinal1   | 5                | SI          | Slovenia    | 17                | LPS              | Disko          | 0.96374         | 18               | 6.609652         | 8.124038             | 51.75            |
            | 3    | 2022        | SemiFinal2   | 15               | ME          | Montenegro  | 17                | Vladana          | Breathe        | 0.945558        | 20               | 8.347904         | 13.793114            | 108.875          |
            | 4    | 2022        | GrandFinal   | 13               | DE          | Germany     | 25                | Malik Harris     | Rockstars      | 0.94138         | 39               | 6.244998         | 7.399324             | 43.5             |
            | 5    | 2022        | SemiFinal2   | 8                | AU          | Australia   | 2                 | Sheldon Riley    | Not The Same   | 0.919805        | 20               | 34.017459        | 18.778312            | 587.5625         |
            | 6    | 2022        | SemiFinal1   | 6                | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania       | 0.917676        | 18               | 30.32635         | 40.12792             | 1116.75          |
            | 7    | 2023        | GrandFinal   | 9                | SE          | Sweden      | 1                 | Loreen           | Tattoo         | 0.909555        | 36               | 50.00125         | 37.599867            | 1710             |
            | 8    | 2022        | SemiFinal2   | 4                | AZ          | Azerbaijan  | 10                | Nadir Rustamli   | Fade To Black  | 0.896761        | 20               | 22.940139        | 4.472136             | 92               |
            | 9    | 2022        | SemiFinal1   | 4                | CH          | Switzerland | 9                 | Marius Bear      | Boys Do Cry    | 0.886           | 18               | 24.62849         | 6.96868              | 152.0625         |
            | 10   | 2022        | SemiFinal2   | 18               | CZ          | Czechia     | 4                 | We Are Domi      | Lights Off     | 0.877163        | 20               | 23.782872        | 26.430333            | 551.375          |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 86,
            totalPages: 9
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(pageIndex: 2, pageSize: 3);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle      | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------------|----------------|-----------------|------------------|------------------|----------------------|------------------|
            | 7    | 2023        | GrandFinal   | 9                | SE          | Sweden      | 1                 | Loreen           | Tattoo         | 0.909555        | 36               | 50.00125         | 37.599867            | 1710             |
            | 8    | 2022        | SemiFinal2   | 4                | AZ          | Azerbaijan  | 10                | Nadir Rustamli   | Fade To Black  | 0.896761        | 20               | 22.940139        | 4.472136             | 92               |
            | 9    | 2022        | SemiFinal1   | 4                | CH          | Switzerland | 9                 | Marius Bear      | Boys Do Cry    | 0.886           | 18               | 24.62849         | 6.96868              | 152.0625         |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            pageIndex: 2,
            pageSize: 3,
            descending: false,
            totalItems: 86,
            totalPages: 29
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_3(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(
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
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName     | FinishingPosition | ActName                   | SongTitle      | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct |
            |------|-------------|--------------|------------------|-------------|-----------------|-------------------|---------------------------|----------------|-----------------|------------------|------------------|----------------------|------------------|
            | 86   | 2022        | SemiFinal2   | 2                | IL          | Israel          | 13                | Michael Ben David         | I.M            | 0.409864        | 20               | 13.294736        | 13.511569            | 73.625           |
            | 85   | 2022        | SemiFinal2   | 11               | MK          | North Macedonia | 11                | Andrea                    | Circles        | 0.442202        | 20               | 18.506756        | 12.020815            | 98.375           |
            | 84   | 2022        | SemiFinal1   | 7                | BG          | Bulgaria        | 16                | Intelligent Music Project | Intention      | 0.459991        | 18               | 9.555757         | 11.858541            | 52.125           |
            | 83   | 2022        | SemiFinal1   | 2                | LV          | Latvia          | 14                | Citi Zēni                 | Eat Your Salad | 0.465667        | 18               | 14.25            | 10.624265            | 70.5             |
            | 82   | 2023        | GrandFinal   | 10               | AL          | Albania         | 22                | Albina & Familja Kelmendi | Duje           | 0.482852        | 36               | 10.825318        | 18.222582            | 95.25            |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 86,
            totalPages: 18
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_4(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(
            minYear: 2022,
            maxYear: 2022,
            contestStage: "Any",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName         | SongTitle      | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|-----------------|----------------|-----------------|------------------|------------------|----------------------|------------------|
            | 1    | 2022        | SemiFinal2   | 17               | SE          | Sweden      | 1                 | Cornelia Jakobs | Hold Me Closer | 0.971201        | 20               | 42.197453        | 34.431091            | 1411.0625        |
            | 2    | 2022        | SemiFinal1   | 5                | SI          | Slovenia    | 17                | LPS             | Disko          | 0.96374         | 18               | 6.609652         | 8.124038             | 51.75            |
            | 3    | 2022        | SemiFinal2   | 15               | ME          | Montenegro  | 17                | Vladana         | Breathe        | 0.945558        | 20               | 8.347904         | 13.793114            | 108.875          |
            | 4    | 2022        | GrandFinal   | 13               | DE          | Germany     | 25                | Malik Harris    | Rockstars      | 0.94138         | 39               | 6.244998         | 7.399324             | 43.5             |
            | 5    | 2022        | SemiFinal2   | 8                | AU          | Australia   | 2                 | Sheldon Riley   | Not The Same   | 0.919805        | 20               | 34.017459        | 18.778312            | 587.5625         |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minYear: 2022,
            maxYear: 2022,
            contestStage: "Any",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 60,
            totalPages: 12
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_5(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(
            contestStage: "SemiFinals",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle      | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------------|----------------|-----------------|------------------|------------------|----------------------|------------------|
            | 1    | 2022        | SemiFinal2   | 17               | SE          | Sweden      | 1                 | Cornelia Jakobs  | Hold Me Closer | 0.971201        | 20               | 42.197453        | 34.431091            | 1411.0625        |
            | 2    | 2022        | SemiFinal1   | 5                | SI          | Slovenia    | 17                | LPS              | Disko          | 0.96374         | 18               | 6.609652         | 8.124038             | 51.75            |
            | 3    | 2022        | SemiFinal2   | 15               | ME          | Montenegro  | 17                | Vladana          | Breathe        | 0.945558        | 20               | 8.347904         | 13.793114            | 108.875          |
            | 4    | 2022        | SemiFinal2   | 8                | AU          | Australia   | 2                 | Sheldon Riley    | Not The Same   | 0.919805        | 20               | 34.017459        | 18.778312            | 587.5625         |
            | 5    | 2022        | SemiFinal1   | 6                | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania       | 0.917676        | 18               | 30.32635         | 40.12792             | 1116.75          |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(
            competingCountryCode: "FI",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName    | SongTitle   | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------|-------------|-----------------|------------------|------------------|----------------------|------------------|
            | 1    | 2022        | SemiFinal2   | 1                | FI          | Finland     | 7                 | The Rasmus | Jezebel     | 0.852689        | 20               | 16.406935        | 24.146687            | 337.8125         |
            | 2    | 2023        | GrandFinal   | 13               | FI          | Finland     | 2                 | Käärijä    | Cha Cha Cha | 0.817828        | 36               | 30.941477        | 52.660469            | 1332.5625        |
            | 3    | 2022        | GrandFinal   | 4                | FI          | Finland     | 21                | The Rasmus | Jezebel     | 0.749963        | 39               | 9.585145         | 12.519984            | 90               |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 3,
            totalPages: 1
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_7(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(
            minYear: 2016,
            maxYear: 2025,
            contestStage: "GrandFinal",
            competingCountryCode: "FI",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName    | SongTitle   | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------|-------------|-----------------|------------------|------------------|----------------------|------------------|
            | 1    | 2023        | GrandFinal   | 13               | FI          | Finland     | 2                 | Käärijä    | Cha Cha Cha | 0.817828        | 36               | 30.941477        | 52.660469            | 1332.5625        |
            | 2    | 2022        | GrandFinal   | 4                | FI          | Finland     | 21                | The Rasmus | Jezebel     | 0.749963        | 39               | 9.585145         | 12.519984            | 90               |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minYear: 2016,
            maxYear: 2025,
            contestStage: "GrandFinal",
            competingCountryCode: "FI",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 2,
            totalPages: 1
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_rankings_page_when_no_data_matches_query(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(minYear: 1066, maxYear: 1963);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_metadata_should_match(
            minYear: 1066,
            maxYear: 1963,
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(pageIndex: -1);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(pageSize: 0);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(pageSize: 101);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(minYear: 2023, maxYear: 2022);

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
    public async Task Should_fail_on_illegal_competing_country_code_value(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(competingCountryCode: "!");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal competing country code value",
            detail: "Competing country code value must be a string of 2 upper-case letters."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(
            key: "competingCountryCode",
            value: "!"
        );
    }

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetCompetitorPointsConsensusRankingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_a_page_of_competitor_points_consensus_rankings(
            bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? competingCountryCode = null,
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
                { nameof(competingCountryCode), competingCountryCode },
                { nameof(contestStage), contestStage },
                { nameof(maxYear), maxYear },
                { nameof(minYear), minYear },
            };

            Request = Kernel.Requests.CompetitorRankings.GetCompetitorPointsConsensusRankings(queryParams);
        }

        public async Task Then_the_retrieved_rankings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Rankings).IsEmpty();

        public async Task Then_the_retrieved_rankings_in_order_should_match(string table)
        {
            CompetitorPointsConsensusRanking[] expected = MarkdownParser.ParseTable(table, MapToRanking);

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
            string? contestStage = null,
            string? competingCountryCode = null,
            int? maxYear = null,
            int? minYear = null
        )
        {
            ContestStageFilter? expectedContestStage = contestStage.ToNullableContestStageFilter();

            await Assert
                .That(SuccessResponse?.Data?.Metadata)
                .HasProperty(cp => cp.MinYear, minYear)
                .And.HasProperty(cp => cp.MaxYear, maxYear)
                .And.HasProperty(cp => cp.ContestStage, expectedContestStage)
                .And.HasProperty(cp => cp.CompetingCountryCode, competingCountryCode)
                .And.HasProperty(cp => cp.PageIndex, pageIndex)
                .And.HasProperty(cp => cp.PageSize, pageSize)
                .And.HasProperty(cp => cp.Descending, descending)
                .And.HasProperty(cp => cp.TotalItems, totalItems)
                .And.HasProperty(cp => cp.TotalPages, totalPages);
        }

        private static CompetitorPointsConsensusRanking MapToRanking(Dictionary<string, string> row)
        {
            return new CompetitorPointsConsensusRanking
            {
                Rank = int.Parse(row["Rank"]),
                ContestYear = int.Parse(row["ContestYear"]),
                ContestStage = Enum.Parse<ContestStage>(row["ContestStage"]),
                RunningOrderSpot = int.Parse(row["RunningOrderSpot"]),
                CountryCode = row["CountryCode"],
                CountryName = row["CountryName"],
                FinishingPosition = int.Parse(row["FinishingPosition"]),
                ActName = row["ActName"],
                SongTitle = row["SongTitle"],
                PointsConsensus = decimal.Parse(row["PointsConsensus"]),
                VectorDimensions = int.Parse(row["VectorDimensions"]),
                JuryVectorLength = decimal.Parse(row["JuryVectorLength"]),
                TelevoteVectorLength = decimal.Parse(row["TelevoteVectorLength"]),
                VectorDotProduct = decimal.Parse(row["VectorDotProduct"]),
            };
        }

        private sealed class RankingEqualityComparer : IEqualityComparer<CompetitorPointsConsensusRanking>
        {
            public bool Equals(CompetitorPointsConsensusRanking? x, CompetitorPointsConsensusRanking? y)
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
                    && x.ContestYear == y.ContestYear
                    && x.ContestStage == y.ContestStage
                    && x.RunningOrderSpot == y.RunningOrderSpot
                    && x.CountryCode.Equals(y.CountryCode, StringComparison.Ordinal)
                    && x.CountryName.Equals(y.CountryName, StringComparison.Ordinal)
                    && x.FinishingPosition == y.FinishingPosition
                    && x.ActName.Equals(y.ActName, StringComparison.Ordinal)
                    && x.SongTitle.Equals(y.SongTitle, StringComparison.Ordinal)
                    && x.PointsConsensus == y.PointsConsensus
                    && x.VectorDimensions == y.VectorDimensions
                    && x.JuryVectorLength == y.JuryVectorLength
                    && x.TelevoteVectorLength == y.TelevoteVectorLength
                    && x.VectorDotProduct == y.VectorDotProduct;
            }

            public int GetHashCode(CompetitorPointsConsensusRanking obj)
            {
                HashCode hashCode = new();
                hashCode.Add(obj.Rank);
                hashCode.Add(obj.ContestYear);
                hashCode.Add((int)obj.ContestStage);
                hashCode.Add(obj.RunningOrderSpot);
                hashCode.Add(obj.CountryCode);
                hashCode.Add(obj.CountryName);
                hashCode.Add(obj.FinishingPosition);
                hashCode.Add(obj.ActName);
                hashCode.Add(obj.SongTitle);
                hashCode.Add(obj.PointsConsensus);
                hashCode.Add(obj.VectorDimensions);
                hashCode.Add(obj.JuryVectorLength);
                hashCode.Add(obj.TelevoteVectorLength);
                hashCode.Add(obj.VectorDotProduct);

                return hashCode.ToHashCode();
            }
        }
    }
}

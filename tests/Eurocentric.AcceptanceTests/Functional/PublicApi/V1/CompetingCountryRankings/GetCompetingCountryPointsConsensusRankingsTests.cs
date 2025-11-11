using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.CompetingCountryRankings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.CompetingCountryRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.CompetingCountryRankings;

[Category("public-api")]
public sealed class GetCompetingCountryPointsConsensusRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 1    | ME          | Montenegro     | 0.945558        | 20               | 8.347904         | 13.793114            | 108.875          | 1          | 1        | 20              |
            | 2    | SE          | Sweden         | 0.918081        | 95               | 77.353895        | 59.728553            | 4241.75          | 3          | 2        | 39              |
            | 3    | MT          | Malta          | 0.860147        | 20               | 11.63239         | 8.838835             | 88.4375          | 1          | 1        | 20              |
            | 4    | GB          | United Kingdom | 0.855805        | 75               | 45.11236         | 32.339991            | 1248.5625        | 2          | 2        | 39              |
            | 5    | UA          | Ukraine        | 0.838269        | 93               | 50.291028        | 79.501179            | 3351.5625        | 3          | 2        | 39              |
            | 6    | ES          | Spain          | 0.822196        | 75               | 44.85393         | 37.20131             | 1371.9375        | 2          | 2        | 39              |
            | 7    | DE          | Germany        | 0.819231        | 75               | 9.072624         | 12.512494            | 93               | 2          | 2        | 39              |
            | 8    | FI          | Finland        | 0.817975        | 95               | 36.310295        | 59.270039            | 1760.375         | 3          | 2        | 39              |
            | 9    | IT          | Italy          | 0.811229        | 75               | 46.318193        | 39.690994            | 1491.375         | 2          | 2        | 39              |
            | 10   | SI          | Slovenia       | 0.80128         | 54               | 16.443084        | 18.528694            | 244.125          | 2          | 2        | 37              |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(
            pageIndex: 2,
            pageSize: 3
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 7    | DE          | Germany        | 0.819231        | 75               | 9.072624         | 12.512494            | 93               | 2          | 2        | 39              |
            | 8    | FI          | Finland        | 0.817975        | 95               | 36.310295        | 59.270039            | 1760.375         | 3          | 2        | 39              |
            | 9    | IT          | Italy          | 0.811229        | 75               | 46.318193        | 39.690994            | 1491.375         | 2          | 2        | 39              |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(
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
            | Rank | CountryCode | CountryName     | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | VotingCountries |
            |------|-------------|-----------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 40   | MK          | North Macedonia | 0.442202        | 20               | 18.506756        | 12.020815            | 98.375           | 1          | 1        | 20              |
            | 39   | BG          | Bulgaria        | 0.459991        | 18               | 9.555757         | 11.858541            | 52.125           | 1          | 1        | 18              |
            | 38   | LV          | Latvia          | 0.465667        | 18               | 14.25            | 10.624265            | 70.5             | 1          | 1        | 18              |
            | 37   | SM          | San Marino      | 0.512014        | 20               | 12.290749        | 11.997396            | 75.5             | 1          | 1        | 20              |
            | 36   | AT          | Austria         | 0.541175        | 54               | 25.865034        | 15.672428            | 219.375          | 2          | 2        | 37              |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(
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
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 1    | SI          | Slovenia    | 0.96374         | 18               | 6.609652         | 8.124038             | 51.75            | 1          | 1        | 18              |
            | 2    | ME          | Montenegro  | 0.945558        | 20               | 8.347904         | 13.793114            | 108.875          | 1          | 1        | 20              |
            | 3    | DE          | Germany     | 0.94138         | 39               | 6.244998         | 7.399324             | 43.5             | 1          | 1        | 39              |
            | 4    | SE          | Sweden      | 0.924305        | 59               | 59.021183        | 46.408512            | 2531.75          | 2          | 1        | 39              |
            | 5    | ES          | Spain       | 0.872942        | 39               | 39.002404        | 36.583808            | 1245.5625        | 1          | 1        | 39              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minYear: 2022,
            maxYear: 2022,
            contestStage: "Any",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(
            contestStage: "SemiFinals",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.971201        | 20               | 42.197453        | 34.431091            | 1411.0625        | 1          | 1        | 20              |
            | 2    | SI          | Slovenia    | 0.96374         | 18               | 6.609652         | 8.124038             | 51.75            | 1          | 1        | 18              |
            | 3    | ME          | Montenegro  | 0.945558        | 20               | 8.347904         | 13.793114            | 108.875          | 1          | 1        | 20              |
            | 4    | AU          | Australia   | 0.919805        | 20               | 34.017459        | 18.778312            | 587.5625         | 1          | 1        | 20              |
            | 5    | UA          | Ukraine     | 0.917676        | 18               | 30.32635         | 40.12792             | 1116.75          | 1          | 1        | 18              |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(
            votingCountryCode: "GB",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 1    | AL          | Albania     | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1               |
            | 1    | AT          | Austria     | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1               |
            | 1    | DE          | Germany     | 1               | 2                | 1.414214         | 1.414214             | 2                | 2          | 2        | 1               |
            | 1    | FR          | France      | 1               | 2                | 1.414214         | 1.414214             | 2                | 2          | 2        | 1               |
            | 1    | GE          | Georgia     | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1               |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            votingCountryCode: "GB",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 36,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_7(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(
            minYear: 2023,
            contestStage: "Any",
            votingCountryCode: "GB",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|-----------------|
            | 1    | AL          | Albania     | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1               |
            | 1    | AM          | Armenia     | 1               | 1                | 3.25             | 1                    | 3.25             | 1          | 1        | 1               |
            | 1    | AT          | Austria     | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1               |
            | 1    | AU          | Australia   | 1               | 1                | 8.5              | 2.5                  | 21.25            | 1          | 1        | 1               |
            | 1    | BE          | Belgium     | 1               | 1                | 6.25             | 5.5                  | 34.375           | 1          | 1        | 1               |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minYear: 2023,
            contestStage: "Any",
            votingCountryCode: "GB",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 25,
            totalPages: 5
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_rankings_page_when_no_data_matches_query(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(pageIndex: -1);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(pageSize: 0);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(pageSize: 101);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(votingCountryCode: "!");

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

    private sealed class EuroFan(EuroFanKernel kernel)
        : EuroFanActor<GetCompetingCountryPointsConsensusRankingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_a_page_of_competing_country_points_consensus_rankings(
            bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
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
                { nameof(votingCountryCode), votingCountryCode },
                { nameof(contestStage), contestStage },
                { nameof(maxYear), maxYear },
                { nameof(minYear), minYear },
            };

            Request = Kernel.Requests.CompetingCountryRankings.GetCompetingCountryPointsConsensusRankings(queryParams);
        }

        public async Task Then_the_retrieved_rankings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Rankings).IsEmpty();

        public async Task Then_the_retrieved_rankings_in_order_should_match(string table)
        {
            CompetingCountryPointsConsensusRanking[] expected = MarkdownParser.ParseTable(table, MapToRanking);

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
            string? votingCountryCode = null,
            string? contestStage = null,
            int? maxYear = null,
            int? minYear = null
        )
        {
            ContestStageFilter? expectedContestStage = contestStage.ToNullableContestStageFilter();

            await Assert
                .That(SuccessResponse?.Data?.Metadata)
                .HasProperty(cc => cc.MinYear, minYear)
                .And.HasProperty(cc => cc.MaxYear, maxYear)
                .And.HasProperty(cc => cc.ContestStage, expectedContestStage)
                .And.HasProperty(cc => cc.VotingCountryCode, votingCountryCode)
                .And.HasProperty(cc => cc.PageIndex, pageIndex)
                .And.HasProperty(cc => cc.PageSize, pageSize)
                .And.HasProperty(cc => cc.Descending, descending)
                .And.HasProperty(cc => cc.TotalItems, totalItems)
                .And.HasProperty(cc => cc.TotalPages, totalPages);
        }

        private static CompetingCountryPointsConsensusRanking MapToRanking(Dictionary<string, string> row)
        {
            return new CompetingCountryPointsConsensusRanking
            {
                Rank = int.Parse(row["Rank"]),
                CountryCode = row["CountryCode"],
                CountryName = row["CountryName"],
                PointsConsensus = decimal.Parse(row["PointsConsensus"]),
                VectorDimensions = int.Parse(row["VectorDimensions"]),
                JuryVectorLength = decimal.Parse(row["JuryVectorLength"]),
                TelevoteVectorLength = decimal.Parse(row["TelevoteVectorLength"]),
                VectorDotProduct = decimal.Parse(row["VectorDotProduct"]),
                Broadcasts = int.Parse(row["Broadcasts"]),
                Contests = int.Parse(row["Contests"]),
                VotingCountries = int.Parse(row["VotingCountries"]),
            };
        }

        private sealed class RankingEqualityComparer : IEqualityComparer<CompetingCountryPointsConsensusRanking>
        {
            public bool Equals(CompetingCountryPointsConsensusRanking? x, CompetingCountryPointsConsensusRanking? y)
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
                    && x.PointsConsensus == y.PointsConsensus
                    && x.VectorDimensions == y.VectorDimensions
                    && x.JuryVectorLength == y.JuryVectorLength
                    && x.TelevoteVectorLength == y.TelevoteVectorLength
                    && x.VectorDotProduct == y.VectorDotProduct
                    && x.Broadcasts == y.Broadcasts
                    && x.Contests == y.Contests
                    && x.VotingCountries == y.VotingCountries;
            }

            public int GetHashCode(CompetingCountryPointsConsensusRanking obj)
            {
                HashCode hashCode = new();
                hashCode.Add(obj.Rank);
                hashCode.Add(obj.CountryCode);
                hashCode.Add(obj.CountryName);
                hashCode.Add(obj.PointsConsensus);
                hashCode.Add(obj.VectorDimensions);
                hashCode.Add(obj.JuryVectorLength);
                hashCode.Add(obj.TelevoteVectorLength);
                hashCode.Add(obj.VectorDotProduct);
                hashCode.Add(obj.Broadcasts);
                hashCode.Add(obj.Contests);
                hashCode.Add(obj.VotingCountries);

                return hashCode.ToHashCode();
            }
        }
    }
}

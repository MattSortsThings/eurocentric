using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.VotingCountryRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.VotingCountryRankings;

[Category("public-api")]
public sealed class GetVotingCountryPointsConsensusRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName     | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-----------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 1    | IS          | Iceland         | 0.83795         | 66               | 32.908965        | 32.908965            | 907.5            | 3          | 2        | 33                 |
            | 2    | AM          | Armenia         | 0.814406        | 65               | 32.893768        | 32.893768            | 881.1875         | 3          | 2        | 33                 |
            | 3    | ME          | Montenegro      | 0.805469        | 42               | 26.832816        | 26.832816            | 579.9375         | 2          | 1        | 32                 |
            | 4    | IL          | Israel          | 0.802295        | 67               | 32.924155        | 32.924155            | 869.6875         | 3          | 2        | 36                 |
            | 5    | MK          | North Macedonia | 0.797656        | 42               | 26.832816        | 26.832816            | 574.3125         | 2          | 1        | 32                 |
            | 6    | EE          | Estonia         | 0.781337        | 66               | 32.908965        | 32.908965            | 846.1875         | 3          | 2        | 36                 |
            | 7    | MT          | Malta           | 0.777074        | 68               | 32.939338        | 32.939338            | 843.125          | 3          | 2        | 36                 |
            | 8    | NO          | Norway          | 0.771777        | 65               | 32.893768        | 32.893768            | 835.0625         | 3          | 2        | 33                 |
            | 9    | FR          | France          | 0.754328        | 66               | 32.908965        | 32.908965            | 816.9375         | 3          | 2        | 33                 |
            | 10   | AZ          | Azerbaijan      | 0.750404        | 67               | 32.924155        | 32.924155            | 813.4375         | 3          | 2        | 36                 |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(pageIndex: 2, pageSize: 3);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName     | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-----------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 7    | MT          | Malta           | 0.777074        | 68               | 32.939338        | 32.939338            | 843.125          | 3          | 2        | 36                 |
            | 8    | NO          | Norway          | 0.771777        | 65               | 32.893768        | 32.893768            | 835.0625         | 3          | 2        | 33                 |
            | 9    | FR          | France          | 0.754328        | 66               | 32.908965        | 32.908965            | 816.9375         | 3          | 2        | 33                 |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(
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
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 40   | CH          | Switzerland | 0.547193        | 65               | 32.893768        | 32.893768            | 592.0625         | 3          | 2        | 33                 |
            | 39   | UA          | Ukraine     | 0.551352        | 65               | 32.893768        | 32.893768            | 596.5625         | 3          | 2        | 33                 |
            | 38   | DE          | Germany     | 0.55685         | 67               | 32.924155        | 32.924155            | 603.625          | 3          | 2        | 36                 |
            | 37   | GR          | Greece      | 0.581891        | 66               | 32.908965        | 32.908965            | 630.1875         | 3          | 2        | 33                 |
            | 36   | ES          | Spain       | 0.600957        | 67               | 32.924155        | 32.924155            | 651.4375         | 3          | 2        | 36                 |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(
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
            | Rank | CountryCode | CountryName     | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-----------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 1    | IS          | Iceland         | 0.880919        | 40               | 26.795522        | 26.795522            | 632.5            | 2          | 1        | 31                 |
            | 2    | IL          | Israel          | 0.811719        | 42               | 26.832816        | 26.832816            | 584.4375         | 2          | 1        | 32                 |
            | 3    | LT          | Lithuania       | 0.808061        | 40               | 26.795522        | 26.795522            | 580.1875         | 2          | 1        | 31                 |
            | 4    | ME          | Montenegro      | 0.805469        | 42               | 26.832816        | 26.832816            | 579.9375         | 2          | 1        | 32                 |
            | 5    | MK          | North Macedonia | 0.797656        | 42               | 26.832816        | 26.832816            | 574.3125         | 2          | 1        | 32                 |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(
            contestStage: "SemiFinals",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 1    | IS          | Iceland     | 0.873239        | 16               | 18.841444        | 18.841444            | 310              | 1          | 1        | 16                 |
            | 2    | EE          | Estonia     | 0.872015        | 17               | 18.867962        | 18.867962            | 310.4375         | 1          | 1        | 17                 |
            | 3    | FI          | Finland     | 0.859375        | 17               | 18.867962        | 18.867962            | 305.9375         | 1          | 1        | 17                 |
            | 4    | IL          | Israel      | 0.853055        | 17               | 18.867962        | 18.867962            | 303.6875         | 1          | 1        | 17                 |
            | 5    | NO          | Norway      | 0.827289        | 16               | 18.841444        | 18.841444            | 293.6875         | 1          | 1        | 16                 |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestStage: "SemiFinals",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_6(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(
            competingCountryCode: "FI",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 1    | BG          | Bulgaria    | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1                  |
            | 1    | NO          | Norway      | 1               | 2                | 10.049876        | 10.049876            | 101              | 2          | 2        | 1                  |
            | 3    | IS          | Iceland     | 0.999848        | 2                | 8.558621         | 10.049876            | 86               | 2          | 2        | 1                  |
            | 3    | NL          | Netherlands | 0.999848        | 2                | 8.558621         | 10.049876            | 86               | 2          | 2        | 1                  |
            | 5    | AM          | Armenia     | 0.99928         | 2                | 7.071068         | 5.59017              | 39.5             | 2          | 2        | 1                  |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 39,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_7(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(
            contestStage: "SemiFinal2",
            competingCountryCode: "SM",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsConsensus | VectorDimensions | JuryVectorLength | TelevoteVectorLength | VectorDotProduct | Broadcasts | Contests | CompetingCountries |
            |------|-------------|-------------|-----------------|------------------|------------------|----------------------|------------------|------------|----------|--------------------|
            | 1    | AU          | Australia   | 1               | 1                | 1                | 4.75                 | 4.75             | 1          | 1        | 1                  |
            | 1    | AZ          | Azerbaijan  | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1                  |
            | 1    | BE          | Belgium     | 1               | 1                | 10               | 1                    | 10               | 1          | 1        | 1                  |
            | 1    | CY          | Cyprus      | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1                  |
            | 1    | CZ          | Czechia     | 1               | 1                | 1                | 1                    | 1                | 1          | 1        | 1                  |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestStage: "SemiFinal2",
            competingCountryCode: "SM",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 20,
            totalPages: 4
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_rankings_page_when_no_data_matches_query(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(
            minYear: 1066,
            maxYear: 1963,
            competingCountryCode: "ZZ"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_metadata_should_match(
            minYear: 1066,
            maxYear: 1963,
            competingCountryCode: "ZZ",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(pageIndex: -1);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(pageSize: 0);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(pageSize: 101);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(
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
    public async Task Should_fail_on_illegal_competing_country_code_value(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(competingCountryCode: "!");

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

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetVotingCountryPointsConsensusRankingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_a_page_of_voting_country_points_consensus_rankings(
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

            Request = Kernel.Requests.VotingCountryRankings.GetVotingCountryPointsConsensusRankings(queryParams);
        }

        public async Task Then_the_retrieved_rankings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Rankings).IsEmpty();

        public async Task Then_the_retrieved_rankings_in_order_should_match(string table)
        {
            VotingCountryPointsConsensusRanking[] expected = MarkdownParser.ParseTable(table, MapToRanking);

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
            string? competingCountryCode = null,
            string? contestStage = null,
            int? maxYear = null,
            int? minYear = null
        )
        {
            ContestStageFilter? expectedContestStage = contestStage.ToNullableContestStageFilter();

            await Assert
                .That(SuccessResponse?.Data?.Metadata)
                .HasProperty(vc => vc.MinYear, minYear)
                .And.HasProperty(vc => vc.MaxYear, maxYear)
                .And.HasProperty(vc => vc.ContestStage, expectedContestStage)
                .And.HasProperty(vc => vc.CompetingCountryCode, competingCountryCode)
                .And.HasProperty(vc => vc.PageIndex, pageIndex)
                .And.HasProperty(vc => vc.PageSize, pageSize)
                .And.HasProperty(vc => vc.Descending, descending)
                .And.HasProperty(vc => vc.TotalItems, totalItems)
                .And.HasProperty(vc => vc.TotalPages, totalPages);
        }

        private static VotingCountryPointsConsensusRanking MapToRanking(Dictionary<string, string> row)
        {
            return new VotingCountryPointsConsensusRanking
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
                CompetingCountries = int.Parse(row["CompetingCountries"]),
            };
        }

        private sealed class RankingEqualityComparer : IEqualityComparer<VotingCountryPointsConsensusRanking>
        {
            public bool Equals(VotingCountryPointsConsensusRanking? x, VotingCountryPointsConsensusRanking? y)
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
                    && x.CompetingCountries == y.CompetingCountries;
            }

            public int GetHashCode(VotingCountryPointsConsensusRanking obj)
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
                hashCode.Add(obj.CompetingCountries);

                return hashCode.ToHashCode();
            }
        }
    }
}

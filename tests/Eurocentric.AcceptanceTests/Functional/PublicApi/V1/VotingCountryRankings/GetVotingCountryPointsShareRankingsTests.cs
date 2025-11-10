using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.VotingCountryRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.VotingCountryRankings;

[Category("public-api")]
public sealed class GetVotingCountryPointsShareRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(competingCountryCode: "FI");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName       | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|-------------|-------------|-----------------|--------------|------------|----------|
            | 1    | XX          | Rest of the World | 0.833333    | 20          | 24              | 2            | 2          | 1        |
            | 2    | EE          | Estonia           | 0.763889    | 55          | 72              | 6            | 3          | 2        |
            | 3    | SE          | Sweden            | 0.702381    | 59          | 84              | 7            | 4          | 2        |
            | 4    | NO          | Norway            | 0.6         | 36          | 60              | 5            | 3          | 2        |
            | 5    | NL          | Netherlands       | 0.533333    | 32          | 60              | 5            | 3          | 2        |
            | 6    | RS          | Serbia            | 0.488095    | 41          | 84              | 7            | 4          | 2        |
            | 7    | HR          | Croatia           | 0.483333    | 29          | 60              | 5            | 3          | 2        |
            | 8    | IS          | Iceland           | 0.458333    | 22          | 48              | 4            | 2          | 2        |
            | 9    | IL          | Israel            | 0.440476    | 37          | 84              | 7            | 4          | 2        |
            | 10   | CZ          | Czechia           | 0.428571    | 36          | 84              | 7            | 4          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            competingCountryCode: "FI",
            pageIndex: 2,
            pageSize: 3
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName       | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|-------------|-------------|-----------------|--------------|------------|----------|
            | 7    | HR          | Croatia           | 0.483333    | 29          | 60              | 5            | 3          | 2        |
            | 8    | IS          | Iceland           | 0.458333    | 22          | 48              | 4            | 2          | 2        |
            | 9    | IL          | Israel            | 0.440476    | 37          | 84              | 7            | 4          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            competingCountryCode: "FI",
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
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|
            | 40   | BG          | Bulgaria    | 0           | 0           | 24              | 2            | 1          | 1        |
            | 39   | ME          | Montenegro  | 0.083333    | 4           | 48              | 4            | 2          | 1        |
            | 38   | AL          | Albania     | 0.166667    | 8           | 48              | 4            | 2          | 2        |
            | 37   | CY          | Cyprus      | 0.194444    | 14          | 72              | 6            | 3          | 2        |
            | 35   | GR          | Greece      | 0.208333    | 10          | 48              | 4            | 2          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            competingCountryCode: "FI",
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
            | Rank | CountryCode | CountryName     | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-----------------|-------------|-------------|-----------------|--------------|------------|----------|
            | 1    | EE          | Estonia         | 0.6875      | 33          | 48              | 4            | 2          | 1        |
            | 2    | SE          | Sweden          | 0.479167    | 23          | 48              | 4            | 2          | 1        |
            | 3    | CZ          | Czechia         | 0.270833    | 13          | 48              | 4            | 2          | 1        |
            | 4    | MK          | North Macedonia | 0.25        | 12          | 48              | 4            | 2          | 1        |
            | 4    | RS          | Serbia          | 0.25        | 12          | 48              | 4            | 2          | 1        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
            minYear: 2022,
            maxYear: 2022,
            contestStage: "Any",
            votingMethod: "Any",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 39,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_5(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            competingCountryCode: "FI",
            contestStage: "SemiFinals",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|
            | 1    | HR          | Croatia     | 1           | 12          | 12              | 1            | 1          | 1        |
            | 1    | LV          | Latvia      | 1           | 12          | 12              | 1            | 1          | 1        |
            | 1    | NO          | Norway      | 1           | 12          | 12              | 1            | 1          | 1        |
            | 4    | CH          | Switzerland | 0.833333    | 10          | 12              | 1            | 1          | 1        |
            | 4    | EE          | Estonia     | 0.833333    | 20          | 24              | 2            | 1          | 1        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
            contestStage: "SemiFinals",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 30,
            totalPages: 6
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_6(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            competingCountryCode: "FI",
            votingMethod: "Jury",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|
            | 1    | EE          | Estonia     | 0.638889    | 23          | 36              | 3            | 3          | 2        |
            | 2    | NO          | Norway      | 0.5         | 12          | 24              | 2            | 2          | 2        |
            | 2    | RS          | Serbia      | 0.5         | 18          | 36              | 3            | 3          | 2        |
            | 4    | SE          | Sweden      | 0.444444    | 16          | 36              | 3            | 3          | 2        |
            | 5    | IS          | Iceland     | 0.416667    | 10          | 24              | 2            | 2          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
            votingMethod: "Jury",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            competingCountryCode: "FI",
            votingMethod: "Televote",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName       | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|-------------|-------------|-----------------|--------------|------------|----------|
            | 1    | SE          | Sweden            | 0.895833    | 43          | 48              | 4            | 4          | 2        |
            | 2    | EE          | Estonia           | 0.888889    | 32          | 36              | 3            | 3          | 2        |
            | 3    | XX          | Rest of the World | 0.833333    | 20          | 24              | 2            | 2          | 1        |
            | 4    | LV          | Latvia            | 0.694444    | 25          | 36              | 3            | 3          | 2        |
            | 5    | NO          | Norway            | 0.666667    | 24          | 36              | 3            | 3          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            competingCountryCode: "SM",
            contestStage: "SemiFinal2",
            votingMethod: "Jury",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|
            | 1    | BE          | Belgium     | 1           | 12          | 12              | 1            | 1          | 1        |
            | 2    | FI          | Finland     | 0.416667    | 5           | 12              | 1            | 1          | 1        |
            | 3    | PL          | Poland      | 0.166667    | 2           | 12              | 1            | 1          | 1        |
            | 3    | RO          | Romania     | 0.166667    | 2           | 12              | 1            | 1          | 1        |
            | 5    | AU          | Australia   | 0           | 0           | 12              | 1            | 1          | 1        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "SM",
            contestStage: "SemiFinal2",
            votingMethod: "Jury",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            competingCountryCode: "ZZ",
            minYear: 1066,
            maxYear: 1963
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "ZZ",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            pageIndex: -1,
            competingCountryCode: "FI"
        );

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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            pageSize: 0,
            competingCountryCode: "FI"
        );

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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            pageSize: 101,
            competingCountryCode: "FI"
        );

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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            minYear: 2023,
            maxYear: 2022,
            competingCountryCode: "FI"
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(competingCountryCode: "!");

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

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetVotingCountryPointsShareRankingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_a_page_of_voting_country_points_share_rankings(
            bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingMethod = null,
            string competingCountryCode = "",
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
                { nameof(competingCountryCode), competingCountryCode },
                { nameof(contestStage), contestStage },
                { nameof(maxYear), maxYear },
                { nameof(minYear), minYear },
            };

            Request = Kernel.Requests.VotingCountryRankings.GetVotingCountryPointsShareRankings(queryParams);
        }

        public async Task Then_the_retrieved_rankings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Rankings).IsEmpty();

        public async Task Then_the_retrieved_rankings_in_order_should_match(string table)
        {
            VotingCountryPointsShareRanking[] expected = MarkdownParser.ParseTable(table, MapToRanking);

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
            string competingCountryCode = "",
            string? contestStage = null,
            int? maxYear = null,
            int? minYear = null
        )
        {
            ContestStageFilter? expectedContestStage = contestStage.ToNullableContestStageFilter();
            VotingMethodFilter? expectedVotingMethod = votingMethod.ToNullableVotingMethodFilter();

            await Assert
                .That(SuccessResponse?.Data?.Metadata)
                .HasProperty(vc => vc.MinYear, minYear)
                .And.HasProperty(vc => vc.MaxYear, maxYear)
                .And.HasProperty(vc => vc.ContestStage, expectedContestStage)
                .And.HasProperty(vc => vc.CompetingCountryCode, competingCountryCode)
                .And.HasProperty(vc => vc.VotingMethod, expectedVotingMethod)
                .And.HasProperty(vc => vc.PageIndex, pageIndex)
                .And.HasProperty(vc => vc.PageSize, pageSize)
                .And.HasProperty(vc => vc.Descending, descending)
                .And.HasProperty(vc => vc.TotalItems, totalItems)
                .And.HasProperty(vc => vc.TotalPages, totalPages);
        }

        private static VotingCountryPointsShareRanking MapToRanking(Dictionary<string, string> row)
        {
            return new VotingCountryPointsShareRanking
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
            };
        }

        private sealed class RankingEqualityComparer : IEqualityComparer<VotingCountryPointsShareRanking>
        {
            public bool Equals(VotingCountryPointsShareRanking? x, VotingCountryPointsShareRanking? y)
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
                    && x.Contests == y.Contests;
            }

            public int GetHashCode(VotingCountryPointsShareRanking obj)
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

                return hashCode.ToHashCode();
            }
        }
    }
}

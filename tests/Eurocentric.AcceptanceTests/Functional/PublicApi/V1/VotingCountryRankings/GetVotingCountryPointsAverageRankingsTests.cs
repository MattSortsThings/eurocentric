using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.VotingCountryRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.VotingCountryRankings;

[Category("public-api")]
public sealed class GetVotingCountryPointsAverageRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(competingCountryCode: "FI");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName       | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|-------------|--------------|------------|----------|
            | 1    | XX          | Rest of the World | 10            | 20          | 2            | 2          | 1        |
            | 2    | EE          | Estonia           | 9.166667      | 55          | 6            | 3          | 2        |
            | 3    | SE          | Sweden            | 8.428571      | 59          | 7            | 4          | 2        |
            | 4    | NO          | Norway            | 7.2           | 36          | 5            | 3          | 2        |
            | 5    | NL          | Netherlands       | 6.4           | 32          | 5            | 3          | 2        |
            | 6    | RS          | Serbia            | 5.857143      | 41          | 7            | 4          | 2        |
            | 7    | HR          | Croatia           | 5.8           | 29          | 5            | 3          | 2        |
            | 8    | IS          | Iceland           | 5.5           | 22          | 4            | 2          | 2        |
            | 9    | IL          | Israel            | 5.285714      | 37          | 7            | 4          | 2        |
            | 10   | CZ          | Czechia           | 5.142857      | 36          | 7            | 4          | 2        |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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
            | Rank | CountryCode | CountryName       | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|-------------|--------------|------------|----------|
            | 7    | HR          | Croatia           | 5.8           | 29          | 5            | 3          | 2        |
            | 8    | IS          | Iceland           | 5.5           | 22          | 4            | 2          | 2        |
            | 9    | IL          | Israel            | 5.285714      | 37          | 7            | 4          | 2        |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|
            | 40   | BG          | Bulgaria    | 0             | 0           | 2            | 1          | 1        |
            | 39   | ME          | Montenegro  | 1             | 4           | 4            | 2          | 1        |
            | 38   | AL          | Albania     | 2             | 8           | 4            | 2          | 2        |
            | 37   | CY          | Cyprus      | 2.333333      | 14          | 6            | 3          | 2        |
            | 35   | GR          | Greece      | 2.5           | 10          | 4            | 2          | 2        |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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
            | Rank | CountryCode | CountryName     | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-----------------|---------------|-------------|--------------|------------|----------|
            | 1    | EE          | Estonia         | 8.25          | 33          | 4            | 2          | 1        |
            | 2    | SE          | Sweden          | 5.75          | 23          | 4            | 2          | 1        |
            | 3    | CZ          | Czechia         | 3.25          | 13          | 4            | 2          | 1        |
            | 4    | MK          | North Macedonia | 3             | 12          | 4            | 2          | 1        |
            | 4    | RS          | Serbia          | 3             | 12          | 4            | 2          | 1        |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|
            | 1    | HR          | Croatia     | 12            | 12          | 1            | 1          | 1        |
            | 1    | LV          | Latvia      | 12            | 12          | 1            | 1          | 1        |
            | 1    | NO          | Norway      | 12            | 12          | 1            | 1          | 1        |
            | 4    | CH          | Switzerland | 10            | 10          | 1            | 1          | 1        |
            | 4    | EE          | Estonia     | 10            | 20          | 2            | 1          | 1        |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|
            | 1    | EE          | Estonia     | 7.666667      | 23          | 3            | 3          | 2        |
            | 2    | NO          | Norway      | 6             | 12          | 2            | 2          | 2        |
            | 2    | RS          | Serbia      | 6             | 18          | 3            | 3          | 2        |
            | 4    | SE          | Sweden      | 5.333333      | 16          | 3            | 3          | 2        |
            | 5    | IS          | Iceland     | 5             | 10          | 2            | 2          | 2        |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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
            | Rank | CountryCode | CountryName       | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|-------------|--------------|------------|----------|
            | 1    | SE          | Sweden            | 10.75         | 43          | 4            | 4          | 2        |
            | 2    | EE          | Estonia           | 10.666667     | 32          | 3            | 3          | 2        |
            | 3    | XX          | Rest of the World | 10            | 20          | 2            | 2          | 1        |
            | 4    | LV          | Latvia            | 8.333333      | 25          | 3            | 3          | 2        |
            | 5    | NO          | Norway            | 8             | 24          | 3            | 3          | 2        |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|
            | 1    | BE          | Belgium     | 12            | 12          | 1            | 1          | 1        |
            | 2    | FI          | Finland     | 5             | 5           | 1            | 1          | 1        |
            | 3    | PL          | Poland      | 2             | 2           | 1            | 1          | 1        |
            | 3    | RO          | Romania     | 2             | 2           | 1            | 1          | 1        |
            | 5    | AU          | Australia   | 0             | 0           | 1            | 1          | 1        |
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(competingCountryCode: "!");

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

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetVotingCountryPointsAverageRankingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_a_page_of_voting_country_points_average_rankings(
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

            Request = Kernel.Requests.VotingCountryRankings.GetVotingCountryPointsAverageRankings(queryParams);
        }

        public async Task Then_the_retrieved_rankings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Rankings).IsEmpty();

        public async Task Then_the_retrieved_rankings_in_order_should_match(string table)
        {
            VotingCountryPointsAverageRanking[] expected = MarkdownParser.ParseTable(table, MapToRanking);

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

        private static VotingCountryPointsAverageRanking MapToRanking(Dictionary<string, string> row)
        {
            return new VotingCountryPointsAverageRanking
            {
                Rank = int.Parse(row["Rank"]),
                CountryCode = row["CountryCode"],
                CountryName = row["CountryName"],
                PointsAverage = decimal.Parse(row["PointsAverage"]),
                TotalPoints = int.Parse(row["TotalPoints"]),
                PointsAwards = int.Parse(row["PointsAwards"]),
                Broadcasts = int.Parse(row["Broadcasts"]),
                Contests = int.Parse(row["Contests"]),
            };
        }

        private sealed class RankingEqualityComparer : IEqualityComparer<VotingCountryPointsAverageRanking>
        {
            public bool Equals(VotingCountryPointsAverageRanking? x, VotingCountryPointsAverageRanking? y)
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
                    && x.Contests == y.Contests;
            }

            public int GetHashCode(VotingCountryPointsAverageRanking obj) =>
                HashCode.Combine(
                    obj.Rank,
                    obj.CountryCode,
                    obj.CountryName,
                    obj.PointsAverage,
                    obj.TotalPoints,
                    obj.PointsAwards,
                    obj.Broadcasts,
                    obj.Contests
                );
        }
    }
}

using Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils.DataSourceGenerators;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.PublicApi.V0.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.CompetingCountryRankings;

public sealed class GetCompetingCountryPointsInRangeRankingsTests : SeededParallelAcceptanceTest
{
    [Test]
    [PublicApiV0Point2AndUp]
    public async Task Endpoint_should_retrieve_page_of_rankings(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_rank_competing_countries_by_points_in_range(minPoints: 1, maxPoints: 12);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.937799      | 196                 | 209          | 4          | 2        | 40              |
            | 2    | UA          | Ukraine     | 0.73262       | 137                 | 187          | 3          | 2        | 40              |
            | 3    | IT          | Italy       | 0.695364      | 105                 | 151          | 2          | 2        | 40              |
            | 4    | IL          | Israel      | 0.679389      | 89                  | 131          | 3          | 2        | 39              |
            | 5    | NO          | Norway      | 0.678049      | 139                 | 205          | 4          | 2        | 40              |
            | 6    | ES          | Spain       | 0.629139      | 95                  | 151          | 2          | 2        | 40              |
            | 7    | NL          | Netherlands | 0.590909      | 78                  | 132          | 3          | 2        | 40              |
            | 8    | FI          | Finland     | 0.588517      | 123                 | 209          | 4          | 2        | 40              |
            | 9    | AU          | Australia   | 0.514286      | 108                 | 210          | 4          | 2        | 40              |
            | 10   | EE          | Estonia     | 0.509524      | 107                 | 210          | 4          | 2        | 40              |
            """);
        await euroFan.Then_the_response_metadata_should_match(
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 40,
            totalPages: 4,
            minPoints: 1,
            maxPoints: 12);
    }

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetCompetingCountryPointsInRangeRankingsResponse>(apiDriver)
    {
        public void Given_I_want_to_rank_competing_countries_by_points_in_range(
            string? votingMethod = null,
            string? votingCountryCode = null,
            string? contestStage = null,
            int? minYear = null,
            int? maxYear = null,
            int minPoints = 0,
            int maxPoints = 0, bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null)
        {
            Dictionary<string, object?> queryParams = new()
            {
                [nameof(descending)] = descending,
                [nameof(pageSize)] = pageSize,
                [nameof(pageIndex)] = pageIndex,
                [nameof(votingMethod)] = votingMethod is not null ? Enum.Parse<QueryableVotingMethod>(votingMethod) : null,
                [nameof(votingCountryCode)] = votingCountryCode,
                [nameof(contestStage)] = contestStage is not null ? Enum.Parse<QueryableContestStage>(contestStage) : null,
                [nameof(minYear)] = minYear,
                [nameof(maxYear)] = maxYear,
                [nameof(minPoints)] = minPoints,
                [nameof(maxPoints)] = maxPoints
            };

            Request = ApiDriver.RequestFactory.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings(queryParams);
        }

        public async Task Then_the_response_rankings_page_should_match(string rankings)
        {
            (CompetingCountryPointsInRangeRanking[] actualRankings, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetingCountryPointsInRangeRanking[] expectedRankings =
                MarkdownParser.ParseTable(rankings, MapToRanking).ToArray();

            await Assert.That(actualRankings).IsEquivalentTo(expectedRankings, CollectionOrdering.Matching);
        }

        public async Task Then_the_response_metadata_should_match(
            string? votingMethod = null,
            string? votingCountryCode = null,
            string? contestStage = null,
            int? minYear = null,
            int? maxYear = null,
            int minPoints = 0,
            int maxPoints = 0,
            bool descending = false,
            int pageSize = 0,
            int pageIndex = 0,
            int totalItems = 0,
            int totalPages = 0)
        {
            (_, CompetingCountryPointsInRangeMetadata actualMetadata) = await Assert.That(ResponseBody).IsNotNull();

            CompetingCountryPointsInRangeMetadata expectedMetadata = new()
            {
                Descending = descending,
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalItems = totalItems,
                TotalPages = totalPages,
                VotingMethod = votingMethod is not null ? Enum.Parse<QueryableVotingMethod>(votingMethod) : null,
                VotingCountryCode = votingCountryCode,
                ContestStage = contestStage is not null ? Enum.Parse<QueryableContestStage>(contestStage) : null,
                MinYear = minYear,
                MaxYear = maxYear,
                MinPoints = minPoints,
                MaxPoints = maxPoints
            };

            await Assert.That(actualMetadata).IsEqualTo(expectedMetadata);
        }

        private static CompetingCountryPointsInRangeRanking MapToRanking(Dictionary<string, string> row) => new()
        {
            Rank = int.Parse(row["Rank"]),
            CountryCode = row["CountryCode"],
            CountryName = row["CountryName"],
            PointsInRange = decimal.Parse(row["PointsInRange"]),
            PointsAwardsInRange = int.Parse(row["PointsAwardsInRange"]),
            PointsAwards = int.Parse(row["PointsAwards"]),
            Broadcasts = int.Parse(row["Broadcasts"]),
            Contests = int.Parse(row["Contests"]),
            VotingCountries = int.Parse(row["VotingCountries"])
        };
    }
}

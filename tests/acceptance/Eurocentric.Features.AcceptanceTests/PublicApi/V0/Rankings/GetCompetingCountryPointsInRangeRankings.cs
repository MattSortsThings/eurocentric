using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils.Mixins.SampleData;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.PublicApi.V0.Rankings;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Rankings;

public static class GetCompetingCountryPointsInRangeRankings
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v0.2")]
        public async Task Should_rank_competing_countries_scenario_1(string apiVersion)
        {
            EuroFan euroFan = new(RestClient, BackDoor, apiVersion);

            // Given
            await euroFan.Given_the_system_is_populated_with_the_sample_countries();
            await euroFan.Given_the_system_is_populated_with_the_2021_sample_contest();
            await euroFan.Given_the_system_is_populated_with_the_2022_sample_contest();
            euroFan.Given_I_want_to_rank_competing_countries_by_points_in_range(minPoints: 10, maxPoints: 12);

            // When
            await euroFan.When_I_send_my_request();

            // Then
            euroFan.Then_my_request_should_succeed_with_status_code(200);
            euroFan.Then_the_response_rankings_should_be(
                """
                | Rank | CountryCode | CountryName | PointsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
                |:-----|:------------|:------------|:--------------|:-------------|:-----------|:---------|:----------------|
                | 1    | BE          | Belgium     | 0.833333      | 18           | 2          | 2        | 5               |
                | 2    | AT          | Austria     | 0.555555      | 18           | 2          | 2        | 5               |
                | 2    | CZ          | Czechia     | 0.555555      | 18           | 2          | 2        | 5               |
                | 3    | DK          | Denmark     | 0.5           | 18           | 2          | 2        | 5               |
                """);
            euroFan.Then_the_response_filters_should_be(
                minPoints: 10,
                maxPoints: 12,
                contestStage: "Any",
                votingMethod: "Any");
            euroFan.Then_the_response_pagination_should_be(
                pageIndex: 0,
                pageSize: 10,
                totalItems: 4,
                totalPages: 1,
                descending: false);
        }

        [Theory]
        [InlineData("v0.2")]
        public async Task Should_rank_competing_countries_scenario_2(string apiVersion)
        {
            EuroFan euroFan = new(RestClient, BackDoor, apiVersion);

            // Given
            await euroFan.Given_the_system_is_populated_with_the_sample_countries();
            await euroFan.Given_the_system_is_populated_with_the_2021_sample_contest();
            await euroFan.Given_the_system_is_populated_with_the_2022_sample_contest();
            euroFan.Given_I_want_to_rank_competing_countries_by_points_in_range(
                minPoints: 10,
                maxPoints: 12,
                contestStage: "SemiFinals",
                votingMethod: "Jury");

            // When
            await euroFan.When_I_send_my_request();

            // Then
            euroFan.Then_my_request_should_succeed_with_status_code(200);
            euroFan.Then_the_response_rankings_should_be(
                """
                | Rank | CountryCode | CountryName | PointsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
                |:-----|:------------|:------------|:--------------|:-------------|:-----------|:---------|:----------------|
                | 1    | BE          | Belgium     | 0.888888      | 9            | 2          | 2        | 5               |
                | 2    | AT          | Austria     | 0.555555      | 9            | 2          | 2        | 5               |
                | 2    | DK          | Denmark     | 0.555555      | 9            | 2          | 2        | 5               |
                | 3    | CZ          | Czechia     | 0.444444      | 9            | 2          | 2        | 5               |
                """);
            euroFan.Then_the_response_filters_should_be(
                minPoints: 10,
                maxPoints: 12,
                contestStage: "SemiFinals",
                votingMethod: "Jury");
            euroFan.Then_the_response_pagination_should_be(
                pageIndex: 0,
                pageSize: 10,
                totalItems: 4,
                totalPages: 1,
                descending: false);
        }
    }

    private sealed class EuroFan : EuroFanActor<GetCompetingCountryPointsInRangeRankingsResponse>
    {
        public EuroFan(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
        {
        }

        public void Given_I_want_to_rank_competing_countries_by_points_in_range(bool? descending = null,
            int? pageIndex = null,
            int? pageSize = null,
            int? minYear = null,
            int? maxYear = null,
            string? contestStage = null,
            string? votingMethod = null,
            string? votingCountryCode = null,
            int minPoints = 0,
            int maxPoints = 0)
        {
            GetCompetingCountryPointsInRangeRankingsRequest query = new()
            {
                Descending = descending,
                PageIndex = pageIndex,
                PageSize = pageSize,
                MinYear = minYear,
                MaxYear = maxYear,
                ContestStage = contestStage is null ? null : Enum.Parse<ContestStageFilter>(contestStage),
                VotingMethod = votingMethod is null ? null : Enum.Parse<VotingMethodFilter>(votingMethod),
                VotingCountryCode = votingCountryCode,
                MinPoints = minPoints,
                MaxPoints = maxPoints
            };

            Request = RequestFactory.Rankings.GetCompetingCountryPointsInRangeRankings(query);
        }

        public void Then_the_response_rankings_should_be(string markdownTable)
        {
            Assert.NotNull(ResponseObject);

            CompetingCountryPointsInRangeRanking[] expectedRankings = MarkdownParser.ParseTable(markdownTable, RowMapper);
            CompetingCountryPointsInRangeRanking[] actualRankings = ResponseObject.Rankings;

            Assert.Equal(expectedRankings, actualRankings, EqualityComparer);
        }

        public void Then_the_response_filters_should_be(int? minYear = null,
            int? maxYear = null,
            string contestStage = "",
            string votingMethod = "",
            string? votingCountryCode = null,
            int minPoints = 0,
            int maxPoints = 0)
        {
            Assert.NotNull(ResponseObject);

            CompetingCountryPointsInRangeFilters expectedFilters = new()
            {
                MinYear = minYear,
                MaxYear = maxYear,
                ContestStage = Enum.Parse<ContestStageFilter>(contestStage),
                VotingMethod = Enum.Parse<VotingMethodFilter>(votingMethod),
                VotingCountryCode = votingCountryCode,
                MinPoints = minPoints,
                MaxPoints = maxPoints
            };

            Assert.Equal(expectedFilters, ResponseObject.Filters);
        }

        private static CompetingCountryPointsInRangeRanking RowMapper(Dictionary<string, string> row) => new()
        {
            Rank = int.Parse(row["Rank"]),
            CountryCode = row["CountryCode"],
            CountryName = row["CountryName"],
            PointsInRange = double.Parse(row["PointsInRange"]),
            PointsAwards = int.Parse(row["PointsAwards"]),
            Broadcasts = int.Parse(row["Broadcasts"]),
            Contests = int.Parse(row["Contests"]),
            VotingCountries = int.Parse(row["VotingCountries"])
        };

        private static bool EqualityComparer(CompetingCountryPointsInRangeRanking a, CompetingCountryPointsInRangeRanking b) =>
            a.Rank == b.Rank
            && a.CountryCode.Equals(b.CountryCode, StringComparison.Ordinal)
            && a.CountryName.Equals(b.CountryName, StringComparison.Ordinal)
            && a.PointsInRange.EqualsTo6DecimalPlaces(b.PointsInRange)
            && a.PointsAwards == b.PointsAwards
            && a.Broadcasts == b.Broadcasts
            && a.Contests == b.Contests
            && a.VotingCountries == b.VotingCountries;
    }
}

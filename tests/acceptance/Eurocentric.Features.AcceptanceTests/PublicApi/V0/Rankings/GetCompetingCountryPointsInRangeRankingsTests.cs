using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Rankings.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.PublicApi.V0.Rankings;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Rankings;

public static class GetCompetingCountryPointsInRangeRankingsTests
{
    public sealed class Feature(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v0.2")]
        public async Task Should_rank_competing_countries_scenario_1(string apiVersion)
        {
            EuroFan euroFan = new(BackDoor, RestClient, apiVersion);

            // Given
            await euroFan.Given_the_system_is_populated_with_the_sample_queryable_data();
            euroFan.Given_I_want_to_rank_competing_countries_by_points_in_range(
                minPoints: 10,
                maxPoints: 12);

            // When
            await euroFan.When_I_send_my_request();

            // Then
            euroFan.Then_my_request_should_succeed_with_status_code(200);
            euroFan.Then_the_response_rankings_should_match(
                """
                | Rank | CountryCode | CountryName | PointsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
                |:-----|:------------|:------------|:--------------|:-------------|:-----------|:---------|:----------------|
                | 1    | BE          | Belgium     | 0.833         | 18           | 2          | 2        | 5               |
                | 2    | AT          | Austria     | 0.556         | 18           | 2          | 2        | 5               |
                | 2    | CZ          | Czechia     | 0.556         | 18           | 2          | 2        | 5               |
                | 3    | DK          | Denmark     | 0.5           | 18           | 2          | 2        | 5               |
                """);
            euroFan.Then_the_response_filters_info_should_match(
                minPoints: 10,
                maxPoints: 12,
                contestStage: "Any",
                votingMethod: "Any");
            euroFan.Then_the_response_pagination_data_should_match(
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
            EuroFan euroFan = new(BackDoor, RestClient, apiVersion);

            // Given
            await euroFan.Given_the_system_is_populated_with_the_sample_queryable_data();
            euroFan.Given_I_want_to_rank_competing_countries_by_points_in_range(
                minPoints: 10,
                maxPoints: 12,
                contestStage: "SemiFinals",
                votingMethod: "Jury");

            // When
            await euroFan.When_I_send_my_request();

            // Then
            euroFan.Then_my_request_should_succeed_with_status_code(200);
            euroFan.Then_the_response_rankings_should_match(
                """
                | Rank | CountryCode | CountryName | PointsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
                |:-----|:------------|:------------|:--------------|:-------------|:-----------|:---------|:----------------|
                | 1    | BE          | Belgium     | 0.889         | 9            | 2          | 2        | 5               |
                | 2    | AT          | Austria     | 0.556         | 9            | 2          | 2        | 5               |
                | 2    | DK          | Denmark     | 0.556         | 9            | 2          | 2        | 5               |
                | 3    | CZ          | Czechia     | 0.444         | 9            | 2          | 2        | 5               |
                """);
            euroFan.Then_the_response_filters_info_should_match(
                minPoints: 10,
                maxPoints: 12,
                contestStage: "SemiFinals",
                votingMethod: "Jury");
            euroFan.Then_the_response_pagination_data_should_match(
                pageIndex: 0,
                pageSize: 10,
                totalItems: 4,
                totalPages: 1,
                descending: false);
        }

        [Theory]
        [InlineData("v0.2")]
        public async Task Should_fail_on_missing_minPoints_query_param(string apiVersion)
        {
            EuroFan euroFan = new(BackDoor, RestClient, apiVersion);

            // Given
            await euroFan.Given_the_system_is_populated_with_the_sample_queryable_data();
            euroFan.Given_I_want_to_rank_competing_countries_by_points_in_range(maxPoints: 12);

            // When
            await euroFan.When_I_send_my_request();

            // Then
            euroFan.Then_my_request_should_fail_with_status_code(400);
        }

        [Theory]
        [InlineData("v0.2")]
        public async Task Should_be_unable_to_rank_competing_countries_without_maxPoints_query_param(string apiVersion)
        {
            EuroFan euroFan = new(BackDoor, RestClient, apiVersion);

            // Given
            await euroFan.Given_the_system_is_populated_with_the_sample_queryable_data();
            euroFan.Given_I_want_to_rank_competing_countries_by_points_in_range(minPoints: 1);

            // When
            await euroFan.When_I_send_my_request();

            // Then
            euroFan.Then_my_request_should_fail_with_status_code(400);
        }
    }

    private sealed class EuroFan : ActorWithResponse<GetCompetingCountryPointsInRangeRankingsResponse>
    {
        public EuroFan(IWebAppFixtureBackDoor backDoor, IWebAppFixtureRestClient restClient, string apiVersion = "v1.0") :
            base(backDoor, restClient, apiVersion)
        {
        }

        public void Given_I_want_to_rank_competing_countries_by_points_in_range(bool? descending = null,
            int? pageIndex = null,
            int? pageSize = null,
            int? minPoints = null,
            int? maxPoints = null,
            int? minYear = null,
            int? maxYear = null,
            string? contestStage = null,
            string? votingMethod = null,
            string? votingCountryCode = null)
        {
            Request = new RestRequest("public/api/{apiVersion}/rankings/competing-countries/points-in-range");

            Request.AddUrlSegment("apiVersion", ApiVersion);

            if (descending.HasValue)
            {
                Request.AddQueryParameter("descending", descending.Value);
            }

            if (pageIndex.HasValue)
            {
                Request.AddQueryParameter("pageIndex", pageIndex.Value);
            }

            if (pageSize.HasValue)
            {
                Request.AddQueryParameter("pageSize", pageSize.Value);
            }

            if (minPoints.HasValue)
            {
                Request.AddQueryParameter("minPoints", minPoints.Value);
            }

            if (maxPoints.HasValue)
            {
                Request.AddQueryParameter("maxPoints", maxPoints.Value);
            }

            if (minYear.HasValue)
            {
                Request.AddQueryParameter("minYear", minYear.Value);
            }

            if (maxYear.HasValue)
            {
                Request.AddQueryParameter("maxYear", maxYear.Value);
            }

            if (contestStage is not null)
            {
                Request.AddQueryParameter("contestStage", contestStage);
            }

            if (votingMethod is not null)
            {
                Request.AddQueryParameter("votingMethod", votingMethod);
            }

            if (votingCountryCode is not null)
            {
                Request.AddQueryParameter("votingCountryCode", votingCountryCode);
            }
        }

        public void Then_the_response_rankings_should_match(string markdownTable)
        {
            Assert.NotNull(ResponseObject);

            CompetingCountryPointsInRangeRanking[] expectedRankings = markdownTable.ParseAll(MapToRanking);

            Assert.Equal(expectedRankings, ResponseObject.Rankings, EqualValues);
        }

        public void Then_the_response_filters_info_should_match(int? minYear = null,
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
                MinPoints = minPoints,
                MaxPoints = maxPoints,
                MinYear = minYear,
                MaxYear = maxYear,
                ContestStage = Enum.Parse<ContestStageFilter>(contestStage),
                VotingMethod = Enum.Parse<VotingMethodFilter>(votingMethod),
                VotingCountryCode = votingCountryCode
            };

            Assert.Equal(expectedFilters, ResponseObject.Filters);
        }

        private static CompetingCountryPointsInRangeRanking MapToRanking(Dictionary<string, string> row) => new()
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

        private static bool EqualValues(CompetingCountryPointsInRangeRanking a, CompetingCountryPointsInRangeRanking b) =>
            a.Rank == b.Rank
            && a.CountryCode.Equals(b.CountryCode, StringComparison.InvariantCulture)
            && a.CountryName.Equals(b.CountryName, StringComparison.InvariantCulture)
            && a.PointsInRange.EqualsTo3DecimalPlaces(b.PointsInRange)
            && a.PointsAwards == b.PointsAwards
            && a.Broadcasts == b.Broadcasts
            && a.Contests == b.Contests
            && a.VotingCountries == b.VotingCountries;
    }
}

using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V0.Common.Dtos;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.PublicApi.V0.Rankings.GetCompetingCountryPointsAverageRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Rankings;

public static class GetCompetingCountryPointsAverageRankingsTests
{
    public sealed class GetCompetingCountryPointsAverageRankings : ParallelSeededAcceptanceTest
    {
        [Test]
        [Arguments("v0.2")]
        public async Task Should_retrieve_competing_country_points_average_rankings(string apiVersion)
        {
            EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

            // Given
            euroFan.Given_I_want_to_rank_competing_countries_by_points_average();

            // When
            await euroFan.When_I_send_my_request();

            // Then
            await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
            await euroFan.Then_the_retrieved_rankings_should_match(
                """
                | Rank | CountryCode | CountryName    | PointsAverage | PointsAwards | Broadcasts | Contests | VotingCountries |
                |-----:|:------------|:---------------|--------------:|:-------------|-----------:|---------:|----------------:|
                |    1 | SE          | Sweden         |         7.426 | 209          |          4 |        2 |              40 |
                |    2 | UA          | Ukraine        |         6.476 | 187          |          3 |        2 |              40 |
                |    3 | FI          | Finland        |         4.321 | 209          |          4 |        2 |              40 |
                |    4 | IL          | Israel         |         4.198 | 131          |          3 |        2 |              39 |
                |    5 | IT          | Italy          |         4.093 | 151          |          2 |        2 |              40 |
                |    6 | ES          | Spain          |         3.702 | 151          |          2 |        2 |              40 |
                |    7 | NO          | Norway         |         3.556 | 205          |          4 |        2 |              40 |
                |    8 | GR          | Greece         |         3.308 | 133          |          3 |        2 |              40 |
                |    9 | GB          | United Kingdom |         3.245 | 151          |          2 |        2 |              40 |
                |   10 | AU          | Australia      |         3.181 | 210          |          4 |        2 |              40 |
                """);
            await euroFan.Then_the_response_metadata_should_match(contestStage: "Any",
                votingMethod: "Any",
                pageIndex: 0,
                pageSize: 10,
                descending: false,
                totalItems: 40,
                totalPages: 4);
        }

        [Test]
        [Arguments("v0.2")]
        public async Task Should_apply_pagination(string apiVersion)
        {
            EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

            // Given
            euroFan.Given_I_want_to_rank_competing_countries_by_points_average(
                pageIndex: 1,
                pageSize: 4,
                descending: true);

            // When
            await euroFan.When_I_send_my_request();

            // Then
            await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
            await euroFan.Then_the_retrieved_rankings_should_match(
                """
                | Rank | CountryCode | CountryName | PointsAverage | PointsAwards | Broadcasts | Contests | VotingCountries |
                |-----:|:------------|:------------|--------------:|-------------:|-----------:|---------:|----------------:|
                |   36 | SM          | San Marino  |         0.847 |           59 |          2 |        2 |              30 |
                |   35 | MT          | Malta       |         0.862 |           58 |          2 |        2 |              30 |
                |   34 | GE          | Georgia     |         0.932 |           59 |          2 |        2 |              30 |
                |   33 | IE          | Ireland     |         0.983 |           58 |          2 |        2 |              30 |
                """);
            await euroFan.Then_the_response_metadata_should_match(contestStage: "Any",
                votingMethod: "Any",
                pageIndex: 1,
                pageSize: 4,
                descending: true,
                totalItems: 40,
                totalPages: 10);
        }
    }

    private sealed class EuroFanActor : EuroFanActorWithResponse<GetCompetingCountryPointsAverageRankingsResponse>
    {
        public EuroFanActor(IApiDriver apiDriver) : base(apiDriver)
        {
        }

        public void Given_I_want_to_rank_competing_countries_by_points_average(bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? contestStage = null,
            int? minYear = null,
            int? maxYear = null,
            string? votingMethod = null,
            string? votingCountryCode = null)
        {
            Dictionary<string, object?> queryParams = new()
            {
                { nameof(descending), descending },
                { nameof(pageSize), pageSize },
                { nameof(pageIndex), pageIndex },
                { nameof(contestStage), contestStage },
                { nameof(minYear), minYear },
                { nameof(maxYear), maxYear },
                { nameof(votingMethod), votingMethod },
                { nameof(votingCountryCode), votingCountryCode }
            };

            Request = ApiDriver.RequestFactory.Rankings.GetCompetingCountryPointsAverageRankings(queryParams);
        }

        public async Task Then_the_retrieved_rankings_should_match(string rankings)
        {
            GetCompetingCountryPointsAverageRankingsResponse responseBody = await Assert.That(ResponseBody).IsNotNull();
            IEnumerable<CompetingCountryPointsAverageRanking> expectedRankings =
                MarkdownParser.ParseTable(rankings, MapToRanking);

            await Assert.That(responseBody.Rankings)
                .IsEquivalentTo(expectedRankings, new RankingEqualityComparer(), CollectionOrdering.Matching);
        }

        private static CompetingCountryPointsAverageRanking MapToRanking(Dictionary<string, string> row) => new()
        {
            Rank = int.Parse(row["Rank"]),
            CountryCode = row["CountryCode"],
            CountryName = row["CountryName"],
            PointsAverage = double.Parse(row["PointsAverage"]),
            PointsAwards = int.Parse(row["PointsAwards"]),
            Broadcasts = int.Parse(row["Broadcasts"]),
            Contests = int.Parse(row["Contests"]),
            VotingCountries = int.Parse(row["VotingCountries"])
        };


        public async Task Then_the_response_metadata_should_match(bool descending = true,
            int pageIndex = 0,
            int pageSize = 0,
            int totalPages = 0,
            int totalItems = 0,
            string? votingCountryCode = null,
            int? minYear = null,
            int? maxYear = null,
            string contestStage = "",
            string votingMethod = "")
        {
            GetCompetingCountryPointsAverageRankingsResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            PaginationMetadata expectedPagination = new()
            {
                Descending = descending,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalItems = totalItems
            };

            CompetingCountryPointsAverageFilters expectedFilters = new()
            {
                VotingCountryCode = votingCountryCode,
                MinYear = minYear,
                MaxYear = maxYear,
                ContestStage = Enum.Parse<QueryableContestStage>(contestStage),
                VotingMethod = Enum.Parse<QueryableVotingMethod>(votingMethod)
            };

            await Assert.That(responseBody.Pagination)
                .IsEqualTo(expectedPagination);
            await Assert.That(responseBody.Filters)
                .IsEqualTo(expectedFilters);
        }
    }

    private sealed class RankingEqualityComparer : IEqualityComparer<CompetingCountryPointsAverageRanking>
    {
        public bool Equals(CompetingCountryPointsAverageRanking? x, CompetingCountryPointsAverageRanking? y)
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
                   && x.CountryCode == y.CountryCode
                   && x.CountryName == y.CountryName
                   && Math.Abs(x.PointsAverage - y.PointsAverage) <= 0.001d
                   && x.PointsAwards == y.PointsAwards
                   && x.Broadcasts == y.Broadcasts
                   && x.Contests == y.Contests &&
                   x.VotingCountries == y.VotingCountries;
        }

        public int GetHashCode(CompetingCountryPointsAverageRanking obj) => HashCode.Combine(obj.Rank, obj.CountryCode,
            obj.CountryName, obj.PointsAverage, obj.PointsAwards, obj.Broadcasts, obj.Contests, obj.VotingCountries);
    }
}

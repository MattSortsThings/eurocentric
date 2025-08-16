using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsAverageRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Rankings;

public sealed class GetCompetingCountryPointsAverageRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_minimum_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsAverage | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|---------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden         | 7.425837      | 209          | 4          | 2        | 40              |
            | 2    | UA          | Ukraine        | 6.475936      | 187          | 3          | 2        | 40              |
            | 3    | FI          | Finland        | 4.320574      | 209          | 4          | 2        | 40              |
            | 4    | IL          | Israel         | 4.198473      | 131          | 3          | 2        | 39              |
            | 5    | IT          | Italy          | 4.092715      | 151          | 2          | 2        | 40              |
            | 6    | ES          | Spain          | 3.701987      | 151          | 2          | 2        | 40              |
            | 7    | NO          | Norway         | 3.556098      | 205          | 4          | 2        | 40              |
            | 8    | GR          | Greece         | 3.308271      | 133          | 3          | 2        | 40              |
            | 9    | GB          | United Kingdom | 3.245033      | 151          | 2          | 2        | 40              |
            | 10   | AU          | Australia      | 3.180952      | 210          | 4          | 2        | 40              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "Any",
            votingMethod: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 40,
            totalPages: 4);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_pagination_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
            pageIndex: 1,
            pageSize: 3,
            descending: true);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|--------------|------------|----------|-----------------|
            | 37   | ME          | Montenegro  | 0.825         | 40           | 1          | 1        | 20              |
            | 36   | SM          | San Marino  | 0.847458      | 59           | 2          | 2        | 30              |
            | 35   | MT          | Malta       | 0.862069      | 58           | 2          | 2        | 30              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "Any",
            votingMethod: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 1,
            pageSize: 3,
            descending: true,
            totalItems: 40,
            totalPages: 14);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_contest_stage(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
            contestStage: "GrandFinal",
            pageSize: 5);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 6.761589      | 151          | 2          | 2        | 40              |
            | 2    | UA          | Ukraine     | 5.788079      | 151          | 2          | 2        | 40              |
            | 3    | IL          | Israel      | 4.958904      | 73           | 1          | 1        | 37              |
            | 4    | IT          | Italy       | 4.092715      | 151          | 2          | 2        | 40              |
            | 5    | FI          | Finland     | 3.735099      | 151          | 2          | 2        | 40              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "GrandFinal",
            votingMethod: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 31,
            totalPages: 7);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_contest_year_range(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
            minYear: 2023,
            pageSize: 5);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 7.890110      | 91           | 2          | 1        | 37              |
            | 2    | FI          | Finland     | 7.725275      | 91           | 2          | 1        | 37              |
            | 3    | IL          | Israel      | 5.373626      | 91           | 2          | 1        | 37              |
            | 4    | IT          | Italy       | 4.794521      | 73           | 1          | 1        | 37              |
            | 5    | NO          | Norway      | 4.065934      | 91           | 2          | 1        | 37              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "Any",
            minYear: 2023,
            votingMethod: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 37,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_voting_country(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
            votingCountryCode: "GB",
            pageSize: 5);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 8.333333      | 6            | 3          | 2        | 1               |
            | 2    | PL          | Poland      | 7.428571      | 7            | 4          | 2        | 1               |
            | 3    | LT          | Lithuania   | 7.4           | 5            | 3          | 2        | 1               |
            | 4    | IE          | Ireland     | 6             | 2            | 1          | 1        | 1               |
            | 5    | ES          | Spain       | 5             | 4            | 2          | 2        | 1               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "Any",
            votingCountryCode: "GB",
            votingMethod: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 37,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_voting_method_jury_only(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
            votingMethod: "Jury",
            pageSize: 5);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 8.631579      | 95           | 3          | 2        | 39              |
            | 2    | GR          | Greece      | 5.421053      | 57           | 2          | 1        | 39              |
            | 3    | NL          | Netherlands | 4.754386      | 57           | 2          | 1        | 39              |
            | 4    | IT          | Italy       | 4.453333      | 75           | 2          | 2        | 39              |
            | 5    | AU          | Australia   | 4.442105      | 95           | 3          | 2        | 39              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "Any",
            votingMethod: "Jury");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_voting_method_televote_only(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
            votingMethod: "Televote",
            pageSize: 5);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine     | 8.829787      | 94           | 3          | 2        | 40              |
            | 2    | SE          | Sweden      | 6.421053      | 114          | 4          | 2        | 40              |
            | 3    | FI          | Finland     | 5.947368      | 114          | 4          | 2        | 40              |
            | 4    | NO          | Norway      | 5.071429      | 112          | 4          | 2        | 40              |
            | 5    | MD          | Moldova     | 4.991071      | 112          | 4          | 2        | 40              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "Any",
            votingMethod: "Televote");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_given_multiple_filtering_and_pagination_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
            contestStage: "SemiFinals",
            minYear: 2022,
            maxYear: 2023,
            votingCountryCode: "SM",
            votingMethod: "Televote",
            pageSize: 3,
            pageIndex: 1);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|--------------|------------|----------|-----------------|
            | 4    | AT          | Austria     | 8             | 1            | 1          | 1        | 1               |
            | 5    | IS          | Iceland     | 7             | 1            | 1          | 1        | 1               |
            | 6    | AU          | Australia   | 6.5           | 2            | 2          | 2        | 1               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            contestStage: "SemiFinals",
            minYear: 2022,
            maxYear: 2023,
            votingCountryCode: "SM",
            votingMethod: "Televote");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 1,
            pageSize: 3,
            descending: false,
            totalItems: 25,
            totalPages: 9);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_invalid_pageIndex_query_param(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(pageIndex: -1);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Page index out of range",
            detail: "Query parameter 'pageIndex' value must be greater than or equal to 0.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("pageIndex", -1);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_invalid_pageSize_query_param(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(pageSize: 0);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Page size out of range",
            detail: "Query parameter 'pageSize' value must be greater than or equal to 1.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("pageSize", 0);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_invalid_minYear_and_maxYear_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(minYear: 2050, maxYear: 2016);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Invalid contest year range",
            detail: "Query parameter 'minYear' value must be less than or equal to query parameter 'maxYear' value.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("minYear", 2050);
        await euroFan.Then_the_response_problem_details_extensions_should_include("maxYear", 2016);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_invalid_votingCountryCode_query_param(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(votingCountryCode: "San Marino");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Invalid voting country code",
            detail: "Query parameter 'votingCountryCode' value must be a string of 2 upper-case letters.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("votingCountryCode", "San Marino");
    }

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetCompetingCountryPointsAverageRankingsResponse>(apiDriver)
    {
        public void Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingMethod = null,
            string? votingCountryCode = null,
            int? maxYear = null,
            int? minYear = null,
            string? contestStage = null)
        {
            Dictionary<string, object?> queryParams = new()
            {
                [nameof(descending)] = descending,
                [nameof(pageSize)] = pageSize,
                [nameof(pageIndex)] = pageIndex,
                [nameof(votingMethod)] = votingMethod is not null ? Enum.Parse<QueryableVotingMethod>(votingMethod) : null,
                [nameof(votingCountryCode)] = votingCountryCode,
                [nameof(maxYear)] = maxYear,
                [nameof(minYear)] = minYear,
                [nameof(contestStage)] = contestStage is not null ? Enum.Parse<QueryableContestStage>(contestStage) : null
            };

            Request = ApiDriver.RequestFactory.Rankings.GetCompetingCountryPointsAverageRankings(queryParams);
        }

        public async Task Then_the_response_rankings_page_should_match(string page)
        {
            (CompetingCountryPointsAverageRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetingCountryPointsAverageRanking[] expected = MarkdownParser.ParseTable(page, MapRowToRanking).ToArray();

            await Assert.That(rankings).IsEquivalentTo(expected, new RankingEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_response_filtering_metadata_should_match(string votingMethod = "",
            string? votingCountryCode = null,
            int? maxYear = null,
            int? minYear = null,
            string contestStage = "")
        {
            (_, CompetingCountryPointsAverageFilteringMetadata filtering, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetingCountryPointsAverageFilteringMetadata expectedFiltering = new()
            {
                ContestStage = Enum.Parse<QueryableContestStage>(contestStage),
                MinYear = minYear,
                MaxYear = maxYear,
                VotingCountryCode = votingCountryCode,
                VotingMethod = Enum.Parse<QueryableVotingMethod>(votingMethod)
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

        private static CompetingCountryPointsAverageRanking MapRowToRanking(Dictionary<string, string> row) => new()
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
                       && x.PointsAverage.EquivalentTo6dp(y.PointsAverage)
                       && x.PointsAwards == y.PointsAwards
                       && x.Broadcasts == y.Broadcasts
                       && x.Contests == y.Contests
                       && x.VotingCountries == y.VotingCountries;
            }

            public int GetHashCode(CompetingCountryPointsAverageRanking obj) => HashCode.Combine(obj.Rank,
                obj.CountryCode,
                obj.CountryName,
                obj.PointsAverage,
                obj.PointsAwards,
                obj.Broadcasts,
                obj.Contests,
                obj.VotingCountries);
        }
    }
}

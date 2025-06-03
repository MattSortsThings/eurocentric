using System.Net;
using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.PublicApi.V0.Common.Dtos;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.PublicApi.V0.VotingCountryRankings;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.VotingCountryRankings;

public sealed class GetPointsShareVotingCountryRankingsTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v0.2")]
    public async Task Should_be_able_to_retrieve_points_share_voting_country_rankings_page(string apiVersion)
    {
        EuroFanActor euroFan = new(PublicApiV0Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        euroFan.Given_the_system_has_been_populated_with_the_sample_countries();
        euroFan.Given_the_system_has_been_populated_with_the_Turin_2022_sample_contest();
        euroFan.Given_the_system_has_been_populated_with_the_Liverpool_2023_sample_contest();
        euroFan.Given_I_want_to_rank_voting_countries_by_points_share(competingCountryCode: "GB");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        euroFan.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        euroFan.Then_the_retrieved_page_of_rankings_should_be(
            """
            | Rank | CountryCode | CountryName | PointsAwards | TotalPoints | AvailablePoints | PointsShare |
            |    1 | UA          | Ukraine     |            4 |          28 |              48 |    0.583333 |
            |    2 | BG          | Bulgaria    |            2 |          13 |              24 |    0.541667 |
            |    3 | MT          | Malta       |            4 |          24 |              48 |         0.5 |
            |    4 | AT          | Austria     |            4 |          20 |              48 |    0.416667 |
            |    4 | AZ          | Azerbaijan  |            4 |          20 |              48 |    0.416667 |
            |    4 | IL          | Israel      |            4 |          20 |              48 |    0.416667 |
            |    5 | DE          | Germany     |            4 |          18 |              48 |       0.375 |
            |    5 | FI          | Finland     |            4 |          18 |              48 |       0.375 |
            |    5 | SE          | Sweden      |            4 |          18 |              48 |       0.375 |
            |    6 | GE          | Georgia     |            4 |          16 |              48 |    0.333333 |
            """);
        euroFan.Then_the_retrieved_filters_metadata_should_be(competingCountryCode: "GB",
            votingMethod: "Any",
            contestStages: "All");
        euroFan.Then_the_retrieved_pagination_metadata_should_be(pageIndex: 0,
            pageSize: 10,
            totalItems: 40,
            totalPages: 4,
            descending: false);
    }

    [Theory]
    [InlineData("v0.2")]
    public async Task Should_be_able_to_retrieve_empty_rankings_page_when_no_queryable_data(string apiVersion)
    {
        EuroFanActor euroFan = new(PublicApiV0Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        euroFan.Given_I_want_to_rank_voting_countries_by_points_share(competingCountryCode: "AT");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        euroFan.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        euroFan.Then_the_retrieved_page_of_rankings_should_be_an_empty_list();
        euroFan.Then_the_retrieved_filters_metadata_should_be(competingCountryCode: "AT",
            votingMethod: "Any",
            contestStages: "All");
        euroFan.Then_the_retrieved_pagination_metadata_should_be(pageIndex: 0,
            pageSize: 10,
            totalItems: 0,
            totalPages: 0,
            descending: false);
    }

    private sealed class EuroFanActor : ActorWithResponse<GetPointsShareVotingCountryRankingsResponse>
    {
        public EuroFanActor(IPublicApiV0Driver apiDriver, IWebAppFixtureBackDoor backDoor) : base(apiDriver)
        {
            BackDoor = backDoor;
        }

        private IWebAppFixtureBackDoor BackDoor { get; }

        public void Given_the_system_has_been_populated_with_the_sample_countries() =>
            BackDoor.ExecuteScoped(SampleDataOperations.PopulateSampleCountries);

        public void Given_the_system_has_been_populated_with_the_Turin_2022_sample_contest() =>
            BackDoor.ExecuteScoped(SampleDataOperations.PopulateTurin2022Contest);

        public void Given_the_system_has_been_populated_with_the_Liverpool_2023_sample_contest() =>
            BackDoor.ExecuteScoped(SampleDataOperations.PopulateLiverpool2023Contest);

        public void Given_I_want_to_rank_voting_countries_by_points_share(int? startYear = null,
            int? endYear = null,
            string? competingCountryCode = null,
            string? votingMethod = null,
            string? contestStages = null,
            int? pageIndex = null,
            int? pageSize = null,
            bool? descending = null)
        {
            Dictionary<string, object> queryParams = new();

            if (startYear is not null)
            {
                queryParams.Add("startYear", startYear);
            }

            if (endYear is not null)
            {
                queryParams.Add("endYear", endYear);
            }

            if (competingCountryCode is not null)
            {
                queryParams.Add("competingCountryCode", competingCountryCode);
            }

            if (votingMethod is not null)
            {
                queryParams.Add("votingMethod", votingMethod);
            }

            if (contestStages is not null)
            {
                queryParams.Add("contestStages", contestStages);
            }

            if (pageIndex is not null)
            {
                queryParams.Add("pageIndex", pageIndex);
            }

            if (pageSize is not null)
            {
                queryParams.Add("pageSize", pageSize);
            }

            if (descending is not null)
            {
                queryParams.Add("descending", descending);
            }

            Request = apiDriver => apiDriver.VotingCountryRankings.GetPointsShareVotingCountryRankings(queryParams,
                TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_page_of_rankings_should_be(string multiLineTable)
        {
            Assert.NotNull(ResponseObject);

            PointsShareVotingCountryRanking[] expected = multiLineTable.ParseItems<PointsShareVotingCountryRanking>();

            Assert.Equal(expected, ResponseObject.Rankings);
        }

        public void Then_the_retrieved_page_of_rankings_should_be_an_empty_list()
        {
            Assert.NotNull(ResponseObject);
            Assert.Empty(ResponseObject.Rankings);
        }

        public void Then_the_retrieved_filters_metadata_should_be(int? startYear = null,
            int? endYear = null,
            string competingCountryCode = "",
            string votingMethod = "",
            string contestStages = "")
        {
            Assert.NotNull(ResponseObject);

            PointsShareVotingCountryRankingFilters expected = new()
            {
                CompetingCountryCode = competingCountryCode,
                StartYear = startYear,
                EndYear = endYear,
                VotingMethod = Enum.Parse<VotingMethod>(votingMethod),
                ContestStages = Enum.Parse<ContestStages>(contestStages)
            };

            Assert.Equal(expected, ResponseObject.Filters);
        }

        public void Then_the_retrieved_pagination_metadata_should_be(int totalPages = 0,
            int totalItems = 0,
            int pageIndex = 0,
            int pageSize = 0,
            bool descending = false)
        {
            Assert.NotNull(ResponseObject);

            PaginationMetadata expected = new()
            {
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Descending = descending
            };

            Assert.Equal(expected, ResponseObject.Pagination);
        }
    }
}

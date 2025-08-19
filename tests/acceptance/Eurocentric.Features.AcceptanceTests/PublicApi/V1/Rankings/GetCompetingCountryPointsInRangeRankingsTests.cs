using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsInRangeRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Rankings;

public sealed class GetCompetingCountryPointsInRangeRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_minimum_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12);

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
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
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
    public async Task Endpoint_should_retrieve_rankings_with_requested_pagination(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            pageIndex: 1,
            pageSize: 3,
            descending: true);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 37   | SM          | San Marino  | 0.186441      | 11                  | 59           | 2          | 2        | 30              |
            | 36   | FR          | France      | 0.238411      | 36                  | 151          | 2          | 2        | 40              |
            | 35   | RO          | Romania     | 0.270073      | 37                  | 137          | 3          | 2        | 40              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            pageSize: 5,
            contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.913907      | 138                 | 151          | 2          | 2        | 40              |
            | 2    | IL          | Israel      | 0.753425      | 55                  | 73           | 1          | 1        | 37              |
            | 3    | IT          | Italy       | 0.695364      | 105                 | 151          | 2          | 2        | 40              |
            | 4    | UA          | Ukraine     | 0.682119      | 103                 | 151          | 2          | 2        | 40              |
            | 5    | ES          | Spain       | 0.629139      | 95                  | 151          | 2          | 2        | 40              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
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
    public async Task Endpoint_should_retrieve_rankings_filtering_by_min_year(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            pageSize: 5,
            minYear: 2023);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.989011      | 90                  | 91           | 2          | 1        | 37              |
            | 2    | FI          | Finland     | 0.846154      | 77                  | 91           | 2          | 1        | 37              |
            | 3    | IL          | Israel      | 0.802198      | 73                  | 91           | 2          | 1        | 37              |
            | 4    | IT          | Italy       | 0.767123      | 56                  | 73           | 1          | 1        | 37              |
            | 5    | NO          | Norway      | 0.703297      | 64                  | 91           | 2          | 1        | 37              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any",
            minYear: 2023);
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 37,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_max_year(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            pageSize: 5,
            maxYear: 2022);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | ES          | Spain          | 0.923077      | 72                  | 78           | 1          | 1        | 39              |
            | 2    | SE          | Sweden         | 0.898305      | 106                 | 118          | 2          | 1        | 39              |
            | 3    | GB          | United Kingdom | 0.884615      | 69                  | 78           | 1          | 1        | 39              |
            | 4    | UA          | Ukraine        | 0.868421      | 99                  | 114          | 2          | 1        | 39              |
            | 5    | RS          | Serbia         | 0.686441      | 81                  | 118          | 2          | 1        | 39              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any",
            maxYear: 2022);
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_voting_country_code(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            pageSize: 5,
            votingCountryCode: "GB");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 1             | 6                   | 6            | 3          | 2        | 1               |
            | 2    | EE          | Estonia     | 0.857143      | 6                   | 7            | 4          | 2        | 1               |
            | 2    | PL          | Poland      | 0.857143      | 6                   | 7            | 4          | 2        | 1               |
            | 4    | LT          | Lithuania   | 0.8           | 4                   | 5            | 3          | 2        | 1               |
            | 5    | ES          | Spain       | 0.75          | 3                   | 4            | 2          | 2        | 1               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any",
            votingCountryCode: "GB");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 37,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_voting_method_any(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            pageSize: 5,
            votingMethod: "Any");

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
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_voting_method_jury_only(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            pageSize: 5,
            votingMethod: "Jury");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.936842      | 89                  | 95           | 3          | 2        | 39              |
            | 2    | GR          | Greece      | 0.77193       | 44                  | 57           | 2          | 1        | 39              |
            | 3    | NL          | Netherlands | 0.754386      | 43                  | 57           | 2          | 1        | 39              |
            | 4    | ES          | Spain       | 0.733333      | 55                  | 75           | 2          | 2        | 39              |
            | 5    | IT          | Italy       | 0.706667      | 53                  | 75           | 2          | 2        | 39              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            pageSize: 5,
            votingMethod: "Televote");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.938596      | 107                 | 114          | 4          | 2        | 40              |
            | 2    | NO          | Norway      | 0.910714      | 102                 | 112          | 4          | 2        | 40              |
            | 3    | UA          | Ukraine     | 0.904255      | 85                  | 94           | 3          | 2        | 40              |
            | 4    | MD          | Moldova     | 0.776786      | 87                  | 112          | 4          | 2        | 40              |
            | 5    | FI          | Finland     | 0.710526      | 81                  | 114          | 4          | 2        | 40              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
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
    public async Task Endpoint_should_retrieve_rankings_filtering_by_multiple_facets_scenario_1(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 0,
            pageSize: 5,
            minYear: 2016,
            maxYear: 2022,
            contestStage: "SemiFinals",
            votingMethod: "Televote");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName     | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-----------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | AZ          | Azerbaijan      | 1             | 20                  | 20           | 1          | 1        | 20              |
            | 2    | SI          | Slovenia        | 0.944444      | 17                  | 18           | 1          | 1        | 18              |
            | 3    | ME          | Montenegro      | 0.9           | 18                  | 20           | 1          | 1        | 20              |
            | 4    | MK          | North Macedonia | 0.85          | 17                  | 20           | 1          | 1        | 20              |
            | 5    | BG          | Bulgaria        | 0.833333      | 15                  | 18           | 1          | 1        | 18              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 0,
            maxPoints: 0,
            minYear: 2016,
            maxYear: 2022,
            contestStage: "SemiFinals",
            votingMethod: "Televote");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 35,
            totalPages: 7);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_multiple_facets_scenario_2(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 5,
            maxPoints: 10,
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any",
            votingCountryCode: "SM",
            votingMethod: "Jury");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 12   | AL          | Albania     | 0             | 0                   | 1            | 1          | 1        | 1               |
            | 12   | AM          | Armenia     | 0             | 0                   | 2            | 2          | 2        | 1               |
            | 12   | CH          | Switzerland | 0             | 0                   | 2            | 2          | 2        | 1               |
            | 12   | CY          | Cyprus      | 0             | 0                   | 2            | 2          | 2        | 1               |
            | 12   | DE          | Germany     | 0             | 0                   | 2            | 2          | 2        | 1               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 5,
            maxPoints: 10,
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any",
            votingCountryCode: "SM",
            votingMethod: "Jury");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 36,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_empty_rankings_page_when_no_data_matches_filtering(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            minYear: 2023,
            maxYear: 2023,
            contestStage: "SemiFinals",
            votingMethod: "Jury");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_be_empty();
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            minYear: 2023,
            maxYear: 2023,
            contestStage: "SemiFinals",
            votingMethod: "Jury");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 0,
            totalPages: 0);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_missing_minPoints_query_param(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            maxPoints: 0);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Bad HTTP request",
            detail: "BadHttpRequestException was thrown while handling the request.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("exceptionMessage",
            "Required parameter \"int MinPoints\" was not provided from query string.");
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_missing_maxPoints_query_param(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 0);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Bad HTTP request",
            detail: "BadHttpRequestException was thrown while handling the request.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("exceptionMessage",
            "Required parameter \"int MaxPoints\" was not provided from query string.");
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_pageIndex_query_param_value_less_than_0(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 0,
            pageIndex: -1);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Page index out of range",
            detail: "Query parameter 'pageIndex' value must be an integer greater than or equal to 0.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("pageIndex", -1);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_pageSize_query_param_value_less_than_1(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 0,
            pageSize: 0);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Page size out of range",
            detail: "Query parameter 'pageSize' value must be an integer between 1 and 100.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("pageSize", 0);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_pageSize_query_param_value_greater_than_100(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 0,
            pageSize: 101);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Page size out of range",
            detail: "Query parameter 'pageSize' value must be an integer between 1 and 100.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("pageSize", 101);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_minYear_query_param_value_greater_than_maxYear_query_param_value(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 0,
            minYear: 2050,
            maxYear: 2016);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Invalid contest year range",
            detail: "Query parameter 'minYear' integer value must not be greater than query parameter 'maxYear' integer value.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("minYear", 2050);
        await euroFan.Then_the_response_problem_details_extensions_should_include("maxYear", 2016);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_votingCountryCode_query_param_value_that_is_not_string_of_2_upper_case_letters(
        string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 0,
            votingCountryCode: "NOT_A_COUNTRY_CODE");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Invalid voting country code",
            detail: "Query parameter 'votingCountryCode' value must be a string of 2 upper-case letters.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("votingCountryCode", "NOT_A_COUNTRY_CODE");
    }

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetCompetingCountryPointsInRangeRankingsResponse>(apiDriver)
    {
        public void Given_I_want_to_obtain_a_page_of_competing_country_points_in_range_rankings(bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingMethod = null,
            string? votingCountryCode = null,
            int? maxYear = null,
            int? minYear = null,
            string? contestStage = null,
            int? minPoints = null,
            int? maxPoints = null)
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
                [nameof(contestStage)] = contestStage is not null ? Enum.Parse<QueryableContestStage>(contestStage) : null,
                [nameof(minPoints)] = minPoints,
                [nameof(maxPoints)] = maxPoints
            };

            Request = ApiDriver.RequestFactory.Rankings.GetCompetingCountryPointsInRangeRankings(queryParams);
        }

        public async Task Then_the_response_rankings_page_should_match(string page)
        {
            (CompetingCountryPointsInRangeRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetingCountryPointsInRangeRanking[] expected = MarkdownParser.ParseTable(page, MapRowToRanking).ToArray();

            await Assert.That(rankings).IsEquivalentTo(expected, CollectionOrdering.Matching);
        }

        public async Task Then_the_response_rankings_page_should_be_empty()
        {
            (CompetingCountryPointsInRangeRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(rankings).IsEmpty();
        }

        public async Task Then_the_response_filtering_metadata_should_match(string votingMethod = "",
            string? votingCountryCode = null,
            int? maxYear = null,
            int? minYear = null,
            string contestStage = "",
            int minPoints = 0,
            int maxPoints = 0)
        {
            (_, CompetingCountryPointsInRangeFilteringMetadata filtering, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetingCountryPointsInRangeFilteringMetadata expectedFiltering = new()
            {
                ContestStage = Enum.Parse<QueryableContestStage>(contestStage),
                MinPoints = minPoints,
                MaxPoints = maxPoints,
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

        private static CompetingCountryPointsInRangeRanking MapRowToRanking(Dictionary<string, string> row) => new()
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

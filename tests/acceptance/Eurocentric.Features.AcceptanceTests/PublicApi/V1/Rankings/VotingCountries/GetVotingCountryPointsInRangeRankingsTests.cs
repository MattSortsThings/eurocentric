using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsInRangeRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Rankings.VotingCountries;

public sealed class GetVotingCountryPointsInRangeRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_minimum_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
            minPoints: 1,
            maxPoints: 12);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName       | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|---------------------|--------------|------------|----------|
            | 1    | EE          | Estonia           | 1             | 6                   | 6            | 3          | 2        |
            | 1    | XX          | Rest of the World | 1             | 2                   | 2            | 2          | 1        |
            | 3    | CZ          | Czechia           | 0.857143      | 6                   | 7            | 4          | 2        |
            | 3    | MT          | Malta             | 0.857143      | 6                   | 7            | 4          | 2        |
            | 3    | RS          | Serbia            | 0.857143      | 6                   | 7            | 4          | 2        |
            | 3    | SE          | Sweden            | 0.857143      | 6                   | 7            | 4          | 2        |
            | 7    | GE          | Georgia           | 0.833333      | 5                   | 6            | 3          | 2        |
            | 8    | AZ          | Azerbaijan        | 0.714286      | 5                   | 7            | 4          | 2        |
            | 9    | BE          | Belgium           | 0.666667      | 4                   | 6            | 3          | 2        |
            | 9    | CY          | Cyprus            | 0.666667      | 4                   | 6            | 3          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
            | Rank | CountryCode | CountryName    | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|----------------|---------------|---------------------|--------------|------------|----------|
            | 37   | SI          | Slovenia       | 0.25          | 1                   | 4            | 2          | 2        |
            | 36   | GB          | United Kingdom | 0.333333      | 2                   | 6            | 3          | 2        |
            | 33   | CH          | Switzerland    | 0.4           | 2                   | 5            | 3          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
            | Rank | CountryCode | CountryName       | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|---------------------|--------------|------------|----------|
            | 1    | EE          | Estonia           | 1             | 4                   | 4            | 2          | 2        |
            | 1    | XX          | Rest of the World | 1             | 1                   | 1            | 1          | 1        |
            | 3    | CZ          | Czechia           | 0.75          | 3                   | 4            | 2          | 2        |
            | 3    | GE          | Georgia           | 0.75          | 3                   | 4            | 2          | 2        |
            | 3    | MT          | Malta             | 0.75          | 3                   | 4            | 2          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
            minPoints: 1,
            maxPoints: 12,
            contestStage: "GrandFinal",
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
    public async Task Endpoint_should_retrieve_rankings_filtering_by_min_year(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|
            | 1    | AM          | Armenia     | 1             | 2                   | 2            | 1          | 1        |
            | 1    | AT          | Austria     | 1             | 2                   | 2            | 1          | 1        |
            | 1    | AU          | Australia   | 1             | 2                   | 2            | 1          | 1        |
            | 1    | AZ          | Azerbaijan  | 1             | 3                   | 3            | 2          | 1        |
            | 1    | BE          | Belgium     | 1             | 2                   | 2            | 1          | 1        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|
            | 1    | EE          | Estonia     | 1             | 4                   | 4            | 2          | 1        |
            | 2    | CZ          | Czechia     | 0.75          | 3                   | 4            | 2          | 1        |
            | 2    | GE          | Georgia     | 0.75          | 3                   | 4            | 2          | 1        |
            | 2    | MT          | Malta       | 0.75          | 3                   | 4            | 2          | 1        |
            | 2    | RS          | Serbia      | 0.75          | 3                   | 4            | 2          | 1        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
            minPoints: 1,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any",
            maxYear: 2022);
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 39,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_voting_method_any(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
            | Rank | CountryCode | CountryName       | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|---------------------|--------------|------------|----------|
            | 1    | EE          | Estonia           | 1             | 6                   | 6            | 3          | 2        |
            | 1    | XX          | Rest of the World | 1             | 2                   | 2            | 2          | 1        |
            | 3    | CZ          | Czechia           | 0.857143      | 6                   | 7            | 4          | 2        |
            | 3    | MT          | Malta             | 0.857143      | 6                   | 7            | 4          | 2        |
            | 3    | RS          | Serbia            | 0.857143      | 6                   | 7            | 4          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|
            | 1    | EE          | Estonia     | 1             | 3                   | 3            | 3          | 2        |
            | 1    | RS          | Serbia      | 1             | 3                   | 3            | 3          | 2        |
            | 3    | AZ          | Azerbaijan  | 0.666667      | 2                   | 3            | 3          | 2        |
            | 3    | BE          | Belgium     | 0.666667      | 2                   | 3            | 3          | 2        |
            | 3    | CY          | Cyprus      | 0.666667      | 2                   | 3            | 3          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
            minPoints: 1,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Jury");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 39,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_voting_method_televote_only(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|
            | 1    | AL          | Albania     | 1             | 2                   | 2            | 2          | 2        |
            | 1    | CZ          | Czechia     | 1             | 4                   | 4            | 4          | 2        |
            | 1    | EE          | Estonia     | 1             | 3                   | 3            | 3          | 2        |
            | 1    | GE          | Georgia     | 1             | 3                   | 3            | 3          | 2        |
            | 1    | LV          | Latvia      | 1             | 3                   | 3            | 3          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "AU",
            minPoints: 10,
            maxPoints: 10,
            pageSize: 5,
            minYear: 2016,
            maxYear: 2023,
            contestStage: "SemiFinals",
            votingMethod: "Televote");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|
            | 1    | DK          | Denmark     | 1             | 1                   | 1            | 1          | 1        |
            | 2    | RO          | Romania     | 0.5           | 1                   | 2            | 2          | 2        |
            | 3    | AL          | Albania     | 0             | 0                   | 1            | 1          | 1        |
            | 3    | AM          | Armenia     | 0             | 0                   | 1            | 1          | 1        |
            | 3    | AT          | Austria     | 0             | 0                   | 1            | 1          | 1        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "AU",
            minPoints: 10,
            maxPoints: 10,
            minYear: 2016,
            maxYear: 2023,
            contestStage: "SemiFinals",
            votingMethod: "Televote");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 30,
            totalPages: 6);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_multiple_facets_scenario_2(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "SI",
            minPoints: 0,
            maxPoints: 0,
            pageIndex: 3,
            pageSize: 5,
            descending: true,
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any",
            votingMethod: "Any");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|
            | 21   | CY          | Cyprus      | 0.666667      | 2                   | 3            | 2          | 1        |
            | 21   | ES          | Spain       | 0.666667      | 2                   | 3            | 2          | 1        |
            | 21   | GE          | Georgia     | 0.666667      | 2                   | 3            | 2          | 1        |
            | 18   | LV          | Latvia      | 0.75          | 3                   | 4            | 2          | 2        |
            | 18   | MD          | Moldova     | 0.75          | 3                   | 4            | 2          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "SI",
            minPoints: 0,
            maxPoints: 0,
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any",
            votingMethod: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 3,
            pageSize: 5,
            descending: true,
            totalItems: 38,
            totalPages: 8);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_empty_rankings_page_when_no_data_matches_filtering(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "XX",
            minPoints: 0,
            maxPoints: 12,
            minYear: 2050,
            maxYear: 2016,
            votingMethod: "Any",
            contestStage: "Any");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_be_empty();
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "XX",
            minPoints: 0,
            maxPoints: 12,
            minYear: 2050,
            maxYear: 2016,
            votingMethod: "Any",
            contestStage: "Any");
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
    public async Task Endpoint_should_fail_on_missing_competingCountryCode_query_param(
        string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 0);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Bad HTTP request",
            detail: "BadHttpRequestException was thrown while handling the request.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("exceptionMessage",
            "Required parameter \"string CompetingCountryCode\" was not provided from query string.");
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_competingCountryCode_query_param_value_that_is_not_string_of_2_upper_case_letters(
        string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 0,
            competingCountryCode: "NOT_A_COUNTRY_CODE");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Invalid competing country code",
            detail: "Query parameter 'competingCountryCode' value must be a string of 2 upper-case letters.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("competingCountryCode", "NOT_A_COUNTRY_CODE");
    }

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetVotingCountryPointsInRangeRankingsResponse>(apiDriver)
    {
        public void Given_I_want_to_obtain_a_page_of_voting_country_points_in_range_rankings(bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingMethod = null,
            string? competingCountryCode = null,
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
                [nameof(competingCountryCode)] = competingCountryCode,
                [nameof(maxYear)] = maxYear,
                [nameof(minYear)] = minYear,
                [nameof(contestStage)] = contestStage is not null ? Enum.Parse<QueryableContestStage>(contestStage) : null,
                [nameof(minPoints)] = minPoints,
                [nameof(maxPoints)] = maxPoints
            };

            Request = ApiDriver.RequestFactory.VotingCountryRankings.GetVotingCountryPointsInRangeRankings(queryParams);
        }

        public async Task Then_the_response_rankings_page_should_match(string page)
        {
            (VotingCountryPointsInRangeRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            VotingCountryPointsInRangeRanking[] expected = MarkdownParser.ParseTable(page, MapRowToRanking).ToArray();

            await Assert.That(rankings).IsEquivalentTo(expected, CollectionOrdering.Matching);
        }

        public async Task Then_the_response_rankings_page_should_be_empty()
        {
            (VotingCountryPointsInRangeRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(rankings).IsEmpty();
        }

        public async Task Then_the_response_query_metadata_should_match(string votingMethod = "",
            string competingCountryCode = "",
            int? maxYear = null,
            int? minYear = null,
            string contestStage = "",
            int minPoints = 0,
            int maxPoints = 0)
        {
            (_, VotingCountryPointsInRangeQueryMetadata query, _) = await Assert.That(ResponseBody).IsNotNull();

            VotingCountryPointsInRangeQueryMetadata expectedQuery = new()
            {
                ContestStage = Enum.Parse<QueryableContestStage>(contestStage),
                MinPoints = minPoints,
                MaxPoints = maxPoints,
                MinYear = minYear,
                MaxYear = maxYear,
                CompetingCountryCode = competingCountryCode,
                VotingMethod = Enum.Parse<QueryableVotingMethod>(votingMethod)
            };

            await Assert.That(query).IsEqualTo(expectedQuery);
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

        private static VotingCountryPointsInRangeRanking MapRowToRanking(Dictionary<string, string> row) => new()
        {
            Rank = int.Parse(row["Rank"]),
            CountryCode = row["CountryCode"],
            CountryName = row["CountryName"],
            PointsInRange = decimal.Parse(row["PointsInRange"]),
            PointsAwardsInRange = int.Parse(row["PointsAwardsInRange"]),
            PointsAwards = int.Parse(row["PointsAwards"]),
            Broadcasts = int.Parse(row["Broadcasts"]),
            Contests = int.Parse(row["Contests"])
        };
    }
}

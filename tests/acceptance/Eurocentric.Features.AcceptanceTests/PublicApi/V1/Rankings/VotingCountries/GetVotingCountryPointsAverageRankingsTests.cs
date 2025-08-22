using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsAverageRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Rankings.VotingCountries;

public sealed class GetVotingCountryPointsAverageRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_minimum_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(competingCountryCode: "FI");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
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
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "FI",
            pageIndex: 1,
            pageSize: 3,
            descending: true);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|
            | 37   | CY          | Cyprus      | 2.333333      | 14          | 6            | 3          | 2        |
            | 35   | GR          | Greece      | 2.5           | 10          | 4            | 2          | 2        |
            | 35   | SI          | Slovenia    | 2.5           | 10          | 4            | 2          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "FI",
            pageSize: 5,
            contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName       | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|-------------|--------------|------------|----------|
            | 1    | XX          | Rest of the World | 10            | 10          | 1            | 1          | 1        |
            | 2    | EE          | Estonia           | 8.75          | 35          | 4            | 2          | 2        |
            | 3    | SE          | Sweden            | 7.75          | 31          | 4            | 2          | 2        |
            | 4    | RS          | Serbia            | 6.25          | 25          | 4            | 2          | 2        |
            | 5    | NO          | Norway            | 6             | 24          | 4            | 2          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "FI",
            pageSize: 5,
            minYear: 2023);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|
            | 1    | NO          | Norway      | 12            | 36          | 3            | 2          | 1        |
            | 1    | SE          | Sweden      | 12            | 36          | 3            | 2          | 1        |
            | 3    | EE          | Estonia     | 11            | 22          | 2            | 1          | 1        |
            | 3    | IS          | Iceland     | 11            | 22          | 2            | 1          | 1        |
            | 5    | IE          | Ireland     | 10.666667     | 32          | 3            | 2          | 1        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "FI",
            pageSize: 5,
            maxYear: 2022);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName     | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-----------------|---------------|-------------|--------------|------------|----------|
            | 1    | EE          | Estonia         | 8.25          | 33          | 4            | 2          | 1        |
            | 2    | SE          | Sweden          | 5.75          | 23          | 4            | 2          | 1        |
            | 3    | CZ          | Czechia         | 3.25          | 13          | 4            | 2          | 1        |
            | 4    | MK          | North Macedonia | 3             | 12          | 4            | 2          | 1        |
            | 4    | RS          | Serbia          | 3             | 12          | 4            | 2          | 1        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "FI",
            pageSize: 5,
            votingMethod: "Any");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName       | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|-------------|--------------|------------|----------|
            | 1    | XX          | Rest of the World | 10            | 20          | 2            | 2          | 1        |
            | 2    | EE          | Estonia           | 9.166667      | 55          | 6            | 3          | 2        |
            | 3    | SE          | Sweden            | 8.428571      | 59          | 7            | 4          | 2        |
            | 4    | NO          | Norway            | 7.2           | 36          | 5            | 3          | 2        |
            | 5    | NL          | Netherlands       | 6.4           | 32          | 5            | 3          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "FI",
            pageSize: 5,
            votingMethod: "Jury");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|
            | 1    | EE          | Estonia     | 7.666667      | 23          | 3            | 3          | 2        |
            | 2    | NO          | Norway      | 6             | 12          | 2            | 2          | 2        |
            | 2    | RS          | Serbia      | 6             | 18          | 3            | 3          | 2        |
            | 4    | SE          | Sweden      | 5.333333      | 16          | 3            | 3          | 2        |
            | 5    | IS          | Iceland     | 5             | 10          | 2            | 2          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "FI",
            pageSize: 5,
            votingMethod: "Televote");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName       | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|-------------|--------------|------------|----------|
            | 1    | SE          | Sweden            | 10.75         | 43          | 4            | 4          | 2        |
            | 2    | EE          | Estonia           | 10.666667     | 32          | 3            | 3          | 2        |
            | 3    | XX          | Rest of the World | 10            | 20          | 2            | 2          | 1        |
            | 4    | LV          | Latvia            | 8.333333      | 25          | 3            | 3          | 2        |
            | 5    | NO          | Norway            | 8             | 24          | 3            | 3          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "AU",
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
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|
            | 1    | AL          | Albania     | 12            | 12          | 1            | 1          | 1        |
            | 1    | IS          | Iceland     | 12            | 12          | 1            | 1          | 1        |
            | 3    | DK          | Denmark     | 10            | 10          | 1            | 1          | 1        |
            | 4    | EE          | Estonia     | 8.5           | 17          | 2            | 2          | 2        |
            | 5    | AT          | Austria     | 8             | 8           | 1            | 1          | 1        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            competingCountryCode: "AU",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "SI",
            pageIndex: 4,
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
            | Rank | CountryCode | CountryName    | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests |
            |------|-------------|----------------|---------------|-------------|--------------|------------|----------|
            | 17   | AM          | Armenia        | 1             | 5           | 5            | 3          | 2        |
            | 17   | BE          | Belgium        | 1             | 3           | 3            | 2          | 1        |
            | 16   | AL          | Albania        | 1.2           | 6           | 5            | 3          | 2        |
            | 15   | GB          | United Kingdom | 1.333333      | 4           | 3            | 2          | 1        |
            | 14   | LT          | Lithuania      | 1.4           | 7           | 5            | 3          | 2        |
            """);
        await euroFan.Then_the_response_query_metadata_should_match(
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any",
            competingCountryCode: "SI",
            votingMethod: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 4,
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "FI",
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
            competingCountryCode: "FI",
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
    public async Task Endpoint_should_fail_on_pageIndex_query_param_value_less_than_0(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
            competingCountryCode: "FI",
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
    public async Task Endpoint_should_fail_on_missing_competingCountryCode_query_param_value(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings();

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
        euroFan.Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(
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
        : EuroFanActorWithResponse<GetVotingCountryPointsAverageRankingsResponse>(apiDriver)
    {
        public void Given_I_want_to_obtain_a_page_of_voting_country_points_average_rankings(bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingMethod = null,
            string? competingCountryCode = null,
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
                [nameof(competingCountryCode)] = competingCountryCode,
                [nameof(maxYear)] = maxYear,
                [nameof(minYear)] = minYear,
                [nameof(contestStage)] = contestStage is not null ? Enum.Parse<QueryableContestStage>(contestStage) : null
            };

            Request = ApiDriver.RequestFactory.VotingCountryRankings.GetVotingCountryPointsAverageRankings(queryParams);
        }

        public async Task Then_the_response_rankings_page_should_match(string page)
        {
            (VotingCountryPointsAverageRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            VotingCountryPointsAverageRanking[] expected = MarkdownParser.ParseTable(page, MapRowToRanking).ToArray();

            await Assert.That(rankings).IsEquivalentTo(expected, CollectionOrdering.Matching);
        }

        public async Task Then_the_response_rankings_page_should_be_empty()
        {
            (VotingCountryPointsAverageRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(rankings).IsEmpty();
        }

        public async Task Then_the_response_query_metadata_should_match(string votingMethod = "",
            string competingCountryCode = "",
            int? maxYear = null,
            int? minYear = null,
            string contestStage = "")
        {
            (_, VotingCountryPointsAverageQueryMetadata query, _) = await Assert.That(ResponseBody).IsNotNull();

            VotingCountryPointsAverageQueryMetadata expectedQuery = new()
            {
                ContestStage = Enum.Parse<QueryableContestStage>(contestStage),
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

        private static VotingCountryPointsAverageRanking MapRowToRanking(Dictionary<string, string> row) => new()
        {
            Rank = int.Parse(row["Rank"]),
            CountryCode = row["CountryCode"],
            CountryName = row["CountryName"],
            PointsAverage = decimal.Parse(row["PointsAverage"]),
            TotalPoints = int.Parse(row["TotalPoints"]),
            PointsAwards = int.Parse(row["PointsAwards"]),
            Broadcasts = int.Parse(row["Broadcasts"]),
            Contests = int.Parse(row["Contests"])
        };
    }
}

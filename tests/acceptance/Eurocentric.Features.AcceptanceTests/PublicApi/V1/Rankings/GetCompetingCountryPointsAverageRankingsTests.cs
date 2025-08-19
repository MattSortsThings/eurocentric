using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
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
            | Rank | CountryCode | CountryName    | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden         | 7.425837      | 1552        | 209          | 4          | 2        | 40              |
            | 2    | UA          | Ukraine        | 6.475936      | 1211        | 187          | 3          | 2        | 40              |
            | 3    | FI          | Finland        | 4.320574      | 903         | 209          | 4          | 2        | 40              |
            | 4    | IL          | Israel         | 4.198473      | 550         | 131          | 3          | 2        | 39              |
            | 5    | IT          | Italy          | 4.092715      | 618         | 151          | 2          | 2        | 40              |
            | 6    | ES          | Spain          | 3.701987      | 559         | 151          | 2          | 2        | 40              |
            | 7    | NO          | Norway         | 3.556098      | 729         | 205          | 4          | 2        | 40              |
            | 8    | GR          | Greece         | 3.308271      | 440         | 133          | 3          | 2        | 40              |
            | 9    | GB          | United Kingdom | 3.245033      | 490         | 151          | 2          | 2        | 40              |
            | 10   | AU          | Australia      | 3.180952      | 668         | 210          | 4          | 2        | 40              |
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
    public async Task Endpoint_should_retrieve_rankings_with_requested_pagination(string apiVersion)
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
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 37   | ME          | Montenegro  | 0.825         | 33          | 40           | 1          | 1        | 20              |
            | 36   | SM          | San Marino  | 0.847458      | 50          | 59           | 2          | 2        | 30              |
            | 35   | MT          | Malta       | 0.862069      | 50          | 58           | 2          | 2        | 30              |
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
            pageSize: 5,
            contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 6.761589      | 1021        | 151          | 2          | 2        | 40              |
            | 2    | UA          | Ukraine     | 5.788079      | 874         | 151          | 2          | 2        | 40              |
            | 3    | IL          | Israel      | 4.958904      | 362         | 73           | 1          | 1        | 37              |
            | 4    | IT          | Italy       | 4.092715      | 618         | 151          | 2          | 2        | 40              |
            | 5    | FI          | Finland     | 3.735099      | 564         | 151          | 2          | 2        | 40              |
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
    public async Task Endpoint_should_retrieve_rankings_filtering_by_min_year(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
            pageSize: 5,
            minYear: 2023);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 7.89011       | 718         | 91           | 2          | 1        | 37              |
            | 2    | FI          | Finland     | 7.725275      | 703         | 91           | 2          | 1        | 37              |
            | 3    | IL          | Israel      | 5.373626      | 489         | 91           | 2          | 1        | 37              |
            | 4    | IT          | Italy       | 4.794521      | 350         | 73           | 1          | 1        | 37              |
            | 5    | NO          | Norway      | 4.065934      | 370         | 91           | 2          | 1        | 37              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
            pageSize: 5,
            maxYear: 2022);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine        | 8.491228      | 968         | 114          | 2          | 1        | 39              |
            | 2    | SE          | Sweden         | 7.067797      | 834         | 118          | 2          | 1        | 39              |
            | 3    | GB          | United Kingdom | 5.974359      | 466         | 78           | 1          | 1        | 39              |
            | 4    | ES          | Spain          | 5.884615      | 459         | 78           | 1          | 1        | 39              |
            | 5    | RS          | Serbia         | 4.652542      | 549         | 118          | 2          | 1        | 39              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
            pageSize: 5,
            votingCountryCode: "GB");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 8.333333      | 50          | 6            | 3          | 2        | 1               |
            | 2    | PL          | Poland      | 7.428571      | 52          | 7            | 4          | 2        | 1               |
            | 3    | LT          | Lithuania   | 7.4           | 37          | 5            | 3          | 2        | 1               |
            | 4    | IE          | Ireland     | 6             | 12          | 2            | 1          | 1        | 1               |
            | 5    | ES          | Spain       | 5             | 20          | 4            | 2          | 2        | 1               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
            pageSize: 5,
            votingMethod: "Any");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden         | 7.425837      | 1552        | 209          | 4          | 2        | 40              |
            | 2    | UA          | Ukraine        | 6.475936      | 1211        | 187          | 3          | 2        | 40              |
            | 3    | FI          | Finland        | 4.320574      | 903         | 209          | 4          | 2        | 40              |
            | 4    | IL          | Israel         | 4.198473      | 550         | 131          | 3          | 2        | 39              |
            | 5    | IT          | Italy          | 4.092715      | 618         | 151          | 2          | 2        | 40              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
            pageSize: 5,
            votingMethod: "Jury");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 8.631579      | 820         | 95           | 3          | 2        | 39              |
            | 2    | GR          | Greece      | 5.421053      | 309         | 57           | 2          | 1        | 39              |
            | 3    | NL          | Netherlands | 4.754386      | 271         | 57           | 2          | 1        | 39              |
            | 4    | IT          | Italy       | 4.453333      | 334         | 75           | 2          | 2        | 39              |
            | 5    | AU          | Australia   | 4.442105      | 422         | 95           | 3          | 2        | 39              |
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
            pageSize: 5,
            votingMethod: "Televote");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine     | 8.829787      | 830         | 94           | 3          | 2        | 40              |
            | 2    | SE          | Sweden      | 6.421053      | 732         | 114          | 4          | 2        | 40              |
            | 3    | FI          | Finland     | 5.947368      | 678         | 114          | 4          | 2        | 40              |
            | 4    | NO          | Norway      | 5.071429      | 568         | 112          | 4          | 2        | 40              |
            | 5    | MD          | Moldova     | 4.991071      | 559         | 112          | 4          | 2        | 40              |
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
    public async Task Endpoint_should_retrieve_rankings_filtering_by_multiple_facets_scenario_1(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
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
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine     | 11.222222     | 202         | 18           | 1          | 1        | 18              |
            | 2    | RS          | Serbia      | 8.7           | 174         | 20           | 1          | 1        | 20              |
            | 2    | SE          | Sweden      | 8.7           | 174         | 20           | 1          | 1        | 20              |
            | 4    | MD          | Moldova     | 7.5           | 135         | 18           | 1          | 1        | 18              |
            | 5    | CZ          | Czechia     | 6.25          | 125         | 20           | 1          | 1        | 20              |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
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
            | Rank | CountryCode | CountryName | PointsAverage | TotalPoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|-------------|--------------|------------|----------|-----------------|
            | 21   | AL          | Albania     | 0             | 0           | 1            | 1          | 1        | 1               |
            | 21   | AM          | Armenia     | 0             | 0           | 2            | 2          | 2        | 1               |
            | 21   | CY          | Cyprus      | 0             | 0           | 2            | 2          | 2        | 1               |
            | 21   | DE          | Germany     | 0             | 0           | 2            | 2          | 2        | 1               |
            | 21   | FR          | France      | 0             | 0           | 2            | 2          | 2        | 1               |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
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
    public async Task Endpoint_should_fail_on_pageIndex_query_param_value_less_than_0(string apiVersion)
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
            detail: "Query parameter 'pageIndex' value must be an integer greater than or equal to 0.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("pageIndex", -1);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_pageSize_query_param_value_less_than_1(string apiVersion)
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
            detail: "Query parameter 'pageSize' value must be an integer between 1 and 100.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("pageSize", 0);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_pageSize_query_param_value_greater_than_100(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(pageSize: 101);

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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(minYear: 2050, maxYear: 2016);

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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_average_rankings(
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

            await Assert.That(rankings).IsEquivalentTo(expected, CollectionOrdering.Matching);
        }

        public async Task Then_the_response_rankings_page_should_be_empty()
        {
            (CompetingCountryPointsAverageRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(rankings).IsEmpty();
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
            PointsAverage = decimal.Parse(row["PointsAverage"]),
            TotalPoints = int.Parse(row["TotalPoints"]),
            PointsAwards = int.Parse(row["PointsAwards"]),
            Broadcasts = int.Parse(row["Broadcasts"]),
            Contests = int.Parse(row["Contests"]),
            VotingCountries = int.Parse(row["VotingCountries"])
        };
    }
}

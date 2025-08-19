using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsShareRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Rankings;

public sealed class GetCompetingCountryPointsShareRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_minimum_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden         | 0.61882     | 1552        | 2508            | 209          | 4          | 2        | 40              |
            | 2    | UA          | Ukraine        | 0.539661    | 1211        | 2244            | 187          | 3          | 2        | 40              |
            | 3    | FI          | Finland        | 0.360048    | 903         | 2508            | 209          | 4          | 2        | 40              |
            | 4    | IL          | Israel         | 0.349873    | 550         | 1572            | 131          | 3          | 2        | 39              |
            | 5    | IT          | Italy          | 0.34106     | 618         | 1812            | 151          | 2          | 2        | 40              |
            | 6    | ES          | Spain          | 0.308499    | 559         | 1812            | 151          | 2          | 2        | 40              |
            | 7    | NO          | Norway         | 0.296341    | 729         | 2460            | 205          | 4          | 2        | 40              |
            | 8    | GR          | Greece         | 0.275689    | 440         | 1596            | 133          | 3          | 2        | 40              |
            | 9    | GB          | United Kingdom | 0.270419    | 490         | 1812            | 151          | 2          | 2        | 40              |
            | 10   | AU          | Australia      | 0.265079    | 668         | 2520            | 210          | 4          | 2        | 40              |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(
            pageIndex: 1,
            pageSize: 3,
            descending: true);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 37   | ME          | Montenegro  | 0.06875     | 33          | 480             | 40           | 1          | 1        | 20              |
            | 36   | SM          | San Marino  | 0.070621    | 50          | 708             | 59           | 2          | 2        | 30              |
            | 35   | MT          | Malta       | 0.071839    | 50          | 696             | 58           | 2          | 2        | 30              |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(
            pageSize: 5,
            contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.563466    | 1021        | 1812            | 151          | 2          | 2        | 40              |
            | 2    | UA          | Ukraine     | 0.48234     | 874         | 1812            | 151          | 2          | 2        | 40              |
            | 3    | IL          | Israel      | 0.413242    | 362         | 876             | 73           | 1          | 1        | 37              |
            | 4    | IT          | Italy       | 0.34106     | 618         | 1812            | 151          | 2          | 2        | 40              |
            | 5    | FI          | Finland     | 0.311258    | 564         | 1812            | 151          | 2          | 2        | 40              |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(
            pageSize: 5,
            minYear: 2023);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.657509    | 718         | 1092            | 91           | 2          | 1        | 37              |
            | 2    | FI          | Finland     | 0.643773    | 703         | 1092            | 91           | 2          | 1        | 37              |
            | 3    | IL          | Israel      | 0.447802    | 489         | 1092            | 91           | 2          | 1        | 37              |
            | 4    | IT          | Italy       | 0.399543    | 350         | 876             | 73           | 1          | 1        | 37              |
            | 5    | NO          | Norway      | 0.338828    | 370         | 1092            | 91           | 2          | 1        | 37              |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(
            pageSize: 5,
            maxYear: 2022);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine        | 0.707602    | 968         | 1368            | 114          | 2          | 1        | 39              |
            | 2    | SE          | Sweden         | 0.588983    | 834         | 1416            | 118          | 2          | 1        | 39              |
            | 3    | GB          | United Kingdom | 0.497863    | 466         | 936             | 78           | 1          | 1        | 39              |
            | 4    | ES          | Spain          | 0.490385    | 459         | 936             | 78           | 1          | 1        | 39              |
            | 5    | RS          | Serbia         | 0.387712    | 549         | 1416            | 118          | 2          | 1        | 39              |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(
            pageSize: 5,
            votingCountryCode: "GB");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.694444    | 50          | 72              | 6            | 3          | 2        | 1               |
            | 2    | PL          | Poland      | 0.619048    | 52          | 84              | 7            | 4          | 2        | 1               |
            | 3    | LT          | Lithuania   | 0.616667    | 37          | 60              | 5            | 3          | 2        | 1               |
            | 4    | IE          | Ireland     | 0.5         | 12          | 24              | 2            | 1          | 1        | 1               |
            | 5    | ES          | Spain       | 0.416667    | 20          | 48              | 4            | 2          | 2        | 1               |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(
            pageSize: 5,
            votingMethod: "Any");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden         | 0.61882     | 1552        | 2508            | 209          | 4          | 2        | 40              |
            | 2    | UA          | Ukraine        | 0.539661    | 1211        | 2244            | 187          | 3          | 2        | 40              |
            | 3    | FI          | Finland        | 0.360048    | 903         | 2508            | 209          | 4          | 2        | 40              |
            | 4    | IL          | Israel         | 0.349873    | 550         | 1572            | 131          | 3          | 2        | 39              |
            | 5    | IT          | Italy          | 0.34106     | 618         | 1812            | 151          | 2          | 2        | 40              |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(
            pageSize: 5,
            votingMethod: "Jury");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.719298    | 820         | 1140            | 95           | 3          | 2        | 39              |
            | 2    | GR          | Greece      | 0.451754    | 309         | 684             | 57           | 2          | 1        | 39              |
            | 3    | NL          | Netherlands | 0.396199    | 271         | 684             | 57           | 2          | 1        | 39              |
            | 4    | IT          | Italy       | 0.371111    | 334         | 900             | 75           | 2          | 2        | 39              |
            | 5    | AU          | Australia   | 0.370175    | 422         | 1140            | 95           | 3          | 2        | 39              |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(
            pageSize: 5,
            votingMethod: "Televote");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine     | 0.735816    | 830         | 1128            | 94           | 3          | 2        | 40              |
            | 2    | SE          | Sweden      | 0.535088    | 732         | 1368            | 114          | 4          | 2        | 40              |
            | 3    | FI          | Finland     | 0.495614    | 678         | 1368            | 114          | 4          | 2        | 40              |
            | 4    | NO          | Norway      | 0.422619    | 568         | 1344            | 112          | 4          | 2        | 40              |
            | 5    | MD          | Moldova     | 0.415923    | 559         | 1344            | 112          | 4          | 2        | 40              |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(
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
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 1    | UA          | Ukraine     | 0.935185    | 202         | 216             | 18           | 1          | 1        | 18              |
            | 2    | RS          | Serbia      | 0.725       | 174         | 240             | 20           | 1          | 1        | 20              |
            | 2    | SE          | Sweden      | 0.725       | 174         | 240             | 20           | 1          | 1        | 20              |
            | 4    | MD          | Moldova     | 0.625       | 135         | 216             | 18           | 1          | 1        | 18              |
            | 5    | CZ          | Czechia     | 0.520833    | 125         | 240             | 20           | 1          | 1        | 20              |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(
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
            | Rank | CountryCode | CountryName | PointsShare | TotalPoints | AvailablePoints | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|-------------|-------------|-----------------|--------------|------------|----------|-----------------|
            | 21   | AL          | Albania     | 0           | 0           | 12              | 1            | 1          | 1        | 1               |
            | 21   | AM          | Armenia     | 0           | 0           | 24              | 2            | 2          | 2        | 1               |
            | 21   | CY          | Cyprus      | 0           | 0           | 24              | 2            | 2          | 2        | 1               |
            | 21   | DE          | Germany     | 0           | 0           | 24              | 2            | 2          | 2        | 1               |
            | 21   | FR          | France      | 0           | 0           | 24              | 2            | 2          | 2        | 1               |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(pageIndex: -1);

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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(pageSize: 0);

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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(pageSize: 101);

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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(minYear: 2050, maxYear: 2016);

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
        euroFan.Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(
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
        : EuroFanActorWithResponse<GetCompetingCountryPointsShareRankingsResponse>(apiDriver)
    {
        public void Given_I_want_to_obtain_a_page_of_competing_country_points_share_rankings(bool? descending = null,
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

            Request = ApiDriver.RequestFactory.Rankings.GetCompetingCountryPointsShareRankings(queryParams);
        }

        public async Task Then_the_response_rankings_page_should_match(string page)
        {
            (CompetingCountryPointsShareRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetingCountryPointsShareRanking[] expected = MarkdownParser.ParseTable(page, MapRowToRanking).ToArray();

            await Assert.That(rankings).IsEquivalentTo(expected, CollectionOrdering.Matching);
        }

        public async Task Then_the_response_rankings_page_should_be_empty()
        {
            (CompetingCountryPointsShareRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(rankings).IsEmpty();
        }

        public async Task Then_the_response_filtering_metadata_should_match(string votingMethod = "",
            string? votingCountryCode = null,
            int? maxYear = null,
            int? minYear = null,
            string contestStage = "")
        {
            (_, CompetingCountryPointsShareFilteringMetadata filtering, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetingCountryPointsShareFilteringMetadata expectedFiltering = new()
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

        private static CompetingCountryPointsShareRanking MapRowToRanking(Dictionary<string, string> row) => new()
        {
            Rank = int.Parse(row["Rank"]),
            CountryCode = row["CountryCode"],
            CountryName = row["CountryName"],
            PointsShare = decimal.Parse(row["PointsShare"]),
            TotalPoints = int.Parse(row["TotalPoints"]),
            AvailablePoints = int.Parse(row["AvailablePoints"]),
            PointsAwards = int.Parse(row["PointsAwards"]),
            Broadcasts = int.Parse(row["Broadcasts"]),
            Contests = int.Parse(row["Contests"]),
            VotingCountries = int.Parse(row["VotingCountries"])
        };
    }
}

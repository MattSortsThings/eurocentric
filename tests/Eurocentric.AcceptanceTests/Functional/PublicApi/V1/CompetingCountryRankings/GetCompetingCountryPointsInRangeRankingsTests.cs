using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.CompetingCountryRankings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.CompetingCountryRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.CompetingCountryRankings;

[Category("public-api")]
public sealed class GetCompetingCountryPointsInRangeRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
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
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 40,
            totalPages: 4
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            pageIndex: 2,
            pageSize: 3
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 7    | NL          | Netherlands | 0.590909      | 78                  | 132          | 3          | 2        | 40              |
            | 8    | FI          | Finland     | 0.588517      | 123                 | 209          | 4          | 2        | 40              |
            | 9    | AU          | Australia   | 0.514286      | 108                 | 210          | 4          | 2        | 40              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            pageIndex: 2,
            pageSize: 3,
            descending: false,
            totalItems: 40,
            totalPages: 14
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_3(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            pageIndex: 0,
            pageSize: 5,
            descending: true
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 40   | DE          | Germany     | 0.05298       | 8                   | 151          | 2          | 2        | 40              |
            | 39   | ME          | Montenegro  | 0.125         | 5                   | 40           | 1          | 1        | 20              |
            | 38   | BG          | Bulgaria    | 0.138889      | 5                   | 36           | 1          | 1        | 18              |
            | 37   | SM          | San Marino  | 0.186441      | 11                  | 59           | 2          | 2        | 30              |
            | 36   | FR          | France      | 0.238411      | 36                  | 151          | 2          | 2        | 40              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 40,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_4(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            minYear: 2022,
            maxYear: 2022,
            contestStage: "Any",
            votingMethod: "Any",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName    | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|----------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | ES          | Spain          | 0.923077      | 72                  | 78           | 1          | 1        | 39              |
            | 2    | SE          | Sweden         | 0.898305      | 106                 | 118          | 2          | 1        | 39              |
            | 3    | GB          | United Kingdom | 0.884615      | 69                  | 78           | 1          | 1        | 39              |
            | 4    | UA          | Ukraine        | 0.868421      | 99                  | 114          | 2          | 1        | 39              |
            | 5    | RS          | Serbia         | 0.686441      | 81                  | 118          | 2          | 1        | 39              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            minYear: 2022,
            maxYear: 2022,
            contestStage: "Any",
            votingMethod: "Any",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_5(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            contestStage: "SemiFinals",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 1             | 58                  | 58           | 2          | 2        | 30              |
            | 2    | CZ          | Czechia     | 0.948276      | 55                  | 58           | 2          | 2        | 30              |
            | 3    | PT          | Portugal    | 0.944444      | 51                  | 54           | 2          | 2        | 28              |
            | 3    | UA          | Ukraine     | 0.944444      | 34                  | 36           | 1          | 1        | 18              |
            | 5    | AU          | Australia   | 0.932203      | 55                  | 59           | 2          | 2        | 30              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            contestStage: "SemiFinals",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 35,
            totalPages: 7
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_6(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            votingMethod: "Jury",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.936842      | 89                  | 95           | 3          | 2        | 39              |
            | 2    | GR          | Greece      | 0.77193       | 44                  | 57           | 2          | 1        | 39              |
            | 3    | NL          | Netherlands | 0.754386      | 43                  | 57           | 2          | 1        | 39              |
            | 4    | ES          | Spain       | 0.733333      | 55                  | 75           | 2          | 2        | 39              |
            | 5    | IT          | Italy       | 0.706667      | 53                  | 75           | 2          | 2        | 39              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            votingMethod: "Jury",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_7(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            votingMethod: "Televote",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 0.938596      | 107                 | 114          | 4          | 2        | 40              |
            | 2    | NO          | Norway      | 0.910714      | 102                 | 112          | 4          | 2        | 40              |
            | 3    | UA          | Ukraine     | 0.904255      | 85                  | 94           | 3          | 2        | 40              |
            | 4    | MD          | Moldova     | 0.776786      | 87                  | 112          | 4          | 2        | 40              |
            | 5    | FI          | Finland     | 0.710526      | 81                  | 114          | 4          | 2        | 40              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            votingMethod: "Televote",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 40,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_8(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any",
            votingCountryCode: "GB",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | SE          | Sweden      | 1             | 6                   | 6            | 3          | 2        | 1               |
            | 2    | EE          | Estonia     | 0.857143      | 6                   | 7            | 4          | 2        | 1               |
            | 2    | PL          | Poland      | 0.857143      | 6                   | 7            | 4          | 2        | 1               |
            | 4    | LT          | Lithuania   | 0.8           | 4                   | 5            | 3          | 2        | 1               |
            | 5    | ES          | Spain       | 0.75          | 3                   | 4            | 2          | 2        | 1               |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any",
            votingCountryCode: "GB",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 37,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_9(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 1,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any",
            votingCountryCode: "GB",
            pageSize: 5,
            descending: true
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 30   | DE          | Germany     | 0             | 0                   | 4            | 2          | 2        | 1               |
            | 30   | DK          | Denmark     | 0             | 0                   | 1            | 1          | 1        | 1               |
            | 30   | FR          | France      | 0             | 0                   | 4            | 2          | 2        | 1               |
            | 30   | GE          | Georgia     | 0             | 0                   | 3            | 2          | 2        | 1               |
            | 30   | HR          | Croatia     | 0             | 0                   | 2            | 1          | 1        | 1               |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 1,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any",
            votingCountryCode: "GB",
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 37,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_10(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 0,
            contestStage: "Any",
            votingMethod: "Any",
            votingCountryCode: "GB",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 1    | DE          | Germany     | 1             | 4                   | 4            | 2          | 2        | 1               |
            | 1    | DK          | Denmark     | 1             | 1                   | 1            | 1          | 1        | 1               |
            | 1    | FR          | France      | 1             | 4                   | 4            | 2          | 2        | 1               |
            | 1    | GE          | Georgia     | 1             | 3                   | 3            | 2          | 2        | 1               |
            | 1    | HR          | Croatia     | 1             | 2                   | 2            | 1          | 1        | 1               |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 0,
            maxPoints: 0,
            contestStage: "Any",
            votingMethod: "Any",
            votingCountryCode: "GB",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 37,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_11(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 0,
            contestStage: "Any",
            votingMethod: "Any",
            votingCountryCode: "GB",
            pageSize: 5,
            descending: true
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests | VotingCountries |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|-----------------|
            | 37   | SE          | Sweden      | 0             | 0                   | 6            | 3          | 2        | 1               |
            | 35   | EE          | Estonia     | 0.142857      | 1                   | 7            | 4          | 2        | 1               |
            | 35   | PL          | Poland      | 0.142857      | 1                   | 7            | 4          | 2        | 1               |
            | 34   | LT          | Lithuania   | 0.2           | 1                   | 5            | 3          | 2        | 1               |
            | 32   | ES          | Spain       | 0.25          | 1                   | 4            | 2          | 2        | 1               |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 0,
            maxPoints: 0,
            contestStage: "Any",
            votingMethod: "Any",
            votingCountryCode: "GB",
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 37,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_rankings_page_when_no_data_matches_query(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 12,
            minYear: 1066,
            maxYear: 1963,
            votingCountryCode: "ZZ"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 0,
            maxPoints: 12,
            minYear: 1066,
            maxYear: 1963,
            votingCountryCode: "ZZ",
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 0,
            totalPages: 0
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_illegal_page_index_value(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            pageIndex: -1,
            minPoints: 0,
            maxPoints: 12
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal page index value",
            detail: "Page index value must be a non-negative integer."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "pageIndex", value: -1);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_illegal_page_size_value_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            pageSize: 0,
            minPoints: 0,
            maxPoints: 12
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal page size value",
            detail: "Page size value must be an integer between 1 and 100."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "pageSize", value: 0);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_illegal_page_size_value_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            pageSize: 101,
            minPoints: 0,
            maxPoints: 12
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal page size value",
            detail: "Page size value must be an integer between 1 and 100."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "pageSize", value: 101);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_illegal_contest_year_range(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minYear: 2023,
            maxYear: 2022,
            minPoints: 0,
            maxPoints: 12
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal contest year range",
            detail: "Maximum contest year must be greater than or equal to minimum contest year."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "minYear", value: 2023);
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "maxYear", value: 2022);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_illegal_voting_country_code_value(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            votingCountryCode: "!",
            minPoints: 0,
            maxPoints: 12
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal voting country code value",
            detail: "Voting country code value must be a string of 2 upper-case letters."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "votingCountryCode", value: "!");
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_illegal_points_value_range(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            minPoints: 12,
            maxPoints: 1
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal points value range",
            detail: "Maximum points value must be greater than or equal to minimum points value."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "minPoints", value: 12);
        await euroFan.Then_the_response_problem_details_extensions_should_include(key: "maxPoints", value: 1);
    }

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetCompetingCountryPointsInRangeRankingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_a_page_of_competing_country_points_in_range_rankings(
            bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingMethod = null,
            string? votingCountryCode = null,
            string? contestStage = null,
            int? maxYear = null,
            int? minYear = null,
            int maxPoints = 0,
            int minPoints = 0
        )
        {
            Dictionary<string, object?> queryParams = new()
            {
                { nameof(descending), descending },
                { nameof(pageSize), pageSize },
                { nameof(pageIndex), pageIndex },
                { nameof(votingMethod), votingMethod },
                { nameof(votingCountryCode), votingCountryCode },
                { nameof(contestStage), contestStage },
                { nameof(maxYear), maxYear },
                { nameof(minYear), minYear },
                { nameof(maxPoints), maxPoints },
                { nameof(minPoints), minPoints },
            };

            Request = Kernel.Requests.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings(queryParams);
        }

        public async Task Then_the_retrieved_rankings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Rankings).IsEmpty();

        public async Task Then_the_retrieved_rankings_in_order_should_match(string table)
        {
            CompetingCountryPointsInRangeRanking[] expected = MarkdownParser.ParseTable(table, MapToRanking);

            await Assert
                .That(SuccessResponse?.Data?.Rankings)
                .IsEquivalentTo(expected, new RankingEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_metadata_should_match(
            int totalPages = -1,
            int totalItems = -1,
            bool descending = true,
            int pageSize = -1,
            int pageIndex = -1,
            string? votingMethod = null,
            string? votingCountryCode = null,
            string? contestStage = null,
            int? maxYear = null,
            int? minYear = null,
            int maxPoints = 0,
            int minPoints = 0
        )
        {
            ContestStageFilter? expectedContestStage = contestStage.ToNullableContestStageFilter();
            VotingMethodFilter? expectedVotingMethod = votingMethod.ToNullableVotingMethodFilter();

            await Assert
                .That(SuccessResponse?.Data?.Metadata)
                .HasProperty(cc => cc.MinPoints, minPoints)
                .And.HasProperty(cc => cc.MaxPoints, maxPoints)
                .And.HasProperty(cc => cc.MinYear, minYear)
                .And.HasProperty(cc => cc.MaxYear, maxYear)
                .And.HasProperty(cc => cc.ContestStage, expectedContestStage)
                .And.HasProperty(cc => cc.VotingCountryCode, votingCountryCode)
                .And.HasProperty(cc => cc.VotingMethod, expectedVotingMethod)
                .And.HasProperty(cc => cc.PageIndex, pageIndex)
                .And.HasProperty(cc => cc.PageSize, pageSize)
                .And.HasProperty(cc => cc.Descending, descending)
                .And.HasProperty(cc => cc.TotalItems, totalItems)
                .And.HasProperty(cc => cc.TotalPages, totalPages);
        }

        private static CompetingCountryPointsInRangeRanking MapToRanking(Dictionary<string, string> row)
        {
            return new CompetingCountryPointsInRangeRanking
            {
                Rank = int.Parse(row["Rank"]),
                CountryCode = row["CountryCode"],
                CountryName = row["CountryName"],
                PointsInRange = decimal.Parse(row["PointsInRange"]),
                PointsAwardsInRange = int.Parse(row["PointsAwardsInRange"]),
                PointsAwards = int.Parse(row["PointsAwards"]),
                Broadcasts = int.Parse(row["Broadcasts"]),
                Contests = int.Parse(row["Contests"]),
                VotingCountries = int.Parse(row["VotingCountries"]),
            };
        }

        private sealed class RankingEqualityComparer : IEqualityComparer<CompetingCountryPointsInRangeRanking>
        {
            public bool Equals(CompetingCountryPointsInRangeRanking? x, CompetingCountryPointsInRangeRanking? y)
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
                    && x.CountryCode.Equals(y.CountryCode, StringComparison.Ordinal)
                    && x.CountryName.Equals(y.CountryName, StringComparison.Ordinal)
                    && x.PointsInRange == y.PointsInRange
                    && x.PointsAwardsInRange == y.PointsAwardsInRange
                    && x.PointsAwards == y.PointsAwards
                    && x.Broadcasts == y.Broadcasts
                    && x.Contests == y.Contests
                    && x.VotingCountries == y.VotingCountries;
            }

            public int GetHashCode(CompetingCountryPointsInRangeRanking obj)
            {
                HashCode hashCode = new();
                hashCode.Add(obj.Rank);
                hashCode.Add(obj.CountryCode);
                hashCode.Add(obj.CountryName);
                hashCode.Add(obj.PointsInRange);
                hashCode.Add(obj.PointsAwardsInRange);
                hashCode.Add(obj.PointsAwards);
                hashCode.Add(obj.Broadcasts);
                hashCode.Add(obj.Contests);
                hashCode.Add(obj.VotingCountries);

                return hashCode.ToHashCode();
            }
        }
    }
}

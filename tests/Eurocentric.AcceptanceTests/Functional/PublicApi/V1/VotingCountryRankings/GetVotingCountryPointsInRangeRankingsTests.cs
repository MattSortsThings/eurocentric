using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.VotingCountryRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.VotingCountryRankings;

[Category("public-api")]
public sealed class GetVotingCountryPointsInRangeRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
            minPoints: 1,
            maxPoints: 12
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
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
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
            | Rank | CountryCode | CountryName       | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|---------------------|--------------|------------|----------|
            | 7    | GE          | Georgia           | 0.833333      | 5                   | 6            | 3          | 2        |
            | 8    | AZ          | Azerbaijan        | 0.714286      | 5                   | 7            | 4          | 2        |
            | 9    | BE          | Belgium           | 0.666667      | 4                   | 6            | 3          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
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
            | Rank | CountryCode | CountryName    | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|----------------|---------------|---------------------|--------------|------------|----------|
            | 40   | BG          | Bulgaria       | 0             | 0                   | 2            | 1          | 1        |
            | 37   | GR          | Greece         | 0.25          | 1                   | 4            | 2          | 2        |
            | 37   | ME          | Montenegro     | 0.25          | 1                   | 4            | 2          | 1        |
            | 37   | SI          | Slovenia       | 0.25          | 1                   | 4            | 2          | 2        |
            | 36   | GB          | United Kingdom | 0.333333      | 2                   | 6            | 3          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
            minPoints: 8,
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
            | Rank | CountryCode | CountryName     | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-----------------|---------------|---------------------|--------------|------------|----------|
            | 1    | EE          | Estonia         | 0.75          | 3                   | 4            | 2          | 1        |
            | 2    | CZ          | Czechia         | 0.25          | 1                   | 4            | 2          | 1        |
            | 2    | MK          | North Macedonia | 0.25          | 1                   | 4            | 2          | 1        |
            | 2    | SE          | Sweden          | 0.25          | 1                   | 4            | 2          | 1        |
            | 5    | AL          | Albania         | 0             | 0                   | 2            | 1          | 1        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
            minPoints: 8,
            maxPoints: 12,
            minYear: 2022,
            maxYear: 2022,
            contestStage: "Any",
            votingMethod: "Any",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 39,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_5(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
            minPoints: 12,
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
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|
            | 1    | HR          | Croatia     | 1             | 1                   | 1            | 1          | 1        |
            | 1    | LV          | Latvia      | 1             | 1                   | 1            | 1          | 1        |
            | 1    | NO          | Norway      | 1             | 1                   | 1            | 1          | 1        |
            | 4    | SE          | Sweden      | 0.666667      | 2                   | 3            | 2          | 2        |
            | 5    | EE          | Estonia     | 0.5           | 1                   | 2            | 1          | 1        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
            minPoints: 12,
            maxPoints: 12,
            contestStage: "SemiFinals",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 30,
            totalPages: 6
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_6(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
            minPoints: 10,
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
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|
            | 1    | IS          | Iceland     | 0.5           | 1                   | 2            | 2          | 2        |
            | 1    | NL          | Netherlands | 0.5           | 1                   | 2            | 2          | 2        |
            | 1    | NO          | Norway      | 0.5           | 1                   | 2            | 2          | 2        |
            | 4    | EE          | Estonia     | 0.333333      | 1                   | 3            | 3          | 2        |
            | 4    | SE          | Sweden      | 0.333333      | 1                   | 3            | 3          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
            minPoints: 10,
            maxPoints: 12,
            votingMethod: "Jury",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 39,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_7(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "FI",
            minPoints: 10,
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
            | Rank | CountryCode | CountryName       | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|---------------------|--------------|------------|----------|
            | 1    | XX          | Rest of the World | 1             | 2                   | 2            | 2          | 1        |
            | 2    | SE          | Sweden            | 0.75          | 3                   | 4            | 4          | 2        |
            | 3    | EE          | Estonia           | 0.666667      | 2                   | 3            | 3          | 2        |
            | 3    | HR          | Croatia           | 0.666667      | 2                   | 3            | 3          | 2        |
            | 3    | LV          | Latvia            | 0.666667      | 2                   | 3            | 3          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
            minPoints: 10,
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "GB",
            minPoints: 1,
            maxPoints: 12,
            contestStage: "GrandFinal",
            votingMethod: "Any",
            pageSize: 5,
            descending: false
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|
            | 1    | BG          | Bulgaria    | 1             | 2                   | 2            | 1          | 1        |
            | 1    | UA          | Ukraine     | 1             | 4                   | 4            | 2          | 2        |
            | 3    | DK          | Denmark     | 0.75          | 3                   | 4            | 2          | 2        |
            | 3    | FI          | Finland     | 0.75          | 3                   | 4            | 2          | 2        |
            | 3    | IE          | Ireland     | 0.75          | 3                   | 4            | 2          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "GB",
            minPoints: 1,
            maxPoints: 12,
            contestStage: "GrandFinal",
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
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_9(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "GB",
            minPoints: 1,
            maxPoints: 12,
            contestStage: "GrandFinal",
            votingMethod: "Any",
            pageSize: 5,
            descending: true
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName       | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|---------------------|--------------|------------|----------|
            | 39   | HR          | Croatia           | 0             | 0                   | 4            | 2          | 2        |
            | 39   | XX          | Rest of the World | 0             | 0                   | 1            | 1          | 1        |
            | 34   | AM          | Armenia           | 0.25          | 1                   | 4            | 2          | 2        |
            | 34   | AU          | Australia         | 0.25          | 1                   | 4            | 2          | 2        |
            | 34   | GR          | Greece            | 0.25          | 1                   | 4            | 2          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "GB",
            minPoints: 1,
            maxPoints: 12,
            contestStage: "GrandFinal",
            votingMethod: "Any",
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 40,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_10(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "GB",
            minPoints: 0,
            maxPoints: 0,
            contestStage: "GrandFinal",
            votingMethod: "Any",
            pageSize: 5,
            descending: false
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName       | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------------|---------------|---------------------|--------------|------------|----------|
            | 1    | HR          | Croatia           | 1             | 4                   | 4            | 2          | 2        |
            | 1    | XX          | Rest of the World | 1             | 1                   | 1            | 1          | 1        |
            | 3    | AM          | Armenia           | 0.75          | 3                   | 4            | 2          | 2        |
            | 3    | AU          | Australia         | 0.75          | 3                   | 4            | 2          | 2        |
            | 3    | GR          | Greece            | 0.75          | 3                   | 4            | 2          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "GB",
            minPoints: 0,
            maxPoints: 0,
            contestStage: "GrandFinal",
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
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_11(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "GB",
            minPoints: 0,
            maxPoints: 0,
            contestStage: "GrandFinal",
            votingMethod: "Any",
            pageSize: 5,
            descending: true
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | CountryCode | CountryName | PointsInRange | PointsAwardsInRange | PointsAwards | Broadcasts | Contests |
            |------|-------------|-------------|---------------|---------------------|--------------|------------|----------|
            | 39   | BG          | Bulgaria    | 0             | 0                   | 2            | 1          | 1        |
            | 39   | UA          | Ukraine     | 0             | 0                   | 4            | 2          | 2        |
            | 34   | DK          | Denmark     | 0.25          | 1                   | 4            | 2          | 2        |
            | 34   | FI          | Finland     | 0.25          | 1                   | 4            | 2          | 2        |
            | 34   | IE          | Ireland     | 0.25          | 1                   | 4            | 2          | 2        |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "GB",
            minPoints: 0,
            maxPoints: 0,
            contestStage: "GrandFinal",
            votingMethod: "Any",
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 40,
            totalPages: 8
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_rankings_page_when_no_data_matches_query(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "ZZ",
            minPoints: 0,
            maxPoints: 12,
            minYear: 1066,
            maxYear: 1963
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "ZZ",
            minPoints: 0,
            maxPoints: 12,
            minYear: 1066,
            maxYear: 1963,
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            pageIndex: -1,
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            pageSize: 0,
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            pageSize: 101,
            competingCountryCode: "FI",
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
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            minYear: 2023,
            maxYear: 2022,
            competingCountryCode: "FI",
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
    public async Task Should_fail_on_illegal_competing_country_code_value(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            competingCountryCode: "!",
            minPoints: 0,
            maxPoints: 12
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code(422);
        await euroFan.Then_the_response_problem_details_should_match(
            status: 422,
            title: "Illegal competing country code value",
            detail: "Competing country code value must be a string of 2 upper-case letters."
        );
        await euroFan.Then_the_response_problem_details_extensions_should_include(
            key: "competingCountryCode",
            value: "!"
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_illegal_points_value_range(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            minPoints: 12,
            maxPoints: 1,
            competingCountryCode: "FI"
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

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetVotingCountryPointsInRangeRankingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_a_page_of_voting_country_points_in_range_rankings(
            bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingMethod = null,
            string competingCountryCode = "",
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
                { nameof(competingCountryCode), competingCountryCode },
                { nameof(contestStage), contestStage },
                { nameof(maxYear), maxYear },
                { nameof(minYear), minYear },
                { nameof(maxPoints), maxPoints },
                { nameof(minPoints), minPoints },
            };

            Request = Kernel.Requests.VotingCountryRankings.GetVotingCountryPointsInRangeRankings(queryParams);
        }

        public async Task Then_the_retrieved_rankings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Rankings).IsEmpty();

        public async Task Then_the_retrieved_rankings_in_order_should_match(string table)
        {
            VotingCountryPointsInRangeRanking[] expected = MarkdownParser.ParseTable(table, MapToRanking);

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
            string competingCountryCode = "",
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
                .HasProperty(vc => vc.MinPoints, minPoints)
                .And.HasProperty(vc => vc.MaxPoints, maxPoints)
                .And.HasProperty(vc => vc.MinYear, minYear)
                .And.HasProperty(vc => vc.MaxYear, maxYear)
                .And.HasProperty(vc => vc.ContestStage, expectedContestStage)
                .And.HasProperty(vc => vc.CompetingCountryCode, competingCountryCode)
                .And.HasProperty(vc => vc.VotingMethod, expectedVotingMethod)
                .And.HasProperty(vc => vc.PageIndex, pageIndex)
                .And.HasProperty(vc => vc.PageSize, pageSize)
                .And.HasProperty(vc => vc.Descending, descending)
                .And.HasProperty(vc => vc.TotalItems, totalItems)
                .And.HasProperty(vc => vc.TotalPages, totalPages);
        }

        private static VotingCountryPointsInRangeRanking MapToRanking(Dictionary<string, string> row)
        {
            return new VotingCountryPointsInRangeRanking
            {
                Rank = int.Parse(row["Rank"]),
                CountryCode = row["CountryCode"],
                CountryName = row["CountryName"],
                PointsInRange = decimal.Parse(row["PointsInRange"]),
                PointsAwardsInRange = int.Parse(row["PointsAwardsInRange"]),
                PointsAwards = int.Parse(row["PointsAwards"]),
                Broadcasts = int.Parse(row["Broadcasts"]),
                Contests = int.Parse(row["Contests"]),
            };
        }

        private sealed class RankingEqualityComparer : IEqualityComparer<VotingCountryPointsInRangeRanking>
        {
            public bool Equals(VotingCountryPointsInRangeRanking? x, VotingCountryPointsInRangeRanking? y)
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
                    && x.Contests == y.Contests;
            }

            public int GetHashCode(VotingCountryPointsInRangeRanking obj) =>
                HashCode.Combine(
                    obj.Rank,
                    obj.CountryCode,
                    obj.CountryName,
                    obj.PointsInRange,
                    obj.PointsAwardsInRange,
                    obj.PointsAwards,
                    obj.Broadcasts,
                    obj.Contests
                );
        }
    }
}

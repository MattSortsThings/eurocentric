using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.Listings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.Listings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.Listings;

[Category("public-api")]
public sealed class GetCompetingCountryPointsListingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_points_listings(
            contestYear: 2023,
            contestStage: "GrandFinal",
            competingCountryCode: "FI"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "GrandFinal",
            competingCountryCode: "FI"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_in_order_should_match(
            """
            | PointsValue | VotingCountryCode | VotingCountryName |
            |-------------|-------------------|-------------------|
            | 12          | NO                | Norway            |
            | 12          | SE                | Sweden            |
            | 10          | EE                | Estonia           |
            | 10          | IS                | Iceland           |
            | 10          | NL                | Netherlands       |
            | 8           | AM                | Armenia           |
            | 8           | AT                | Austria           |
            | 8           | DK                | Denmark           |
            | 8           | FR                | France            |
            | 8           | IE                | Ireland           |
            | 8           | IL                | Israel            |
            | 8           | MT                | Malta             |
            | 7           | HR                | Croatia           |
            | 7           | RS                | Serbia            |
            | 5           | AU                | Australia         |
            | 5           | BE                | Belgium           |
            | 5           | CZ                | Czechia           |
            | 3           | AZ                | Azerbaijan        |
            | 3           | CY                | Cyprus            |
            | 3           | LT                | Lithuania         |
            | 1           | ES                | Spain             |
            | 1           | GE                | Georgia           |
            | 0           | AL                | Albania           |
            | 0           | CH                | Switzerland       |
            | 0           | DE                | Germany           |
            | 0           | GB                | United Kingdom    |
            | 0           | GR                | Greece            |
            | 0           | IT                | Italy             |
            | 0           | LV                | Latvia            |
            | 0           | MD                | Moldova           |
            | 0           | PL                | Poland            |
            | 0           | PT                | Portugal          |
            | 0           | RO                | Romania           |
            | 0           | SI                | Slovenia          |
            | 0           | SM                | San Marino        |
            | 0           | UA                | Ukraine           |
            """
        );
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | VotingCountryCode | VotingCountryName |
            |-------------|-------------------|-------------------|
            | 12          | AT                | Austria           |
            | 12          | AU                | Australia         |
            | 12          | BE                | Belgium           |
            | 12          | DE                | Germany           |
            | 12          | DK                | Denmark           |
            | 12          | EE                | Estonia           |
            | 12          | ES                | Spain             |
            | 12          | GB                | United Kingdom    |
            | 12          | IE                | Ireland           |
            | 12          | IL                | Israel            |
            | 12          | IS                | Iceland           |
            | 12          | LT                | Lithuania         |
            | 12          | LV                | Latvia            |
            | 12          | NL                | Netherlands       |
            | 12          | NO                | Norway            |
            | 12          | RS                | Serbia            |
            | 12          | SE                | Sweden            |
            | 12          | SM                | San Marino        |
            | 10          | CZ                | Czechia           |
            | 10          | GR                | Greece            |
            | 10          | HR                | Croatia           |
            | 10          | PL                | Poland            |
            | 10          | PT                | Portugal          |
            | 10          | RO                | Romania           |
            | 10          | SI                | Slovenia          |
            | 10          | UA                | Ukraine           |
            | 10          | XX                | Rest of the World |
            | 8           | AZ                | Azerbaijan        |
            | 8           | CH                | Switzerland       |
            | 8           | GE                | Georgia           |
            | 8           | MT                | Malta             |
            | 7           | CY                | Cyprus            |
            | 7           | MD                | Moldova           |
            | 6           | AL                | Albania           |
            | 6           | AM                | Armenia           |
            | 6           | FR                | France            |
            | 6           | IT                | Italy             |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_points_listings(
            contestYear: 2022,
            contestStage: "GrandFinal",
            competingCountryCode: "FI"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2022,
            contestStage: "GrandFinal",
            competingCountryCode: "FI"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_in_order_should_match(
            """
            | PointsValue | VotingCountryCode | VotingCountryName |
            |-------------|-------------------|-------------------|
            | 6           | RS                | Serbia            |
            | 5           | EE                | Estonia           |
            | 1           | IT                | Italy             |
            | 0           | AL                | Albania           |
            | 0           | AM                | Armenia           |
            | 0           | AT                | Austria           |
            | 0           | AU                | Australia         |
            | 0           | AZ                | Azerbaijan        |
            | 0           | BE                | Belgium           |
            | 0           | BG                | Bulgaria          |
            | 0           | CH                | Switzerland       |
            | 0           | CY                | Cyprus            |
            | 0           | CZ                | Czechia           |
            | 0           | DE                | Germany           |
            | 0           | DK                | Denmark           |
            | 0           | ES                | Spain             |
            | 0           | FR                | France            |
            | 0           | GB                | United Kingdom    |
            | 0           | GE                | Georgia           |
            | 0           | GR                | Greece            |
            | 0           | HR                | Croatia           |
            | 0           | IE                | Ireland           |
            | 0           | IL                | Israel            |
            | 0           | IS                | Iceland           |
            | 0           | LT                | Lithuania         |
            | 0           | LV                | Latvia            |
            | 0           | MD                | Moldova           |
            | 0           | ME                | Montenegro        |
            | 0           | MK                | North Macedonia   |
            | 0           | MT                | Malta             |
            | 0           | NL                | Netherlands       |
            | 0           | NO                | Norway            |
            | 0           | PL                | Poland            |
            | 0           | PT                | Portugal          |
            | 0           | RO                | Romania           |
            | 0           | SE                | Sweden            |
            | 0           | SI                | Slovenia          |
            | 0           | SM                | San Marino        |
            | 0           | UA                | Ukraine           |
            """
        );
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | VotingCountryCode | VotingCountryName |
            |-------------|-------------------|-------------------|
            | 8           | EE                | Estonia           |
            | 7           | SE                | Sweden            |
            | 4           | UA                | Ukraine           |
            | 2           | AL                | Albania           |
            | 2           | GE                | Georgia           |
            | 1           | CZ                | Czechia           |
            | 1           | LV                | Latvia            |
            | 1           | MT                | Malta             |
            | 0           | AM                | Armenia           |
            | 0           | AT                | Austria           |
            | 0           | AU                | Australia         |
            | 0           | AZ                | Azerbaijan        |
            | 0           | BE                | Belgium           |
            | 0           | BG                | Bulgaria          |
            | 0           | CH                | Switzerland       |
            | 0           | CY                | Cyprus            |
            | 0           | DE                | Germany           |
            | 0           | DK                | Denmark           |
            | 0           | ES                | Spain             |
            | 0           | FR                | France            |
            | 0           | GB                | United Kingdom    |
            | 0           | GR                | Greece            |
            | 0           | HR                | Croatia           |
            | 0           | IE                | Ireland           |
            | 0           | IL                | Israel            |
            | 0           | IS                | Iceland           |
            | 0           | IT                | Italy             |
            | 0           | LT                | Lithuania         |
            | 0           | MD                | Moldova           |
            | 0           | ME                | Montenegro        |
            | 0           | MK                | North Macedonia   |
            | 0           | NL                | Netherlands       |
            | 0           | NO                | Norway            |
            | 0           | PL                | Poland            |
            | 0           | PT                | Portugal          |
            | 0           | RO                | Romania           |
            | 0           | RS                | Serbia            |
            | 0           | SI                | Slovenia          |
            | 0           | SM                | San Marino        |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_3(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_points_listings(
            contestYear: 2023,
            contestStage: "SemiFinal1",
            competingCountryCode: "FI"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "SemiFinal1",
            competingCountryCode: "FI"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | VotingCountryCode | VotingCountryName |
            |-------------|-------------------|-------------------|
            | 12          | DE                | Germany           |
            | 12          | HR                | Croatia           |
            | 12          | IE                | Ireland           |
            | 12          | IL                | Israel            |
            | 12          | LV                | Latvia            |
            | 12          | NO                | Norway            |
            | 12          | SE                | Sweden            |
            | 10          | CH                | Switzerland       |
            | 10          | NL                | Netherlands       |
            | 10          | PT                | Portugal          |
            | 10          | RS                | Serbia            |
            | 10          | XX                | Rest of the World |
            | 8           | AZ                | Azerbaijan        |
            | 8           | CZ                | Czechia           |
            | 7           | FR                | France            |
            | 7           | IT                | Italy             |
            | 7           | MT                | Malta             |
            | 6           | MD                | Moldova           |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_4(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_points_listings(
            contestYear: 2023,
            contestStage: "GrandFinal",
            competingCountryCode: "SI"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "GrandFinal",
            competingCountryCode: "SI"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_in_order_should_match(
            """
            | PointsValue | VotingCountryCode | VotingCountryName |
            |-------------|-------------------|-------------------|
            | 12          | RS                | Serbia            |
            | 6           | AT                | Austria           |
            | 6           | CZ                | Czechia           |
            | 5           | HR                | Croatia           |
            | 3           | UA                | Ukraine           |
            | 1           | GB                | United Kingdom    |
            | 0           | AL                | Albania           |
            | 0           | AM                | Armenia           |
            | 0           | AU                | Australia         |
            | 0           | AZ                | Azerbaijan        |
            | 0           | BE                | Belgium           |
            | 0           | CH                | Switzerland       |
            | 0           | CY                | Cyprus            |
            | 0           | DE                | Germany           |
            | 0           | DK                | Denmark           |
            | 0           | EE                | Estonia           |
            | 0           | ES                | Spain             |
            | 0           | FI                | Finland           |
            | 0           | FR                | France            |
            | 0           | GE                | Georgia           |
            | 0           | GR                | Greece            |
            | 0           | IE                | Ireland           |
            | 0           | IL                | Israel            |
            | 0           | IS                | Iceland           |
            | 0           | IT                | Italy             |
            | 0           | LT                | Lithuania         |
            | 0           | LV                | Latvia            |
            | 0           | MD                | Moldova           |
            | 0           | MT                | Malta             |
            | 0           | NL                | Netherlands       |
            | 0           | NO                | Norway            |
            | 0           | PL                | Poland            |
            | 0           | PT                | Portugal          |
            | 0           | RO                | Romania           |
            | 0           | SE                | Sweden            |
            | 0           | SM                | San Marino        |
            """
        );
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | VotingCountryCode | VotingCountryName |
            |-------------|-------------------|-------------------|
            | 12          | HR                | Croatia           |
            | 8           | RS                | Serbia            |
            | 7           | FI                | Finland           |
            | 5           | AZ                | Azerbaijan        |
            | 3           | CZ                | Czechia           |
            | 2           | AL                | Albania           |
            | 2           | LV                | Latvia            |
            | 2           | PL                | Poland            |
            | 2           | RO                | Romania           |
            | 1           | AU                | Australia         |
            | 1           | EE                | Estonia           |
            | 0           | AM                | Armenia           |
            | 0           | AT                | Austria           |
            | 0           | BE                | Belgium           |
            | 0           | CH                | Switzerland       |
            | 0           | CY                | Cyprus            |
            | 0           | DE                | Germany           |
            | 0           | DK                | Denmark           |
            | 0           | ES                | Spain             |
            | 0           | FR                | France            |
            | 0           | GB                | United Kingdom    |
            | 0           | GE                | Georgia           |
            | 0           | GR                | Greece            |
            | 0           | IE                | Ireland           |
            | 0           | IL                | Israel            |
            | 0           | IS                | Iceland           |
            | 0           | IT                | Italy             |
            | 0           | LT                | Lithuania         |
            | 0           | MD                | Moldova           |
            | 0           | MT                | Malta             |
            | 0           | NL                | Netherlands       |
            | 0           | NO                | Norway            |
            | 0           | PT                | Portugal          |
            | 0           | SE                | Sweden            |
            | 0           | SM                | San Marino        |
            | 0           | UA                | Ukraine           |
            | 0           | XX                | Rest of the World |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_5(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_points_listings(
            contestYear: 2023,
            contestStage: "SemiFinal2",
            competingCountryCode: "SM"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "SemiFinal2",
            competingCountryCode: "SM"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | VotingCountryCode | VotingCountryName |
            |-------------|-------------------|-------------------|
            | 0           | AL                | Albania           |
            | 0           | AM                | Armenia           |
            | 0           | AT                | Austria           |
            | 0           | AU                | Australia         |
            | 0           | BE                | Belgium           |
            | 0           | CY                | Cyprus            |
            | 0           | DK                | Denmark           |
            | 0           | EE                | Estonia           |
            | 0           | ES                | Spain             |
            | 0           | GB                | United Kingdom    |
            | 0           | GE                | Georgia           |
            | 0           | GR                | Greece            |
            | 0           | IS                | Iceland           |
            | 0           | LT                | Lithuania         |
            | 0           | PL                | Poland            |
            | 0           | RO                | Romania           |
            | 0           | SI                | Slovenia          |
            | 0           | UA                | Ukraine           |
            | 0           | XX                | Rest of the World |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_6(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_points_listings(
            contestYear: 2022,
            contestStage: "SemiFinal1",
            competingCountryCode: "NO"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2022,
            contestStage: "SemiFinal1",
            competingCountryCode: "NO"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_in_order_should_match(
            """
            | PointsValue | VotingCountryCode | VotingCountryName |
            |-------------|-------------------|-------------------|
            | 12          | IS                | Iceland           |
            | 7           | BG                | Bulgaria          |
            | 7           | CH                | Switzerland       |
            | 7           | LV                | Latvia            |
            | 7           | MD                | Moldova           |
            | 6           | AM                | Armenia           |
            | 6           | NL                | Netherlands       |
            | 4           | AT                | Austria           |
            | 3           | DK                | Denmark           |
            | 3           | FR                | France            |
            | 3           | LT                | Lithuania         |
            | 2           | AL                | Albania           |
            | 2           | PT                | Portugal          |
            | 2           | SI                | Slovenia          |
            | 1           | GR                | Greece            |
            | 1           | UA                | Ukraine           |
            | 0           | HR                | Croatia           |
            | 0           | IT                | Italy             |
            """
        );
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | VotingCountryCode | VotingCountryName |
            |-------------|-------------------|-------------------|
            | 10          | DK                | Denmark           |
            | 10          | IS                | Iceland           |
            | 10          | MD                | Moldova           |
            | 7           | AT                | Austria           |
            | 7           | GR                | Greece            |
            | 7           | HR                | Croatia           |
            | 7           | LV                | Latvia            |
            | 7           | NL                | Netherlands       |
            | 7           | PT                | Portugal          |
            | 7           | UA                | Ukraine           |
            | 5           | IT                | Italy             |
            | 5           | LT                | Lithuania         |
            | 4           | BG                | Bulgaria          |
            | 4           | SI                | Slovenia          |
            | 3           | AM                | Armenia           |
            | 2           | AL                | Albania           |
            | 2           | FR                | France            |
            | 0           | CH                | Switzerland       |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_7(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_points_listings(
            contestYear: 2022,
            contestStage: "SemiFinal2",
            competingCountryCode: "GE"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2022,
            contestStage: "SemiFinal2",
            competingCountryCode: "GE"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_in_order_should_match(
            """
            | PointsValue | VotingCountryCode | VotingCountryName |
            |-------------|-------------------|-------------------|
            | 5           | CZ                | Czechia           |
            | 3           | DE                | Germany           |
            | 3           | EE                | Estonia           |
            | 1           | AU                | Australia         |
            | 1           | MT                | Malta             |
            | 0           | AZ                | Azerbaijan        |
            | 0           | BE                | Belgium           |
            | 0           | CY                | Cyprus            |
            | 0           | ES                | Spain             |
            | 0           | FI                | Finland           |
            | 0           | GB                | United Kingdom    |
            | 0           | IE                | Ireland           |
            | 0           | IL                | Israel            |
            | 0           | ME                | Montenegro        |
            | 0           | MK                | North Macedonia   |
            | 0           | PL                | Poland            |
            | 0           | RO                | Romania           |
            | 0           | RS                | Serbia            |
            | 0           | SE                | Sweden            |
            | 0           | SM                | San Marino        |
            """
        );
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | VotingCountryCode | VotingCountryName |
            |-------------|-------------------|-------------------|
            | 5           | IL                | Israel            |
            | 2           | FI                | Finland           |
            | 1           | AU                | Australia         |
            | 1           | EE                | Estonia           |
            | 0           | AZ                | Azerbaijan        |
            | 0           | BE                | Belgium           |
            | 0           | CY                | Cyprus            |
            | 0           | CZ                | Czechia           |
            | 0           | DE                | Germany           |
            | 0           | ES                | Spain             |
            | 0           | GB                | United Kingdom    |
            | 0           | IE                | Ireland           |
            | 0           | ME                | Montenegro        |
            | 0           | MK                | North Macedonia   |
            | 0           | MT                | Malta             |
            | 0           | PL                | Poland            |
            | 0           | RO                | Romania           |
            | 0           | RS                | Serbia            |
            | 0           | SE                | Sweden            |
            | 0           | SM                | San Marino        |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_listings_when_no_data_matches_query_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_points_listings(
            contestYear: 1066,
            contestStage: "GrandFinal",
            competingCountryCode: "ZZ"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 1066,
            contestStage: "GrandFinal",
            competingCountryCode: "ZZ"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_televote_points_listings_should_be_an_empty_list();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_listings_when_no_data_matches_query_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_points_listings(
            contestYear: 2023,
            contestStage: "SemiFinal1",
            competingCountryCode: "GB"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "SemiFinal1",
            competingCountryCode: "GB"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_televote_points_listings_should_be_an_empty_list();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_listings_when_no_data_matches_query_scenario_3(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_points_listings(
            contestYear: 2023,
            contestStage: "SemiFinal2",
            competingCountryCode: "GB"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "SemiFinal2",
            competingCountryCode: "GB"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_televote_points_listings_should_be_an_empty_list();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_listings_when_no_data_matches_query_scenario_4(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_points_listings(
            contestYear: 2023,
            contestStage: "GrandFinal",
            competingCountryCode: "SM"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "GrandFinal",
            competingCountryCode: "SM"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_televote_points_listings_should_be_an_empty_list();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_illegal_competing_country_code_value(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_points_listings(
            contestYear: 2023,
            competingCountryCode: "!",
            contestStage: "GrandFinal"
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

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetCompetingCountryPointsListingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_competing_country_points_listings(
            string competingCountryCode = "",
            string contestStage = "",
            int contestYear = 0
        )
        {
            Dictionary<string, object?> queryParams = new()
            {
                { nameof(competingCountryCode), competingCountryCode },
                { nameof(contestStage), contestStage },
                { nameof(contestYear), contestYear },
            };

            Request = Kernel.Requests.Listings.GetCompetingCountryPointsListings(queryParams);
        }

        public async Task Then_the_retrieved_jury_points_listings_in_order_should_match(string table)
        {
            CompetingCountryJuryPointsListing[] expectedListings = MarkdownParser.ParseTable(
                table,
                MapToJuryPointsListing
            );

            await Assert
                .That(SuccessResponse?.Data?.JuryPointsListings)
                .IsEquivalentTo(expectedListings, new JuryPointsListingEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_jury_points_listings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.JuryPointsListings).IsEmpty();

        public async Task Then_the_retrieved_televote_points_listings_in_order_should_match(string table)
        {
            CompetingCountryTelevotePointsListing[] expectedListings = MarkdownParser.ParseTable(
                table,
                MapToTelevotePointsListing
            );

            await Assert
                .That(SuccessResponse?.Data?.TelevotePointsListings)
                .IsEquivalentTo(
                    expectedListings,
                    new TelevotePointsListingEqualityComparer(),
                    CollectionOrdering.Matching
                );
        }

        public async Task Then_the_retrieved_televote_points_listings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.TelevotePointsListings).IsEmpty();

        public async Task Then_the_retrieved_metadata_should_match(
            string competingCountryCode = "",
            string contestStage = "",
            int contestYear = 0
        )
        {
            ContestStage expectedContestStage = Enum.Parse<ContestStage>(contestStage);

            await Assert
                .That(SuccessResponse?.Data?.Metadata)
                .HasProperty(cc => cc.CompetingCountryCode, competingCountryCode)
                .And.HasProperty(cc => cc.ContestStage, expectedContestStage)
                .And.HasProperty(cc => cc.ContestYear, contestYear);
        }

        private static CompetingCountryJuryPointsListing MapToJuryPointsListing(Dictionary<string, string> row)
        {
            return new CompetingCountryJuryPointsListing
            {
                PointsValue = int.Parse(row["PointsValue"]),
                VotingCountryCode = row["VotingCountryCode"],
                VotingCountryName = row["VotingCountryName"],
            };
        }

        private static CompetingCountryTelevotePointsListing MapToTelevotePointsListing(Dictionary<string, string> row)
        {
            return new CompetingCountryTelevotePointsListing
            {
                PointsValue = int.Parse(row["PointsValue"]),
                VotingCountryCode = row["VotingCountryCode"],
                VotingCountryName = row["VotingCountryName"],
            };
        }

        private sealed class JuryPointsListingEqualityComparer : IEqualityComparer<CompetingCountryJuryPointsListing>
        {
            public bool Equals(CompetingCountryJuryPointsListing? x, CompetingCountryJuryPointsListing? y)
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

                return x.PointsValue == y.PointsValue
                    && x.VotingCountryCode.Equals(y.VotingCountryCode, StringComparison.Ordinal)
                    && x.VotingCountryName.Equals(y.VotingCountryName, StringComparison.Ordinal);
            }

            public int GetHashCode(CompetingCountryJuryPointsListing obj) =>
                HashCode.Combine(obj.PointsValue, obj.VotingCountryCode, obj.VotingCountryName);
        }

        private sealed class TelevotePointsListingEqualityComparer
            : IEqualityComparer<CompetingCountryTelevotePointsListing>
        {
            public bool Equals(CompetingCountryTelevotePointsListing? x, CompetingCountryTelevotePointsListing? y)
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

                return x.PointsValue == y.PointsValue
                    && x.VotingCountryCode.Equals(y.VotingCountryCode, StringComparison.Ordinal)
                    && x.VotingCountryName.Equals(y.VotingCountryName, StringComparison.Ordinal);
            }

            public int GetHashCode(CompetingCountryTelevotePointsListing obj) =>
                HashCode.Combine(obj.PointsValue, obj.VotingCountryCode, obj.VotingCountryName);
        }
    }
}

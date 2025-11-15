using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.Listings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.Listings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.Listings;

[Category("public-api")]
public sealed class GetVotingCountryPointsListingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_voting_country_points_listings(
            contestYear: 2023,
            contestStage: "GrandFinal",
            votingCountryCode: "FI"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "GrandFinal",
            votingCountryCode: "FI"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_in_order_should_match(
            """
            | PointsValue | CompetingCountryCode | CompetingCountryName |
            |-------------|----------------------|----------------------|
            | 12          | SE                   | Sweden               |
            | 10          | CH                   | Switzerland          |
            | 8           | CZ                   | Czechia              |
            | 7           | FR                   | France               |
            | 6           | IT                   | Italy                |
            | 5           | BE                   | Belgium              |
            | 4           | GB                   | United Kingdom       |
            | 3           | PT                   | Portugal             |
            | 2           | AT                   | Austria              |
            | 1           | CY                   | Cyprus               |
            | 0           | AL                   | Albania              |
            | 0           | AM                   | Armenia              |
            | 0           | AU                   | Australia            |
            | 0           | DE                   | Germany              |
            | 0           | EE                   | Estonia              |
            | 0           | ES                   | Spain                |
            | 0           | HR                   | Croatia              |
            | 0           | IL                   | Israel               |
            | 0           | LT                   | Lithuania            |
            | 0           | MD                   | Moldova              |
            | 0           | NO                   | Norway               |
            | 0           | PL                   | Poland               |
            | 0           | RS                   | Serbia               |
            | 0           | SI                   | Slovenia             |
            | 0           | UA                   | Ukraine              |
            """
        );
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | CompetingCountryCode | CompetingCountryName |
            |-------------|----------------------|----------------------|
            | 12          | NO                   | Norway               |
            | 10          | CZ                   | Czechia              |
            | 8           | AU                   | Australia            |
            | 7           | SI                   | Slovenia             |
            | 6           | EE                   | Estonia              |
            | 5           | DE                   | Germany              |
            | 4           | HR                   | Croatia              |
            | 3           | MD                   | Moldova              |
            | 2           | AT                   | Austria              |
            | 1           | CH                   | Switzerland          |
            | 0           | AL                   | Albania              |
            | 0           | AM                   | Armenia              |
            | 0           | BE                   | Belgium              |
            | 0           | CY                   | Cyprus               |
            | 0           | ES                   | Spain                |
            | 0           | FR                   | France               |
            | 0           | GB                   | United Kingdom       |
            | 0           | IL                   | Israel               |
            | 0           | IT                   | Italy                |
            | 0           | LT                   | Lithuania            |
            | 0           | PL                   | Poland               |
            | 0           | PT                   | Portugal             |
            | 0           | RS                   | Serbia               |
            | 0           | SE                   | Sweden               |
            | 0           | UA                   | Ukraine              |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_voting_country_points_listings(
            contestYear: 2022,
            contestStage: "GrandFinal",
            votingCountryCode: "FI"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2022,
            contestStage: "GrandFinal",
            votingCountryCode: "FI"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_in_order_should_match(
            """
            | PointsValue | CompetingCountryCode | CompetingCountryName |
            |-------------|----------------------|----------------------|
            | 12          | SE                   | Sweden               |
            | 10          | GB                   | United Kingdom       |
            | 8           | AU                   | Australia            |
            | 7           | RS                   | Serbia               |
            | 6           | GR                   | Greece               |
            | 5           | NL                   | Netherlands          |
            | 4           | NO                   | Norway               |
            | 3           | AZ                   | Azerbaijan           |
            | 2           | IT                   | Italy                |
            | 1           | AM                   | Armenia              |
            | 0           | BE                   | Belgium              |
            | 0           | CH                   | Switzerland          |
            | 0           | CZ                   | Czechia              |
            | 0           | DE                   | Germany              |
            | 0           | EE                   | Estonia              |
            | 0           | ES                   | Spain                |
            | 0           | FR                   | France               |
            | 0           | IS                   | Iceland              |
            | 0           | LT                   | Lithuania            |
            | 0           | MD                   | Moldova              |
            | 0           | PL                   | Poland               |
            | 0           | PT                   | Portugal             |
            | 0           | RO                   | Romania              |
            | 0           | UA                   | Ukraine              |
            """
        );
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | CompetingCountryCode | CompetingCountryName |
            |-------------|----------------------|----------------------|
            | 12          | UA                   | Ukraine              |
            | 10          | EE                   | Estonia              |
            | 8           | SE                   | Sweden               |
            | 7           | RS                   | Serbia               |
            | 6           | NO                   | Norway               |
            | 5           | MD                   | Moldova              |
            | 4           | GB                   | United Kingdom       |
            | 3           | ES                   | Spain                |
            | 2           | LT                   | Lithuania            |
            | 1           | FR                   | France               |
            | 0           | AM                   | Armenia              |
            | 0           | AU                   | Australia            |
            | 0           | AZ                   | Azerbaijan           |
            | 0           | BE                   | Belgium              |
            | 0           | CH                   | Switzerland          |
            | 0           | CZ                   | Czechia              |
            | 0           | DE                   | Germany              |
            | 0           | GR                   | Greece               |
            | 0           | IS                   | Iceland              |
            | 0           | IT                   | Italy                |
            | 0           | NL                   | Netherlands          |
            | 0           | PL                   | Poland               |
            | 0           | PT                   | Portugal             |
            | 0           | RO                   | Romania              |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_3(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_voting_country_points_listings(
            contestYear: 2023,
            contestStage: "SemiFinal1",
            votingCountryCode: "FI"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "SemiFinal1",
            votingCountryCode: "FI"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | CompetingCountryCode | CompetingCountryName |
            |-------------|----------------------|----------------------|
            | 12          | CZ                   | Czechia              |
            | 10          | NO                   | Norway               |
            | 8           | CH                   | Switzerland          |
            | 7           | MD                   | Moldova              |
            | 6           | HR                   | Croatia              |
            | 5           | SE                   | Sweden               |
            | 4           | RS                   | Serbia               |
            | 3           | LV                   | Latvia               |
            | 2           | PT                   | Portugal             |
            | 1           | IL                   | Israel               |
            | 0           | AZ                   | Azerbaijan           |
            | 0           | IE                   | Ireland              |
            | 0           | MT                   | Malta                |
            | 0           | NL                   | Netherlands          |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_4(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_voting_country_points_listings(
            contestYear: 2023,
            contestStage: "GrandFinal",
            votingCountryCode: "SI"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "GrandFinal",
            votingCountryCode: "SI"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_in_order_should_match(
            """
            | PointsValue | CompetingCountryCode | CompetingCountryName |
            |-------------|----------------------|----------------------|
            | 12          | IT                   | Italy                |
            | 10          | EE                   | Estonia              |
            | 8           | LT                   | Lithuania            |
            | 7           | SE                   | Sweden               |
            | 6           | CZ                   | Czechia              |
            | 5           | BE                   | Belgium              |
            | 4           | AU                   | Australia            |
            | 3           | AT                   | Austria              |
            | 2           | ES                   | Spain                |
            | 1           | CH                   | Switzerland          |
            | 0           | AL                   | Albania              |
            | 0           | AM                   | Armenia              |
            | 0           | CY                   | Cyprus               |
            | 0           | DE                   | Germany              |
            | 0           | FI                   | Finland              |
            | 0           | FR                   | France               |
            | 0           | GB                   | United Kingdom       |
            | 0           | HR                   | Croatia              |
            | 0           | IL                   | Israel               |
            | 0           | MD                   | Moldova              |
            | 0           | NO                   | Norway               |
            | 0           | PL                   | Poland               |
            | 0           | PT                   | Portugal             |
            | 0           | RS                   | Serbia               |
            | 0           | UA                   | Ukraine              |
            """
        );
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | CompetingCountryCode | CompetingCountryName |
            |-------------|----------------------|----------------------|
            | 12          | HR                   | Croatia              |
            | 10          | FI                   | Finland              |
            | 8           | IT                   | Italy                |
            | 7           | AL                   | Albania              |
            | 6           | RS                   | Serbia               |
            | 5           | NO                   | Norway               |
            | 4           | SE                   | Sweden               |
            | 3           | FR                   | France               |
            | 2           | BE                   | Belgium              |
            | 1           | PL                   | Poland               |
            | 0           | AM                   | Armenia              |
            | 0           | AT                   | Austria              |
            | 0           | AU                   | Australia            |
            | 0           | CH                   | Switzerland          |
            | 0           | CY                   | Cyprus               |
            | 0           | CZ                   | Czechia              |
            | 0           | DE                   | Germany              |
            | 0           | EE                   | Estonia              |
            | 0           | ES                   | Spain                |
            | 0           | GB                   | United Kingdom       |
            | 0           | IL                   | Israel               |
            | 0           | LT                   | Lithuania            |
            | 0           | MD                   | Moldova              |
            | 0           | PT                   | Portugal             |
            | 0           | UA                   | Ukraine              |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_5(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_voting_country_points_listings(
            contestYear: 2023,
            contestStage: "SemiFinal2",
            votingCountryCode: "SM"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "SemiFinal2",
            votingCountryCode: "SM"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | CompetingCountryCode | CompetingCountryName |
            |-------------|----------------------|----------------------|
            | 12          | LT                   | Lithuania            |
            | 10          | EE                   | Estonia              |
            | 8           | AT                   | Austria              |
            | 7           | IS                   | Iceland              |
            | 6           | AU                   | Australia            |
            | 5           | BE                   | Belgium              |
            | 4           | AM                   | Armenia              |
            | 3           | GE                   | Georgia              |
            | 2           | PL                   | Poland               |
            | 1           | CY                   | Cyprus               |
            | 0           | AL                   | Albania              |
            | 0           | DK                   | Denmark              |
            | 0           | GR                   | Greece               |
            | 0           | RO                   | Romania              |
            | 0           | SI                   | Slovenia             |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_6(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_voting_country_points_listings(
            contestYear: 2022,
            contestStage: "SemiFinal1",
            votingCountryCode: "NO"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2022,
            contestStage: "SemiFinal1",
            votingCountryCode: "NO"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_in_order_should_match(
            """
            | PointsValue | CompetingCountryCode | CompetingCountryName |
            |-------------|----------------------|----------------------|
            | 12          | GR                   | Greece               |
            | 10          | PT                   | Portugal             |
            | 8           | NL                   | Netherlands          |
            | 7           | AM                   | Armenia              |
            | 6           | UA                   | Ukraine              |
            | 5           | LT                   | Lithuania            |
            | 4           | CH                   | Switzerland          |
            | 3           | DK                   | Denmark              |
            | 2           | IS                   | Iceland              |
            | 1           | MD                   | Moldova              |
            | 0           | AL                   | Albania              |
            | 0           | AT                   | Austria              |
            | 0           | BG                   | Bulgaria             |
            | 0           | HR                   | Croatia              |
            | 0           | LV                   | Latvia               |
            | 0           | SI                   | Slovenia             |
            """
        );
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | CompetingCountryCode | CompetingCountryName |
            |-------------|----------------------|----------------------|
            | 12          | GR                   | Greece               |
            | 10          | UA                   | Ukraine              |
            | 8           | LT                   | Lithuania            |
            | 7           | IS                   | Iceland              |
            | 6           | MD                   | Moldova              |
            | 5           | AM                   | Armenia              |
            | 4           | DK                   | Denmark              |
            | 3           | AT                   | Austria              |
            | 2           | PT                   | Portugal             |
            | 1           | NL                   | Netherlands          |
            | 0           | AL                   | Albania              |
            | 0           | BG                   | Bulgaria             |
            | 0           | CH                   | Switzerland          |
            | 0           | HR                   | Croatia              |
            | 0           | LV                   | Latvia               |
            | 0           | SI                   | Slovenia             |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_7(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_voting_country_points_listings(
            contestYear: 2022,
            contestStage: "SemiFinal2",
            votingCountryCode: "GE"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2022,
            contestStage: "SemiFinal2",
            votingCountryCode: "GE"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_in_order_should_match(
            """
            | PointsValue | CompetingCountryCode | CompetingCountryName |
            |-------------|----------------------|----------------------|
            | 12          | SE                   | Sweden               |
            | 10          | AU                   | Australia            |
            | 8           | PL                   | Poland               |
            | 7           | EE                   | Estonia              |
            | 6           | CZ                   | Czechia              |
            | 5           | BE                   | Belgium              |
            | 4           | AZ                   | Azerbaijan           |
            | 3           | FI                   | Finland              |
            | 2           | RS                   | Serbia               |
            | 1           | MT                   | Malta                |
            | 0           | CY                   | Cyprus               |
            | 0           | IE                   | Ireland              |
            | 0           | IL                   | Israel               |
            | 0           | ME                   | Montenegro           |
            | 0           | MK                   | North Macedonia      |
            | 0           | RO                   | Romania              |
            | 0           | SM                   | San Marino           |
            """
        );
        await euroFan.Then_the_retrieved_televote_points_listings_in_order_should_match(
            """
            | PointsValue | CompetingCountryCode | CompetingCountryName |
            |-------------|----------------------|----------------------|
            | 12          | RS                   | Serbia               |
            | 10          | IL                   | Israel               |
            | 8           | SE                   | Sweden               |
            | 7           | EE                   | Estonia              |
            | 6           | FI                   | Finland              |
            | 5           | RO                   | Romania              |
            | 4           | CZ                   | Czechia              |
            | 3           | PL                   | Poland               |
            | 2           | AU                   | Australia            |
            | 1           | CY                   | Cyprus               |
            | 0           | AZ                   | Azerbaijan           |
            | 0           | BE                   | Belgium              |
            | 0           | IE                   | Ireland              |
            | 0           | ME                   | Montenegro           |
            | 0           | MK                   | North Macedonia      |
            | 0           | MT                   | Malta                |
            | 0           | SM                   | San Marino           |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_listings_when_no_data_matches_query_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_voting_country_points_listings(
            contestYear: 1066,
            contestStage: "GrandFinal",
            votingCountryCode: "ZZ"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 1066,
            contestStage: "GrandFinal",
            votingCountryCode: "ZZ"
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
        euroFan.Given_I_want_to_retrieve_voting_country_points_listings(
            contestYear: 2023,
            contestStage: "SemiFinal1",
            votingCountryCode: "GB"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "SemiFinal1",
            votingCountryCode: "GB"
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
        euroFan.Given_I_want_to_retrieve_voting_country_points_listings(
            contestYear: 2023,
            contestStage: "SemiFinal2",
            votingCountryCode: "FR"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2023,
            contestStage: "SemiFinal2",
            votingCountryCode: "FR"
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
        euroFan.Given_I_want_to_retrieve_voting_country_points_listings(
            contestYear: 2022,
            contestStage: "GrandFinal",
            votingCountryCode: "XX"
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestYear: 2022,
            contestStage: "GrandFinal",
            votingCountryCode: "XX"
        );
        await euroFan.Then_the_retrieved_jury_points_listings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_televote_points_listings_should_be_an_empty_list();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_illegal_voting_country_code_value(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_voting_country_points_listings(
            contestYear: 2023,
            votingCountryCode: "!",
            contestStage: "GrandFinal"
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

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetVotingCountryPointsListingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_voting_country_points_listings(
            string votingCountryCode = "",
            string contestStage = "",
            int contestYear = 0
        )
        {
            Dictionary<string, object?> queryParams = new()
            {
                { nameof(votingCountryCode), votingCountryCode },
                { nameof(contestStage), contestStage },
                { nameof(contestYear), contestYear },
            };

            Request = Kernel.Requests.Listings.GetVotingCountryPointsListings(queryParams);
        }

        public async Task Then_the_retrieved_jury_points_listings_in_order_should_match(string table)
        {
            VotingCountryJuryPointsListing[] expectedListings = MarkdownParser.ParseTable(
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
            VotingCountryTelevotePointsListing[] expectedListings = MarkdownParser.ParseTable(
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
            string votingCountryCode = "",
            string contestStage = "",
            int contestYear = 0
        )
        {
            ContestStage expectedContestStage = Enum.Parse<ContestStage>(contestStage);

            await Assert
                .That(SuccessResponse?.Data?.Metadata)
                .HasProperty(vc => vc.VotingCountryCode, votingCountryCode)
                .And.HasProperty(vc => vc.ContestStage, expectedContestStage)
                .And.HasProperty(vc => vc.ContestYear, contestYear);
        }

        private static VotingCountryJuryPointsListing MapToJuryPointsListing(Dictionary<string, string> row)
        {
            return new VotingCountryJuryPointsListing
            {
                PointsValue = int.Parse(row["PointsValue"]),
                CompetingCountryCode = row["CompetingCountryCode"],
                CompetingCountryName = row["CompetingCountryName"],
            };
        }

        private static VotingCountryTelevotePointsListing MapToTelevotePointsListing(Dictionary<string, string> row)
        {
            return new VotingCountryTelevotePointsListing
            {
                PointsValue = int.Parse(row["PointsValue"]),
                CompetingCountryCode = row["CompetingCountryCode"],
                CompetingCountryName = row["CompetingCountryName"],
            };
        }

        private sealed class JuryPointsListingEqualityComparer : IEqualityComparer<VotingCountryJuryPointsListing>
        {
            public bool Equals(VotingCountryJuryPointsListing? x, VotingCountryJuryPointsListing? y)
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
                    && x.CompetingCountryCode.Equals(y.CompetingCountryCode, StringComparison.Ordinal)
                    && x.CompetingCountryName.Equals(y.CompetingCountryName, StringComparison.Ordinal);
            }

            public int GetHashCode(VotingCountryJuryPointsListing obj) =>
                HashCode.Combine(obj.PointsValue, obj.CompetingCountryCode, obj.CompetingCountryName);
        }

        private sealed class TelevotePointsListingEqualityComparer
            : IEqualityComparer<VotingCountryTelevotePointsListing>
        {
            public bool Equals(VotingCountryTelevotePointsListing? x, VotingCountryTelevotePointsListing? y)
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
                    && x.CompetingCountryCode.Equals(y.CompetingCountryCode, StringComparison.Ordinal)
                    && x.CompetingCountryName.Equals(y.CompetingCountryName, StringComparison.Ordinal);
            }

            public int GetHashCode(VotingCountryTelevotePointsListing obj) =>
                HashCode.Combine(obj.PointsValue, obj.CompetingCountryCode, obj.CompetingCountryName);
        }
    }
}

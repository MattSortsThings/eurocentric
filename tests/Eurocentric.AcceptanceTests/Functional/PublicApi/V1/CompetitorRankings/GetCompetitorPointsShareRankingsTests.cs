using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.CompetitorRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.CompetitorRankings;

[Category("public-api")]
public sealed class GetCompetitorPointsShareRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle              | PointsShare | TotalPoints | AvailablePoints | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------------|------------------------|-------------|-------------|-----------------|--------------|-----------------|
            | 1    | 2022        | SemiFinal2   | 17               | SE          | Sweden      | 1                 | Cornelia Jakobs  | Hold Me Closer         | 0.825       | 396         | 480             | 40           | 20              |
            | 2    | 2023        | SemiFinal1   | 15               | FI          | Finland     | 1                 | Käärijä          | Cha Cha Cha            | 0.819444    | 177         | 216             | 18           | 18              |
            | 3    | 2022        | SemiFinal1   | 6                | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania               | 0.780093    | 337         | 432             | 36           | 18              |
            | 4    | 2022        | GrandFinal   | 12               | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania               | 0.674145    | 631         | 936             | 78           | 39              |
            | 5    | 2023        | GrandFinal   | 9                | SE          | Sweden      | 1                 | Loreen           | Tattoo                 | 0.665525    | 583         | 876             | 73           | 37              |
            | 6    | 2023        | SemiFinal2   | 16               | AU          | Australia   | 1                 | Voyager          | Promise                | 0.653509    | 149         | 228             | 19           | 19              |
            | 7    | 2023        | SemiFinal1   | 11               | SE          | Sweden      | 2                 | Loreen           | Tattoo                 | 0.625       | 135         | 216             | 18           | 18              |
            | 8    | 2023        | SemiFinal2   | 13               | AT          | Austria     | 2                 | Teya & Salena    | Who The Hell Is Edgar? | 0.600877    | 137         | 228             | 19           | 19              |
            | 9    | 2023        | GrandFinal   | 13               | FI          | Finland     | 2                 | Käärijä          | Cha Cha Cha            | 0.600457    | 526         | 876             | 73           | 37              |
            | 10   | 2023        | SemiFinal1   | 9                | IL          | Israel      | 3                 | Noa Kirel        | Unicorn                | 0.587963    | 127         | 216             | 18           | 18              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 117,
            totalPages: 12
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(pageIndex: 2, pageSize: 3);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle              | PointsShare | TotalPoints | AvailablePoints | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------------|------------------------|-------------|-------------|-----------------|--------------|-----------------|
            | 7    | 2023        | SemiFinal1   | 11               | SE          | Sweden      | 2                 | Loreen           | Tattoo                 | 0.625       | 135         | 216             | 18           | 18              |
            | 8    | 2023        | SemiFinal2   | 13               | AT          | Austria     | 2                 | Teya & Salena    | Who The Hell Is Edgar? | 0.600877    | 137         | 228             | 19           | 19              |
            | 9    | 2023        | GrandFinal   | 13               | FI          | Finland     | 2                 | Käärijä          | Cha Cha Cha            | 0.600457    | 526         | 876             | 73           | 37              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            pageIndex: 2,
            pageSize: 3,
            descending: false,
            totalItems: 117,
            totalPages: 39
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_3(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(
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
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName        | SongTitle             | PointsShare | TotalPoints | AvailablePoints | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|----------------|-----------------------|-------------|-------------|-----------------|--------------|-----------------|
            | 116  | 2023        | SemiFinal2   | 3                | RO          | Romania     | 15                | Theodor Andrei | D.G.T. (Off and On)   | 0           | 0           | 228             | 19           | 19              |
            | 116  | 2023        | SemiFinal2   | 12               | SM          | San Marino  | 16                | Piqued Jacks   | Like An Animal        | 0           | 0           | 228             | 19           | 19              |
            | 115  | 2022        | GrandFinal   | 13               | DE          | Germany     | 25                | Malik Harris   | Rockstars             | 0.00641     | 6           | 936             | 78           | 39              |
            | 114  | 2023        | SemiFinal1   | 2                | MT          | Malta       | 15                | The Busker     | Dance (Our Own Party) | 0.013889    | 3           | 216             | 18           | 18              |
            | 113  | 2022        | GrandFinal   | 6                | FR          | France      | 24                | Alvan & Ahez   | Fulenn                | 0.018162    | 17          | 936             | 78           | 39              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 117,
            totalPages: 24
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_4(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(
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
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle      | PointsShare | TotalPoints | AvailablePoints | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------------|----------------|-------------|-------------|-----------------|--------------|-----------------|
            | 1    | 2022        | SemiFinal2   | 17               | SE          | Sweden      | 1                 | Cornelia Jakobs  | Hold Me Closer | 0.825       | 396         | 480             | 40           | 20              |
            | 2    | 2022        | SemiFinal1   | 6                | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania       | 0.780093    | 337         | 432             | 36           | 18              |
            | 3    | 2022        | GrandFinal   | 12               | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania       | 0.674145    | 631         | 936             | 78           | 39              |
            | 4    | 2022        | SemiFinal1   | 8                | NL          | Netherlands | 2                 | S10              | De Diepte      | 0.511574    | 221         | 432             | 36           | 18              |
            | 5    | 2022        | SemiFinal2   | 8                | AU          | Australia   | 2                 | Sheldon Riley    | Not The Same   | 0.50625     | 243         | 480             | 40           | 20              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minYear: 2022,
            maxYear: 2022,
            contestStage: "Any",
            votingMethod: "Any",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 60,
            totalPages: 12
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_5(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(
            contestStage: "SemiFinals",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle      | PointsShare | TotalPoints | AvailablePoints | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------------|----------------|-------------|-------------|-----------------|--------------|-----------------|
            | 1    | 2022        | SemiFinal2   | 17               | SE          | Sweden      | 1                 | Cornelia Jakobs  | Hold Me Closer | 0.825       | 396         | 480             | 40           | 20              |
            | 2    | 2023        | SemiFinal1   | 15               | FI          | Finland     | 1                 | Käärijä          | Cha Cha Cha    | 0.819444    | 177         | 216             | 18           | 18              |
            | 3    | 2022        | SemiFinal1   | 6                | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania       | 0.780093    | 337         | 432             | 36           | 18              |
            | 4    | 2023        | SemiFinal2   | 16               | AU          | Australia   | 1                 | Voyager          | Promise        | 0.653509    | 149         | 228             | 19           | 19              |
            | 5    | 2023        | SemiFinal1   | 11               | SE          | Sweden      | 2                 | Loreen           | Tattoo         | 0.625       | 135         | 216             | 18           | 18              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            contestStage: "SemiFinals",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 66,
            totalPages: 14
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_6(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(
            competingCountryCode: "FI",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName    | SongTitle   | PointsShare | TotalPoints | AvailablePoints | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------|-------------|-------------|-------------|-----------------|--------------|-----------------|
            | 1    | 2022        | SemiFinal2   | 1                | FI          | Finland     | 7                 | The Rasmus | Jezebel     | 1           | 162         | 162             | 40           | 20              |
            | 1    | 2022        | GrandFinal   | 4                | FI          | Finland     | 21                | The Rasmus | Jezebel     | 1           | 38          | 38              | 78           | 39              |
            | 1    | 2023        | SemiFinal1   | 15               | FI          | Finland     | 1                 | Käärijä    | Cha Cha Cha | 1           | 177         | 177             | 18           | 18              |
            | 1    | 2023        | GrandFinal   | 13               | FI          | Finland     | 2                 | Käärijä    | Cha Cha Cha | 1           | 526         | 526             | 73           | 37              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            competingCountryCode: "FI",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 4,
            totalPages: 1
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_7(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(votingMethod: "Jury", pageSize: 5);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName                   | SongTitle      | PointsShare | TotalPoints | AvailablePoints | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|---------------------------|----------------|-------------|-------------|-----------------|--------------|-----------------|
            | 1    | 2022        | SemiFinal2   | 17               | SE          | Sweden      | 1                 | Cornelia Jakobs           | Hold Me Closer | 0.925       | 222         | 240             | 20           | 20              |
            | 2    | 2023        | GrandFinal   | 9                | SE          | Sweden      | 1                 | Loreen                    | Tattoo         | 0.787037    | 340         | 432             | 36           | 36              |
            | 3    | 2022        | SemiFinal2   | 8                | AU          | Australia   | 2                 | Sheldon Riley             | Not The Same   | 0.704167    | 169         | 240             | 20           | 20              |
            | 4    | 2022        | SemiFinal1   | 15               | GR          | Greece      | 3                 | Amanda Georgiadi Tenfjord | Die Together   | 0.699074    | 151         | 216             | 18           | 18              |
            | 5    | 2022        | SemiFinal1   | 8                | NL          | Netherlands | 2                 | S10                       | De Diepte      | 0.657407    | 142         | 216             | 18           | 18              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            votingMethod: "Jury",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 86,
            totalPages: 18
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_8(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(
            votingMethod: "Televote",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle       | PointsShare | TotalPoints | AvailablePoints | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------------|-----------------|-------------|-------------|-----------------|--------------|-----------------|
            | 1    | 2022        | GrandFinal   | 12               | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania        | 0.938034    | 439         | 468             | 39           | 39              |
            | 2    | 2022        | SemiFinal1   | 6                | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania        | 0.935185    | 202         | 216             | 18           | 18              |
            | 3    | 2023        | GrandFinal   | 13               | FI          | Finland     | 2                 | Käärijä          | Cha Cha Cha     | 0.846847    | 376         | 444             | 37           | 37              |
            | 4    | 2023        | SemiFinal1   | 15               | FI          | Finland     | 1                 | Käärijä          | Cha Cha Cha     | 0.819444    | 177         | 216             | 18           | 18              |
            | 5    | 2022        | SemiFinal2   | 3                | RS          | Serbia      | 3                 | Konstrakta       | In Corpore Sano | 0.725       | 174         | 240             | 20           | 20              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            votingMethod: "Televote",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 117,
            totalPages: 24
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_9(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(
            minYear: 2023,
            contestStage: "Any",
            votingMethod: "Televote",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName       | SongTitle              | PointsShare | TotalPoints | AvailablePoints | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|---------------|------------------------|-------------|-------------|-----------------|--------------|-----------------|
            | 1    | 2023        | GrandFinal   | 13               | FI          | Finland     | 2                 | Käärijä       | Cha Cha Cha            | 0.846847    | 376         | 444             | 37           | 37              |
            | 2    | 2023        | SemiFinal1   | 15               | FI          | Finland     | 1                 | Käärijä       | Cha Cha Cha            | 0.819444    | 177         | 216             | 18           | 18              |
            | 3    | 2023        | SemiFinal2   | 16               | AU          | Australia   | 1                 | Voyager       | Promise                | 0.653509    | 149         | 228             | 19           | 19              |
            | 4    | 2023        | SemiFinal1   | 11               | SE          | Sweden      | 2                 | Loreen        | Tattoo                 | 0.625       | 135         | 216             | 18           | 18              |
            | 5    | 2023        | SemiFinal2   | 13               | AT          | Austria     | 2                 | Teya & Salena | Who The Hell Is Edgar? | 0.600877    | 137         | 228             | 19           | 19              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minYear: 2023,
            contestStage: "Any",
            votingMethod: "Televote",
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 57,
            totalPages: 12
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_rankings_page_when_no_data_matches_query(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(minYear: 1066, maxYear: 1963);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_should_be_an_empty_list();
        await euroFan.Then_the_retrieved_metadata_should_match(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(pageIndex: -1);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(pageSize: 0);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(pageSize: 101);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(minYear: 2023, maxYear: 2022);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(competingCountryCode: "!");

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

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetCompetitorPointsShareRankingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_a_page_of_competitor_points_share_rankings(
            bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingMethod = null,
            string? competingCountryCode = null,
            string? contestStage = null,
            int? maxYear = null,
            int? minYear = null
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
            };

            Request = Kernel.Requests.CompetitorRankings.GetCompetitorPointsShareRankings(queryParams);
        }

        public async Task Then_the_retrieved_rankings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Rankings).IsEmpty();

        public async Task Then_the_retrieved_rankings_in_order_should_match(string table)
        {
            CompetitorPointsShareRanking[] expected = MarkdownParser.ParseTable(table, MapToRanking);

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
            string? contestStage = null,
            string? competingCountryCode = null,
            int? maxYear = null,
            int? minYear = null
        )
        {
            ContestStageFilter? expectedContestStage = contestStage.ToNullableContestStageFilter();
            VotingMethodFilter? expectedVotingMethod = votingMethod.ToNullableVotingMethodFilter();

            await Assert
                .That(SuccessResponse?.Data?.Metadata)
                .HasProperty(cp => cp.MinYear, minYear)
                .And.HasProperty(cp => cp.MaxYear, maxYear)
                .And.HasProperty(cp => cp.ContestStage, expectedContestStage)
                .And.HasProperty(cp => cp.CompetingCountryCode, competingCountryCode)
                .And.HasProperty(cp => cp.VotingMethod, expectedVotingMethod)
                .And.HasProperty(cp => cp.PageIndex, pageIndex)
                .And.HasProperty(cp => cp.PageSize, pageSize)
                .And.HasProperty(cp => cp.Descending, descending)
                .And.HasProperty(cp => cp.TotalItems, totalItems)
                .And.HasProperty(cp => cp.TotalPages, totalPages);
        }

        private static CompetitorPointsShareRanking MapToRanking(Dictionary<string, string> row)
        {
            return new CompetitorPointsShareRanking
            {
                Rank = int.Parse(row["Rank"]),
                ContestYear = int.Parse(row["ContestYear"]),
                ContestStage = Enum.Parse<ContestStage>(row["ContestStage"]),
                RunningOrderSpot = int.Parse(row["RunningOrderSpot"]),
                CountryCode = row["CountryCode"],
                CountryName = row["CountryName"],
                FinishingPosition = int.Parse(row["FinishingPosition"]),
                ActName = row["ActName"],
                SongTitle = row["SongTitle"],
                PointsShare = decimal.Parse(row["PointsShare"]),
                TotalPoints = int.Parse(row["TotalPoints"]),
                AvailablePoints = int.Parse(row["AvailablePoints"]),
                PointsAwards = int.Parse(row["PointsAwards"]),
                VotingCountries = int.Parse(row["VotingCountries"]),
            };
        }

        private sealed class RankingEqualityComparer : IEqualityComparer<CompetitorPointsShareRanking>
        {
            public bool Equals(CompetitorPointsShareRanking? x, CompetitorPointsShareRanking? y)
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
                    && x.ContestYear == y.ContestYear
                    && x.ContestStage == y.ContestStage
                    && x.RunningOrderSpot == y.RunningOrderSpot
                    && x.CountryCode.Equals(y.CountryCode, StringComparison.Ordinal)
                    && x.CountryName.Equals(y.CountryName, StringComparison.Ordinal)
                    && x.FinishingPosition == y.FinishingPosition
                    && x.ActName.Equals(y.ActName, StringComparison.Ordinal)
                    && x.SongTitle.Equals(y.SongTitle, StringComparison.Ordinal)
                    && x.PointsShare == y.PointsShare
                    && x.TotalPoints == y.TotalPoints
                    && x.AvailablePoints == y.AvailablePoints
                    && x.PointsAwards == y.PointsAwards
                    && x.VotingCountries == y.VotingCountries;
            }

            public int GetHashCode(CompetitorPointsShareRanking obj)
            {
                HashCode hashCode = new();
                hashCode.Add(obj.Rank);
                hashCode.Add(obj.ContestYear);
                hashCode.Add((int)obj.ContestStage);
                hashCode.Add(obj.RunningOrderSpot);
                hashCode.Add(obj.CountryCode);
                hashCode.Add(obj.CountryName);
                hashCode.Add(obj.FinishingPosition);
                hashCode.Add(obj.ActName);
                hashCode.Add(obj.SongTitle);
                hashCode.Add(obj.PointsShare);
                hashCode.Add(obj.TotalPoints);
                hashCode.Add(obj.AvailablePoints);
                hashCode.Add(obj.PointsAwards);
                hashCode.Add(obj.VotingCountries);

                return hashCode.ToHashCode();
            }
        }
    }
}

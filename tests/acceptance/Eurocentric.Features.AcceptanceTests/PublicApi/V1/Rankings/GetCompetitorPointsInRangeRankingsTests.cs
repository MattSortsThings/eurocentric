using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetitorPointsInRangeRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Rankings;

public sealed class GetCompetitorPointsInRangeRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_minimum_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
            maxPoints: 12);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName                   | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|---------------------------|----------------|---------------|---------------------|--------------|
            | 1    | 2022        | SemiFinal2   | 17                   | SE          | Sweden      | 1                 | Cornelia Jakobs           | Hold Me Closer | 0.675         | 27                  | 40           |
            | 2    | 2023        | SemiFinal1   | 15                   | FI          | Finland     | 1                 | Käärijä                   | Cha Cha Cha    | 0.666667      | 12                  | 18           |
            | 3    | 2022        | SemiFinal1   | 6                    | UA          | Ukraine     | 1                 | Kalush Orchestra          | Stefania       | 0.638889      | 23                  | 36           |
            | 4    | 2022        | GrandFinal   | 12                   | UA          | Ukraine     | 1                 | Kalush Orchestra          | Stefania       | 0.564103      | 44                  | 78           |
            | 5    | 2023        | GrandFinal   | 13                   | FI          | Finland     | 2                 | Käärijä                   | Cha Cha Cha    | 0.438356      | 32                  | 73           |
            | 6    | 2023        | GrandFinal   | 9                    | SE          | Sweden      | 1                 | Loreen                    | Tattoo         | 0.424658      | 31                  | 73           |
            | 7    | 2023        | SemiFinal1   | 1                    | NO          | Norway      | 6                 | Alessandra                | Queen of Kings | 0.333333      | 6                   | 18           |
            | 7    | 2023        | SemiFinal1   | 11                   | SE          | Sweden      | 2                 | Loreen                    | Tattoo         | 0.333333      | 6                   | 18           |
            | 9    | 2022        | SemiFinal2   | 8                    | AU          | Australia   | 2                 | Sheldon Riley             | Not The Same   | 0.325         | 13                  | 40           |
            | 10   | 2022        | SemiFinal1   | 15                   | GR          | Greece      | 3                 | Amanda Georgiadi Tenfjord | Die Together   | 0.305556      | 11                  | 36           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 10,
            descending: false,
            totalItems: 117,
            totalPages: 12);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_requested_pagination(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
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
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName       | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|---------------|----------------|---------------|---------------------|--------------|
            | 95   | 2022        | SemiFinal2   | 5                    | GE          | Georgia     | 18                | Circus Mircus | Lock Me In     | 0             | 0                   | 40           |
            | 95   | 2022        | SemiFinal2   | 6                    | MT          | Malta       | 16                | Emma Muscat   | I Am What I Am | 0             | 0                   | 40           |
            | 95   | 2022        | GrandFinal   | 1                    | CZ          | Czechia     | 22                | We Are Domi   | Lights Off     | 0             | 0                   | 78           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 1,
            pageSize: 3,
            descending: true,
            totalItems: 117,
            totalPages: 39);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_contest_stage(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
            maxPoints: 12,
            pageSize: 5,
            contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName    | FinishingPosition | ActName          | SongTitle   | PointsInRange | PointsAwardsInRange | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|----------------|-------------------|------------------|-------------|---------------|---------------------|--------------|
            | 1    | 2022        | GrandFinal   | 12                   | UA          | Ukraine        | 1                 | Kalush Orchestra | Stefania    | 0.564103      | 44                  | 78           |
            | 2    | 2023        | GrandFinal   | 13                   | FI          | Finland        | 2                 | Käärijä          | Cha Cha Cha | 0.438356      | 32                  | 73           |
            | 3    | 2023        | GrandFinal   | 9                    | SE          | Sweden         | 1                 | Loreen           | Tattoo      | 0.424658      | 31                  | 73           |
            | 4    | 2022        | GrandFinal   | 22                   | GB          | United Kingdom | 2                 | Sam Ryder        | SPACE MAN   | 0.217949      | 17                  | 78           |
            | 5    | 2023        | GrandFinal   | 23                   | IL          | Israel         | 3                 | Noa Kirel        | Unicorn     | 0.205479      | 15                  | 73           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
            contestStage: "GrandFinal",
            votingMethod: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 51,
            totalPages: 11);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_min_year(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
            maxPoints: 12,
            pageSize: 5,
            minYear: 2023);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName    | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|------------|----------------|---------------|---------------------|--------------|
            | 1    | 2023        | SemiFinal1   | 15                   | FI          | Finland     | 1                 | Käärijä    | Cha Cha Cha    | 0.666667      | 12                  | 18           |
            | 2    | 2023        | GrandFinal   | 13                   | FI          | Finland     | 2                 | Käärijä    | Cha Cha Cha    | 0.438356      | 32                  | 73           |
            | 3    | 2023        | GrandFinal   | 9                    | SE          | Sweden      | 1                 | Loreen     | Tattoo         | 0.424658      | 31                  | 73           |
            | 4    | 2023        | SemiFinal1   | 1                    | NO          | Norway      | 6                 | Alessandra | Queen of Kings | 0.333333      | 6                   | 18           |
            | 4    | 2023        | SemiFinal1   | 11                   | SE          | Sweden      | 2                 | Loreen     | Tattoo         | 0.333333      | 6                   | 18           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any",
            minYear: 2023);
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 57,
            totalPages: 12);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_max_year(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
            maxPoints: 12,
            pageSize: 5,
            maxYear: 2022);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName                   | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|---------------------------|----------------|---------------|---------------------|--------------|
            | 1    | 2022        | SemiFinal2   | 17                   | SE          | Sweden      | 1                 | Cornelia Jakobs           | Hold Me Closer | 0.675         | 27                  | 40           |
            | 2    | 2022        | SemiFinal1   | 6                    | UA          | Ukraine     | 1                 | Kalush Orchestra          | Stefania       | 0.638889      | 23                  | 36           |
            | 3    | 2022        | GrandFinal   | 12                   | UA          | Ukraine     | 1                 | Kalush Orchestra          | Stefania       | 0.564103      | 44                  | 78           |
            | 4    | 2022        | SemiFinal2   | 8                    | AU          | Australia   | 2                 | Sheldon Riley             | Not The Same   | 0.325         | 13                  | 40           |
            | 5    | 2022        | SemiFinal1   | 15                   | GR          | Greece      | 3                 | Amanda Georgiadi Tenfjord | Die Together   | 0.305556      | 11                  | 36           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any",
            maxYear: 2022);
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 60,
            totalPages: 12);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_voting_method_any(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
            maxPoints: 12,
            pageSize: 5,
            votingMethod: "Any");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName                   | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|---------------------------|----------------|---------------|---------------------|--------------|
            | 1    | 2022        | SemiFinal2   | 17                   | SE          | Sweden      | 1                 | Cornelia Jakobs           | Hold Me Closer | 0.675         | 27                  | 40           |
            | 2    | 2023        | SemiFinal1   | 15                   | FI          | Finland     | 1                 | Käärijä                   | Cha Cha Cha    | 0.666667      | 12                  | 18           |
            | 3    | 2022        | SemiFinal1   | 6                    | UA          | Ukraine     | 1                 | Kalush Orchestra          | Stefania       | 0.638889      | 23                  | 36           |
            | 4    | 2022        | GrandFinal   | 12                   | UA          | Ukraine     | 1                 | Kalush Orchestra          | Stefania       | 0.564103      | 44                  | 78           |
            | 5    | 2023        | GrandFinal   | 13                   | FI          | Finland     | 2                 | Käärijä                   | Cha Cha Cha    | 0.438356      | 32                  | 73           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Any");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 117,
            totalPages: 24);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_voting_method_jury_only(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
            maxPoints: 12,
            pageSize: 5,
            votingMethod: "Jury");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName                   | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|---------------------------|----------------|---------------|---------------------|--------------|
            | 1    | 2022        | SemiFinal2   | 17                   | SE          | Sweden      | 1                 | Cornelia Jakobs           | Hold Me Closer | 0.85          | 17                  | 20           |
            | 2    | 2022        | SemiFinal2   | 8                    | AU          | Australia   | 2                 | Sheldon Riley             | Not The Same   | 0.65          | 13                  | 20           |
            | 3    | 2023        | GrandFinal   | 9                    | SE          | Sweden      | 1                 | Loreen                    | Tattoo         | 0.638889      | 23                  | 36           |
            | 4    | 2022        | SemiFinal1   | 15                   | GR          | Greece      | 3                 | Amanda Georgiadi Tenfjord | Die Together   | 0.444444      | 8                   | 18           |
            | 5    | 2022        | SemiFinal1   | 8                    | NL          | Netherlands | 2                 | S10                       | De Diepte      | 0.388889      | 7                   | 18           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Jury");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 86,
            totalPages: 18);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_voting_method_televote_only(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
            maxPoints: 12,
            pageSize: 5,
            votingMethod: "Televote");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle       | PointsInRange | PointsAwardsInRange | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|------------------|-----------------|---------------|---------------------|--------------|
            | 1    | 2022        | SemiFinal1   | 6                    | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania        | 0.944444      | 17                  | 18           |
            | 2    | 2022        | GrandFinal   | 12                   | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania        | 0.923077      | 36                  | 39           |
            | 3    | 2023        | GrandFinal   | 13                   | FI          | Finland     | 2                 | Käärijä          | Cha Cha Cha     | 0.72973       | 27                  | 37           |
            | 4    | 2023        | SemiFinal1   | 15                   | FI          | Finland     | 1                 | Käärijä          | Cha Cha Cha     | 0.666667      | 12                  | 18           |
            | 5    | 2022        | SemiFinal2   | 3                    | RS          | Serbia      | 3                 | Konstrakta       | In Corpore Sano | 0.55          | 11                  | 20           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
            contestStage: "Any",
            votingMethod: "Televote");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: false,
            totalItems: 117,
            totalPages: 24);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_filtering_by_multiple_facets_scenario_1(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
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
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName     | FinishingPosition | ActName        | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-----------------|-------------------|----------------|----------------|---------------|---------------------|--------------|
            | 1    | 2022        | SemiFinal2   | 4                    | AZ          | Azerbaijan      | 10                | Nadir Rustamli | Fade To Black  | 1             | 20                  | 20           |
            | 2    | 2022        | SemiFinal1   | 5                    | SI          | Slovenia        | 17                | LPS            | Disko          | 0.944444      | 17                  | 18           |
            | 3    | 2022        | SemiFinal2   | 15                   | ME          | Montenegro      | 17                | Vladana        | Breathe        | 0.9           | 18                  | 20           |
            | 4    | 2022        | SemiFinal2   | 11                   | MK          | North Macedonia | 11                | Andrea         | Circles        | 0.85          | 17                  | 20           |
            | 5    | 2022        | SemiFinal1   | 2                    | LV          | Latvia          | 14                | Citi Zēni      | Eat Your Salad | 0.833333      | 15                  | 18           |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
            minPoints: 5,
            maxPoints: 10,
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any",
            votingMethod: "Jury");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName               | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|-----------------------|----------------|---------------|---------------------|--------------|
            | 79   | 2022        | SemiFinal1   | 1                    | AL          | Albania     | 12                | Ronela Hajati         | Sekret         | 0             | 0                   | 18           |
            | 79   | 2022        | SemiFinal1   | 2                    | LV          | Latvia      | 14                | Citi Zēni             | Eat Your Salad | 0             | 0                   | 18           |
            | 79   | 2022        | SemiFinal1   | 13                   | AT          | Austria     | 15                | LUM!X feat. Pia Maria | Halo           | 0             | 0                   | 18           |
            | 79   | 2022        | SemiFinal2   | 9                    | CY          | Cyprus      | 12                | Andromache            | Ela            | 0             | 0                   | 20           |
            | 79   | 2022        | GrandFinal   | 13                   | DE          | Germany     | 25                | Malik Harris          | Rockstars      | 0             | 0                   | 39           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
            minPoints: 5,
            maxPoints: 10,
            minYear: 2016,
            maxYear: 2050,
            contestStage: "Any",
            votingMethod: "Jury");
        await euroFan.Then_the_response_pagination_metadata_should_match(
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 86,
            totalPages: 18);
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_empty_rankings_page_when_no_data_matches_filtering(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
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
            minPoints: 10,
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(
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

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetCompetitorPointsInRangeRankingsResponse>(apiDriver)
    {
        public void Given_I_want_to_obtain_a_page_of_competitor_points_in_range_rankings(bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingMethod = null,
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
                [nameof(maxYear)] = maxYear,
                [nameof(minYear)] = minYear,
                [nameof(contestStage)] = contestStage is not null ? Enum.Parse<QueryableContestStage>(contestStage) : null,
                [nameof(minPoints)] = minPoints,
                [nameof(maxPoints)] = maxPoints
            };

            Request = ApiDriver.RequestFactory.Rankings.GetCompetitorPointsInRangeRankings(queryParams);
        }

        public async Task Then_the_response_rankings_page_should_match(string page)
        {
            (CompetitorPointsInRangeRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetitorPointsInRangeRanking[] expected = MarkdownParser.ParseTable(page, MapRowToRanking).ToArray();

            await Assert.That(rankings).IsEquivalentTo(expected, CollectionOrdering.Matching);
        }

        public async Task Then_the_response_rankings_page_should_be_empty()
        {
            (CompetitorPointsInRangeRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(rankings).IsEmpty();
        }

        public async Task Then_the_response_filtering_metadata_should_match(string votingMethod = "",
            int? maxYear = null,
            int? minYear = null,
            string contestStage = "",
            int minPoints = 0,
            int maxPoints = 0)
        {
            (_, CompetitorPointsInRangeFilteringMetadata filtering, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetitorPointsInRangeFilteringMetadata expectedFiltering = new()
            {
                ContestStage = Enum.Parse<QueryableContestStage>(contestStage),
                MinPoints = minPoints,
                MaxPoints = maxPoints,
                MinYear = minYear,
                MaxYear = maxYear,
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

        private static CompetitorPointsInRangeRanking MapRowToRanking(Dictionary<string, string> row) => new()
        {
            Rank = int.Parse(row["Rank"]),
            ContestYear = int.Parse(row["ContestYear"]),
            ContestStage = Enum.Parse<ContestStage>(row["ContestStage"]),
            RunningOrderPosition = int.Parse(row["RunningOrderPosition"]),
            CountryCode = row["CountryCode"],
            CountryName = row["CountryName"],
            FinishingPosition = int.Parse(row["FinishingPosition"]),
            ActName = row["ActName"],
            SongTitle = row["SongTitle"],
            PointsInRange = decimal.Parse(row["PointsInRange"]),
            PointsAwardsInRange = int.Parse(row["PointsAwardsInRange"]),
            PointsAwards = int.Parse(row["PointsAwards"])
        };
    }
}

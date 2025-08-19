using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetitorPointsAverageRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Rankings;

public sealed class GetCompetitorPointsAverageRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_rankings_with_minimum_query_params(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle              | PointsAverage | TotalPoints | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|------------------|------------------------|---------------|-------------|--------------|
            | 1    | 2022        | SemiFinal2   | 17                   | SE          | Sweden      | 1                 | Cornelia Jakobs  | Hold Me Closer         | 9.9           | 396         | 40           |
            | 2    | 2023        | SemiFinal1   | 15                   | FI          | Finland     | 1                 | Käärijä          | Cha Cha Cha            | 9.833333      | 177         | 18           |
            | 3    | 2022        | SemiFinal1   | 6                    | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania               | 9.361111      | 337         | 36           |
            | 4    | 2022        | GrandFinal   | 12                   | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania               | 8.089744      | 631         | 78           |
            | 5    | 2023        | GrandFinal   | 9                    | SE          | Sweden      | 1                 | Loreen           | Tattoo                 | 7.986301      | 583         | 73           |
            | 6    | 2023        | SemiFinal2   | 16                   | AU          | Australia   | 1                 | Voyager          | Promise                | 7.842105      | 149         | 19           |
            | 7    | 2023        | SemiFinal1   | 11                   | SE          | Sweden      | 2                 | Loreen           | Tattoo                 | 7.5           | 135         | 18           |
            | 8    | 2023        | SemiFinal2   | 13                   | AT          | Austria     | 2                 | Teya & Salena    | Who The Hell Is Edgar? | 7.210526      | 137         | 19           |
            | 9    | 2023        | GrandFinal   | 13                   | FI          | Finland     | 2                 | Käärijä          | Cha Cha Cha            | 7.205479      | 526         | 73           |
            | 10   | 2023        | SemiFinal1   | 9                    | IL          | Israel      | 3                 | Noa Kirel        | Unicorn                | 7.055556      | 127         | 18           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(
            pageIndex: 1,
            pageSize: 3,
            descending: true);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName      | SongTitle             | PointsAverage | TotalPoints | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|--------------|-----------------------|---------------|-------------|--------------|
            | 114  | 2023        | SemiFinal1   | 2                    | MT          | Malta       | 15                | The Busker   | Dance (Our Own Party) | 0.166667      | 3           | 18           |
            | 113  | 2022        | GrandFinal   | 6                    | FR          | France      | 24                | Alvan & Ahez | Fulenn                | 0.217949      | 17          | 78           |
            | 112  | 2023        | SemiFinal1   | 12                   | AZ          | Azerbaijan  | 14                | TuralTuranX  | Tell Me More          | 0.222222      | 4           | 18           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(
            pageSize: 5,
            contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName    | FinishingPosition | ActName          | SongTitle   | PointsAverage | TotalPoints | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|----------------|-------------------|------------------|-------------|---------------|-------------|--------------|
            | 1    | 2022        | GrandFinal   | 12                   | UA          | Ukraine        | 1                 | Kalush Orchestra | Stefania    | 8.089744      | 631         | 78           |
            | 2    | 2023        | GrandFinal   | 9                    | SE          | Sweden         | 1                 | Loreen           | Tattoo      | 7.986301      | 583         | 73           |
            | 3    | 2023        | GrandFinal   | 13                   | FI          | Finland        | 2                 | Käärijä          | Cha Cha Cha | 7.205479      | 526         | 73           |
            | 4    | 2022        | GrandFinal   | 22                   | GB          | United Kingdom | 2                 | Sam Ryder        | SPACE MAN   | 5.974359      | 466         | 78           |
            | 5    | 2022        | GrandFinal   | 10                   | ES          | Spain          | 3                 | Chanel           | SloMo       | 5.884615      | 459         | 78           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(
            pageSize: 5,
            minYear: 2023);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName       | SongTitle              | PointsAverage | TotalPoints | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|---------------|------------------------|---------------|-------------|--------------|
            | 1    | 2023        | SemiFinal1   | 15                   | FI          | Finland     | 1                 | Käärijä       | Cha Cha Cha            | 9.833333      | 177         | 18           |
            | 2    | 2023        | GrandFinal   | 9                    | SE          | Sweden      | 1                 | Loreen        | Tattoo                 | 7.986301      | 583         | 73           |
            | 3    | 2023        | SemiFinal2   | 16                   | AU          | Australia   | 1                 | Voyager       | Promise                | 7.842105      | 149         | 19           |
            | 4    | 2023        | SemiFinal1   | 11                   | SE          | Sweden      | 2                 | Loreen        | Tattoo                 | 7.5           | 135         | 18           |
            | 5    | 2023        | SemiFinal2   | 13                   | AT          | Austria     | 2                 | Teya & Salena | Who The Hell Is Edgar? | 7.210526      | 137         | 19           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(
            pageSize: 5,
            maxYear: 2022);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle      | PointsAverage | TotalPoints | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|------------------|----------------|---------------|-------------|--------------|
            | 1    | 2022        | SemiFinal2   | 17                   | SE          | Sweden      | 1                 | Cornelia Jakobs  | Hold Me Closer | 9.9           | 396         | 40           |
            | 2    | 2022        | SemiFinal1   | 6                    | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania       | 9.361111      | 337         | 36           |
            | 3    | 2022        | GrandFinal   | 12                   | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania       | 8.089744      | 631         | 78           |
            | 4    | 2022        | SemiFinal1   | 8                    | NL          | Netherlands | 2                 | S10              | De Diepte      | 6.138889      | 221         | 36           |
            | 5    | 2022        | SemiFinal2   | 8                    | AU          | Australia   | 2                 | Sheldon Riley    | Not The Same   | 6.075         | 243         | 40           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(
            pageSize: 5,
            votingMethod: "Any");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle              | PointsAverage | TotalPoints | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|------------------|------------------------|---------------|-------------|--------------|
            | 1    | 2022        | SemiFinal2   | 17                   | SE          | Sweden      | 1                 | Cornelia Jakobs  | Hold Me Closer         | 9.9           | 396         | 40           |
            | 2    | 2023        | SemiFinal1   | 15                   | FI          | Finland     | 1                 | Käärijä          | Cha Cha Cha            | 9.833333      | 177         | 18           |
            | 3    | 2022        | SemiFinal1   | 6                    | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania               | 9.361111      | 337         | 36           |
            | 4    | 2022        | GrandFinal   | 12                   | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania               | 8.089744      | 631         | 78           |
            | 5    | 2023        | GrandFinal   | 9                    | SE          | Sweden      | 1                 | Loreen           | Tattoo                 | 7.986301      | 583         | 73           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(
            pageSize: 5,
            votingMethod: "Jury");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName                   | SongTitle      | PointsAverage | TotalPoints | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|---------------------------|----------------|---------------|-------------|--------------|
            | 1    | 2022        | SemiFinal2   | 17                   | SE          | Sweden      | 1                 | Cornelia Jakobs           | Hold Me Closer | 11.1          | 222         | 20           |
            | 2    | 2023        | GrandFinal   | 9                    | SE          | Sweden      | 1                 | Loreen                    | Tattoo         | 9.444444      | 340         | 36           |
            | 3    | 2022        | SemiFinal2   | 8                    | AU          | Australia   | 2                 | Sheldon Riley             | Not The Same   | 8.45          | 169         | 20           |
            | 4    | 2022        | SemiFinal1   | 15                   | GR          | Greece      | 3                 | Amanda Georgiadi Tenfjord | Die Together   | 8.388889      | 151         | 18           |
            | 5    | 2022        | SemiFinal1   | 8                    | NL          | Netherlands | 2                 | S10                       | De Diepte      | 7.888889      | 142         | 18           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(
            pageSize: 5,
            votingMethod: "Televote");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle       | PointsAverage | TotalPoints | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|------------------|-----------------|---------------|-------------|--------------|
            | 1    | 2022        | GrandFinal   | 12                   | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania        | 11.25641      | 439         | 39           |
            | 2    | 2022        | SemiFinal1   | 6                    | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania        | 11.222222     | 202         | 18           |
            | 3    | 2023        | GrandFinal   | 13                   | FI          | Finland     | 2                 | Käärijä          | Cha Cha Cha     | 10.162162     | 376         | 37           |
            | 4    | 2023        | SemiFinal1   | 15                   | FI          | Finland     | 1                 | Käärijä          | Cha Cha Cha     | 9.833333      | 177         | 18           |
            | 5    | 2022        | SemiFinal2   | 3                    | RS          | Serbia      | 3                 | Konstrakta       | In Corpore Sano | 8.7           | 174         | 20           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(
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
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName                         | SongTitle       | PointsAverage | TotalPoints | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|---------------------------------|-----------------|---------------|-------------|--------------|
            | 1    | 2022        | SemiFinal1   | 6                    | UA          | Ukraine     | 1                 | Kalush Orchestra                | Stefania        | 11.222222     | 202         | 18           |
            | 2    | 2022        | SemiFinal2   | 3                    | RS          | Serbia      | 3                 | Konstrakta                      | In Corpore Sano | 8.7           | 174         | 20           |
            | 2    | 2022        | SemiFinal2   | 17                   | SE          | Sweden      | 1                 | Cornelia Jakobs                 | Hold Me Closer  | 8.7           | 174         | 20           |
            | 4    | 2022        | SemiFinal1   | 9                    | MD          | Moldova     | 8                 | Zdob şi Zdub & Advahov Brothers | Trenulețul      | 7.5           | 135         | 18           |
            | 5    | 2022        | SemiFinal2   | 18                   | CZ          | Czechia     | 4                 | We Are Domi                     | Lights Off      | 6.25          | 125         | 20           |
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(
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
            | Rank | ContestYear | ContestStage | RunningOrderPosition | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle         | PointsAverage | TotalPoints | PointsAwards |
            |------|-------------|--------------|----------------------|-------------|-------------|-------------------|------------------|-------------------|---------------|-------------|--------------|
            | 86   | 2022        | GrandFinal   | 13                   | DE          | Germany     | 25                | Malik Harris     | Rockstars         | 0             | 0           | 39           |
            | 85   | 2023        | GrandFinal   | 21                   | DE          | Germany     | 26                | Lord of the Lost | Blood & Glitter   | 0.083333      | 3           | 36           |
            | 84   | 2022        | GrandFinal   | 6                    | FR          | France      | 24                | Alvan & Ahez     | Fulenn            | 0.230769      | 9           | 39           |
            | 83   | 2022        | GrandFinal   | 18                   | IS          | Iceland     | 23                | Systur           | Með Hækkandi Sól  | 0.25641       | 10          | 39           |
            | 82   | 2023        | GrandFinal   | 25                   | HR          | Croatia     | 13                | Let 3            | Mama ŠČ!          | 0.305556      | 11          | 36           |
            """);
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(
            minYear: 2050,
            maxYear: 2016,
            votingMethod: "Any",
            contestStage: "Any");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_response_rankings_page_should_be_empty();
        await euroFan.Then_the_response_filtering_metadata_should_match(
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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(pageIndex: -1);

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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(pageSize: 0);

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
        euroFan.Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(pageSize: 101);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_FAIL_with_status_code_400_BadRequest();
        await euroFan.Then_the_response_problem_details_should_match(status: 400,
            title: "Page size out of range",
            detail: "Query parameter 'pageSize' value must be an integer between 1 and 100.");
        await euroFan.Then_the_response_problem_details_extensions_should_include("pageSize", 101);
    }

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetCompetitorPointsAverageRankingsResponse>(apiDriver)
    {
        public void Given_I_want_to_obtain_a_page_of_competitor_points_average_rankings(bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingMethod = null,
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
                [nameof(maxYear)] = maxYear,
                [nameof(minYear)] = minYear,
                [nameof(contestStage)] = contestStage is not null ? Enum.Parse<QueryableContestStage>(contestStage) : null
            };

            Request = ApiDriver.RequestFactory.Rankings.GetCompetitorPointsAverageRankings(queryParams);
        }

        public async Task Then_the_response_rankings_page_should_match(string page)
        {
            (CompetitorPointsAverageRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetitorPointsAverageRanking[] expected = MarkdownParser.ParseTable(page, MapRowToRanking).ToArray();

            await Assert.That(rankings).IsEquivalentTo(expected, CollectionOrdering.Matching);
        }

        public async Task Then_the_response_rankings_page_should_be_empty()
        {
            (CompetitorPointsAverageRanking[] rankings, _, _) = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(rankings).IsEmpty();
        }

        public async Task Then_the_response_filtering_metadata_should_match(string votingMethod = "",
            int? maxYear = null,
            int? minYear = null,
            string contestStage = "")
        {
            (_, CompetitorPointsAverageFilteringMetadata filtering, _) = await Assert.That(ResponseBody).IsNotNull();

            CompetitorPointsAverageFilteringMetadata expectedFiltering = new()
            {
                ContestStage = Enum.Parse<QueryableContestStage>(contestStage),
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

        private static CompetitorPointsAverageRanking MapRowToRanking(Dictionary<string, string> row) => new()
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
            PointsAverage = decimal.Parse(row["PointsAverage"]),
            TotalPoints = int.Parse(row["TotalPoints"]),
            PointsAwards = int.Parse(row["PointsAwards"])
        };
    }
}

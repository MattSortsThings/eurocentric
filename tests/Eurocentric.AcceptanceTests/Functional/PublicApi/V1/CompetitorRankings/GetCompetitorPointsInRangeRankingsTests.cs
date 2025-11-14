using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.CompetitorRankings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.CompetitorRankings;

[Category("public-api")]
public sealed class GetCompetitorPointsInRangeRankingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(minPoints: 10, maxPoints: 12);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName                   | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|---------------------------|----------------|---------------|---------------------|--------------|-----------------|
            | 1    | 2022        | SemiFinal2   | 17               | SE          | Sweden      | 1                 | Cornelia Jakobs           | Hold Me Closer | 0.675         | 27                  | 40           | 20              |
            | 2    | 2023        | SemiFinal1   | 15               | FI          | Finland     | 1                 | Käärijä                   | Cha Cha Cha    | 0.666667      | 12                  | 18           | 18              |
            | 3    | 2022        | SemiFinal1   | 6                | UA          | Ukraine     | 1                 | Kalush Orchestra          | Stefania       | 0.638889      | 23                  | 36           | 18              |
            | 4    | 2022        | GrandFinal   | 12               | UA          | Ukraine     | 1                 | Kalush Orchestra          | Stefania       | 0.564103      | 44                  | 78           | 39              |
            | 5    | 2023        | GrandFinal   | 13               | FI          | Finland     | 2                 | Käärijä                   | Cha Cha Cha    | 0.438356      | 32                  | 73           | 37              |
            | 6    | 2023        | GrandFinal   | 9                | SE          | Sweden      | 1                 | Loreen                    | Tattoo         | 0.424658      | 31                  | 73           | 37              |
            | 7    | 2023        | SemiFinal1   | 1                | NO          | Norway      | 6                 | Alessandra                | Queen of Kings | 0.333333      | 6                   | 18           | 18              |
            | 7    | 2023        | SemiFinal1   | 11               | SE          | Sweden      | 2                 | Loreen                    | Tattoo         | 0.333333      | 6                   | 18           | 18              |
            | 9    | 2022        | SemiFinal2   | 8                | AU          | Australia   | 2                 | Sheldon Riley             | Not The Same   | 0.325         | 13                  | 40           | 20              |
            | 10   | 2022        | SemiFinal1   | 15               | GR          | Greece      | 3                 | Amanda Georgiadi Tenfjord | Die Together   | 0.305556      | 11                  | 36           | 18              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
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
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName                   | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|---------------------------|----------------|---------------|---------------------|--------------|-----------------|
            | 7    | 2023        | SemiFinal1   | 1                | NO          | Norway      | 6                 | Alessandra                | Queen of Kings | 0.333333      | 6                   | 18           | 18              |
            | 7    | 2023        | SemiFinal1   | 11               | SE          | Sweden      | 2                 | Loreen                    | Tattoo         | 0.333333      | 6                   | 18           | 18              |
            | 9    | 2022        | SemiFinal2   | 8                | AU          | Australia   | 2                 | Sheldon Riley             | Not The Same   | 0.325         | 13                  | 40           | 20              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
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
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName               | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|-----------------------|----------------|---------------|---------------------|--------------|-----------------|
            | 95   | 2022        | SemiFinal1   | 5                | SI          | Slovenia    | 17                | LPS                   | Disko          | 0             | 0                   | 36           | 18              |
            | 95   | 2022        | SemiFinal1   | 12               | DK          | Denmark     | 13                | REDDI                 | The Show       | 0             | 0                   | 36           | 18              |
            | 95   | 2022        | SemiFinal1   | 13               | AT          | Austria     | 15                | LUM!X feat. Pia Maria | Halo           | 0             | 0                   | 36           | 18              |
            | 95   | 2022        | SemiFinal2   | 5                | GE          | Georgia     | 18                | Circus Mircus         | Lock Me In     | 0             | 0                   | 40           | 20              |
            | 95   | 2022        | SemiFinal2   | 6                | MT          | Malta       | 16                | Emma Muscat           | I Am What I Am | 0             | 0                   | 40           | 20              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
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
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName                   | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|---------------------------|----------------|---------------|---------------------|--------------|-----------------|
            | 1    | 2022        | SemiFinal2   | 17               | SE          | Sweden      | 1                 | Cornelia Jakobs           | Hold Me Closer | 0.675         | 27                  | 40           | 20              |
            | 2    | 2022        | SemiFinal1   | 6                | UA          | Ukraine     | 1                 | Kalush Orchestra          | Stefania       | 0.638889      | 23                  | 36           | 18              |
            | 3    | 2022        | GrandFinal   | 12               | UA          | Ukraine     | 1                 | Kalush Orchestra          | Stefania       | 0.564103      | 44                  | 78           | 39              |
            | 4    | 2022        | SemiFinal2   | 8                | AU          | Australia   | 2                 | Sheldon Riley             | Not The Same   | 0.325         | 13                  | 40           | 20              |
            | 5    | 2022        | SemiFinal1   | 15               | GR          | Greece      | 3                 | Amanda Georgiadi Tenfjord | Die Together   | 0.305556      | 11                  | 36           | 18              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
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
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------------|----------------|---------------|---------------------|--------------|-----------------|
            | 1    | 2022        | SemiFinal2   | 17               | SE          | Sweden      | 1                 | Cornelia Jakobs  | Hold Me Closer | 0.675         | 27                  | 40           | 20              |
            | 2    | 2023        | SemiFinal1   | 15               | FI          | Finland     | 1                 | Käärijä          | Cha Cha Cha    | 0.666667      | 12                  | 18           | 18              |
            | 3    | 2022        | SemiFinal1   | 6                | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania       | 0.638889      | 23                  | 36           | 18              |
            | 4    | 2023        | SemiFinal1   | 1                | NO          | Norway      | 6                 | Alessandra       | Queen of Kings | 0.333333      | 6                   | 18           | 18              |
            | 4    | 2023        | SemiFinal1   | 11               | SE          | Sweden      | 2                 | Loreen           | Tattoo         | 0.333333      | 6                   | 18           | 18              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
            minPoints: 10,
            maxPoints: 12,
            competingCountryCode: "FI",
            pageSize: 5
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName    | SongTitle   | PointsInRange | PointsAwardsInRange | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------|-------------|---------------|---------------------|--------------|-----------------|
            | 1    | 2023        | SemiFinal1   | 15               | FI          | Finland     | 1                 | Käärijä    | Cha Cha Cha | 0.666667      | 12                  | 18           | 18              |
            | 2    | 2023        | GrandFinal   | 13               | FI          | Finland     | 2                 | Käärijä    | Cha Cha Cha | 0.438356      | 32                  | 73           | 37              |
            | 3    | 2022        | SemiFinal2   | 1                | FI          | Finland     | 7                 | The Rasmus | Jezebel     | 0.075         | 3                   | 40           | 20              |
            | 4    | 2022        | GrandFinal   | 4                | FI          | Finland     | 21                | The Rasmus | Jezebel     | 0             | 0                   | 78           | 39              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
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
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName                   | SongTitle      | PointsInRange | PointsAwardsInRange | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|---------------------------|----------------|---------------|---------------------|--------------|-----------------|
            | 1    | 2022        | SemiFinal2   | 17               | SE          | Sweden      | 1                 | Cornelia Jakobs           | Hold Me Closer | 0.85          | 17                  | 20           | 20              |
            | 2    | 2022        | SemiFinal2   | 8                | AU          | Australia   | 2                 | Sheldon Riley             | Not The Same   | 0.65          | 13                  | 20           | 20              |
            | 3    | 2023        | GrandFinal   | 9                | SE          | Sweden      | 1                 | Loreen                    | Tattoo         | 0.638889      | 23                  | 36           | 36              |
            | 4    | 2022        | SemiFinal1   | 15               | GR          | Greece      | 3                 | Amanda Georgiadi Tenfjord | Die Together   | 0.444444      | 8                   | 18           | 18              |
            | 5    | 2022        | SemiFinal1   | 8                | NL          | Netherlands | 2                 | S10                       | De Diepte      | 0.388889      | 7                   | 18           | 18              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
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
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName          | SongTitle       | PointsInRange | PointsAwardsInRange | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|------------------|-----------------|---------------|---------------------|--------------|-----------------|
            | 1    | 2022        | SemiFinal1   | 6                | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania        | 0.944444      | 17                  | 18           | 18              |
            | 2    | 2022        | GrandFinal   | 12               | UA          | Ukraine     | 1                 | Kalush Orchestra | Stefania        | 0.923077      | 36                  | 39           | 39              |
            | 3    | 2023        | GrandFinal   | 13               | FI          | Finland     | 2                 | Käärijä          | Cha Cha Cha     | 0.72973       | 27                  | 37           | 37              |
            | 4    | 2023        | SemiFinal1   | 15               | FI          | Finland     | 1                 | Käärijä          | Cha Cha Cha     | 0.666667      | 12                  | 18           | 18              |
            | 5    | 2022        | SemiFinal2   | 3                | RS          | Serbia      | 3                 | Konstrakta       | In Corpore Sano | 0.55          | 11                  | 20           | 20              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 10,
            maxPoints: 12,
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
            minPoints: 12,
            maxPoints: 12,
            minYear: 2023,
            contestStage: "Any",
            votingMethod: "Televote",
            pageSize: 5,
            descending: false
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName   | SongTitle   | PointsInRange | PointsAwardsInRange | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|-----------|-------------|---------------|---------------------|--------------|-----------------|
            | 1    | 2023        | GrandFinal   | 13               | FI          | Finland     | 2                 | Käärijä   | Cha Cha Cha | 0.486486      | 18                  | 37           | 37              |
            | 2    | 2023        | SemiFinal1   | 15               | FI          | Finland     | 1                 | Käärijä   | Cha Cha Cha | 0.388889      | 7                   | 18           | 18              |
            | 3    | 2023        | SemiFinal1   | 9                | IL          | Israel      | 3                 | Noa Kirel | Unicorn     | 0.222222      | 4                   | 18           | 18              |
            | 4    | 2023        | SemiFinal2   | 10               | SI          | Slovenia    | 5                 | Joker Out | Carpe Diem  | 0.157895      | 3                   | 19           | 19              |
            | 4    | 2023        | SemiFinal2   | 16               | AU          | Australia   | 1                 | Voyager   | Promise     | 0.157895      | 3                   | 19           | 19              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 12,
            maxPoints: 12,
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
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_10(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
            minPoints: 12,
            maxPoints: 12,
            minYear: 2023,
            contestStage: "Any",
            votingMethod: "Televote",
            pageSize: 5,
            descending: true
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName       | SongTitle             | PointsInRange | PointsAwardsInRange | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|---------------|-----------------------|---------------|---------------------|--------------|-----------------|
            | 32   | 2023        | SemiFinal1   | 1                | NO          | Norway      | 6                 | Alessandra    | Queen of Kings        | 0             | 0                   | 18           | 18              |
            | 32   | 2023        | SemiFinal1   | 2                | MT          | Malta       | 15                | The Busker    | Dance (Our Own Party) | 0             | 0                   | 18           | 18              |
            | 32   | 2023        | SemiFinal1   | 3                | RS          | Serbia      | 10                | Luke Black    | Samo Mi Se Spava      | 0             | 0                   | 18           | 18              |
            | 32   | 2023        | SemiFinal1   | 4                | LV          | Latvia      | 11                | Sudden Lights | Aijā                  | 0             | 0                   | 18           | 18              |
            | 32   | 2023        | SemiFinal1   | 6                | IE          | Ireland     | 12                | Wild Youth    | We Are One            | 0             | 0                   | 18           | 18              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 12,
            maxPoints: 12,
            minYear: 2023,
            contestStage: "Any",
            votingMethod: "Televote",
            pageIndex: 0,
            pageSize: 5,
            descending: true,
            totalItems: 57,
            totalPages: 12
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_11(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 10,
            minYear: 2023,
            contestStage: "Any",
            votingMethod: "Televote",
            pageSize: 5,
            descending: false
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName       | SongTitle             | PointsInRange | PointsAwardsInRange | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|---------------|-----------------------|---------------|---------------------|--------------|-----------------|
            | 1    | 2023        | SemiFinal1   | 1                | NO          | Norway      | 6                 | Alessandra    | Queen of Kings        | 1             | 18                  | 18           | 18              |
            | 1    | 2023        | SemiFinal1   | 2                | MT          | Malta       | 15                | The Busker    | Dance (Our Own Party) | 1             | 18                  | 18           | 18              |
            | 1    | 2023        | SemiFinal1   | 3                | RS          | Serbia      | 10                | Luke Black    | Samo Mi Se Spava      | 1             | 18                  | 18           | 18              |
            | 1    | 2023        | SemiFinal1   | 4                | LV          | Latvia      | 11                | Sudden Lights | Aijā                  | 1             | 18                  | 18           | 18              |
            | 1    | 2023        | SemiFinal1   | 6                | IE          | Ireland     | 12                | Wild Youth    | We Are One            | 1             | 18                  | 18           | 18              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 0,
            maxPoints: 10,
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
    public async Task Should_retrieve_rankings_page_and_metadata_scenario_12(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
            minPoints: 0,
            maxPoints: 10,
            minYear: 2023,
            contestStage: "Any",
            votingMethod: "Televote",
            pageSize: 5,
            descending: true
        );

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_rankings_in_order_should_match(
            """
            | Rank | ContestYear | ContestStage | RunningOrderSpot | CountryCode | CountryName | FinishingPosition | ActName   | SongTitle   | PointsInRange | PointsAwardsInRange | PointsAwards | VotingCountries |
            |------|-------------|--------------|------------------|-------------|-------------|-------------------|-----------|-------------|---------------|---------------------|--------------|-----------------|
            | 57   | 2023        | GrandFinal   | 13               | FI          | Finland     | 2                 | Käärijä   | Cha Cha Cha | 0.513514      | 19                  | 37           | 37              |
            | 56   | 2023        | SemiFinal1   | 15               | FI          | Finland     | 1                 | Käärijä   | Cha Cha Cha | 0.611111      | 11                  | 18           | 18              |
            | 55   | 2023        | SemiFinal1   | 9                | IL          | Israel      | 3                 | Noa Kirel | Unicorn     | 0.777778      | 14                  | 18           | 18              |
            | 53   | 2023        | SemiFinal2   | 10               | SI          | Slovenia    | 5                 | Joker Out | Carpe Diem  | 0.842105      | 16                  | 19           | 19              |
            | 53   | 2023        | SemiFinal2   | 16               | AU          | Australia   | 1                 | Voyager   | Promise     | 0.842105      | 16                  | 19           | 19              |
            """
        );
        await euroFan.Then_the_retrieved_metadata_should_match(
            minPoints: 0,
            maxPoints: 10,
            minYear: 2023,
            contestStage: "Any",
            votingMethod: "Televote",
            pageIndex: 0,
            pageSize: 5,
            descending: true,
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(minYear: 1066, maxYear: 1963);

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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
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
    public async Task Should_fail_on_illegal_competing_country_code_value(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
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
        euroFan.Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(minPoints: 12, maxPoints: 1);

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

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetCompetitorPointsInRangeRankingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_a_page_of_competitor_points_in_range_rankings(
            bool? descending = null,
            int? pageSize = null,
            int? pageIndex = null,
            string? votingMethod = null,
            string? competingCountryCode = null,
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

            Request = Kernel.Requests.CompetitorRankings.GetCompetitorPointsInRangeRankings(queryParams);
        }

        public async Task Then_the_retrieved_rankings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Rankings).IsEmpty();

        public async Task Then_the_retrieved_rankings_in_order_should_match(string table)
        {
            CompetitorPointsInRangeRanking[] expected = MarkdownParser.ParseTable(table, MapToRanking);

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
            int? minYear = null,
            int maxPoints = 0,
            int minPoints = 0
        )
        {
            ContestStageFilter? expectedContestStage = contestStage.ToNullableContestStageFilter();
            VotingMethodFilter? expectedVotingMethod = votingMethod.ToNullableVotingMethodFilter();

            await Assert
                .That(SuccessResponse?.Data?.Metadata)
                .HasProperty(cp => cp.MinPoints, minPoints)
                .And.HasProperty(cp => cp.MaxPoints, maxPoints)
                .And.HasProperty(cp => cp.MinYear, minYear)
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

        private static CompetitorPointsInRangeRanking MapToRanking(Dictionary<string, string> row)
        {
            return new CompetitorPointsInRangeRanking
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
                PointsInRange = decimal.Parse(row["PointsInRange"]),
                PointsAwardsInRange = int.Parse(row["PointsAwardsInRange"]),
                PointsAwards = int.Parse(row["PointsAwards"]),
                VotingCountries = int.Parse(row["VotingCountries"]),
            };
        }

        private sealed class RankingEqualityComparer : IEqualityComparer<CompetitorPointsInRangeRanking>
        {
            public bool Equals(CompetitorPointsInRangeRanking? x, CompetitorPointsInRangeRanking? y)
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
                    && x.PointsInRange == y.PointsInRange
                    && x.PointsAwardsInRange == y.PointsAwardsInRange
                    && x.PointsAwards == y.PointsAwards
                    && x.VotingCountries == y.VotingCountries;
            }

            public int GetHashCode(CompetitorPointsInRangeRanking obj)
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
                hashCode.Add(obj.PointsInRange);
                hashCode.Add(obj.PointsAwardsInRange);
                hashCode.Add(obj.PointsAwards);
                hashCode.Add(obj.VotingCountries);

                return hashCode.ToHashCode();
            }
        }
    }
}

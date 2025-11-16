using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.Listings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.Listings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.Listings;

[Category("public-api")]
public sealed class GetCompetingCountryResultListingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_result_listings_with_the_competing_country_code("FI");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_comprise_the_competing_country_code("FI");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | ContestYear | ContestStage | RunningOrderSpot | ActName    | SongTitle   | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition | Competitors |
            |-------------|--------------|------------------|------------|-------------|------------|----------|----------------|--------------|---------------|-------------------|-------------|
            | 2022        | SemiFinal2   | 1                | The Rasmus | Jezebel     | 63         | 8        | 99             | 6            | 162           | 7                 | 18          |
            | 2022        | GrandFinal   | 4                | The Rasmus | Jezebel     | 12         | 21       | 26             | 16           | 38            | 21                | 25          |
            | 2023        | SemiFinal1   | 15               | Käärijä    | Cha Cha Cha |            |          | 177            | 1            | 177           | 1                 | 15          |
            | 2023        | GrandFinal   | 13               | Käärijä    | Cha Cha Cha | 150        | 4        | 376            | 1            | 526           | 2                 | 26          |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_result_listings_with_the_competing_country_code("ME");
        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_comprise_the_competing_country_code("ME");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | ContestYear | ContestStage | RunningOrderSpot | ActName | SongTitle | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition | Competitors |
            |-------------|--------------|------------------|---------|-----------|------------|----------|----------------|--------------|---------------|-------------------|-------------|
            | 2022        | SemiFinal2   | 15               | Vladana | Breathe   | 11         | 17       | 22             | 14           | 33            | 17                | 18          |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_3(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_result_listings_with_the_competing_country_code("GB");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_comprise_the_competing_country_code("GB");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | ContestYear | ContestStage | RunningOrderSpot | ActName    | SongTitle      | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition | Competitors |
            |-------------|--------------|------------------|------------|----------------|------------|----------|----------------|--------------|---------------|-------------------|-------------|
            | 2022        | GrandFinal   | 22               | Sam Ryder  | SPACE MAN      | 283        | 1        | 183            | 5            | 466           | 2                 | 25          |
            | 2023        | GrandFinal   | 26               | Mae Muller | I Wrote A Song | 15         | 22       | 9              | 25           | 24            | 25                | 26          |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_4(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_result_listings_with_the_competing_country_code("DE");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_comprise_the_competing_country_code("DE");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | ContestYear | ContestStage | RunningOrderSpot | ActName          | SongTitle       | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition | Competitors |
            |-------------|--------------|------------------|------------------|-----------------|------------|----------|----------------|--------------|---------------|-------------------|-------------|
            | 2022        | GrandFinal   | 13               | Malik Harris     | Rockstars       | 0          | 25       | 6              | 20           | 6             | 25                | 25          |
            | 2023        | GrandFinal   | 21               | Lord of the Lost | Blood & Glitter | 3          | 26       | 15             | 24           | 18            | 26                | 26          |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_5(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_result_listings_with_the_competing_country_code("AT");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_comprise_the_competing_country_code("AT");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | ContestYear | ContestStage | RunningOrderSpot | ActName               | SongTitle              | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition | Competitors |
            |-------------|--------------|------------------|-----------------------|------------------------|------------|----------|----------------|--------------|---------------|-------------------|-------------|
            | 2022        | SemiFinal1   | 13               | LUM!X feat. Pia Maria | Halo                   | 6          | 17       | 36             | 11           | 42            | 15                | 17          |
            | 2023        | SemiFinal2   | 13               | Teya & Salena         | Who The Hell Is Edgar? |            |          | 137            | 2            | 137           | 2                 | 16          |
            | 2023        | GrandFinal   | 1                | Teya & Salena         | Who The Hell Is Edgar? | 104        | 8        | 16             | 21           | 120           | 15                | 26          |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_6(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_result_listings_with_the_competing_country_code("CZ");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_comprise_the_competing_country_code("CZ");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | ContestYear | ContestStage | RunningOrderSpot | ActName     | SongTitle         | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition | Competitors |
            |-------------|--------------|------------------|-------------|-------------------|------------|----------|----------------|--------------|---------------|-------------------|-------------|
            | 2022        | SemiFinal2   | 18               | We Are Domi | Lights Off        | 102        | 5        | 125            | 3            | 227           | 4                 | 18          |
            | 2022        | GrandFinal   | 1                | We Are Domi | Lights Off        | 33         | 19       | 5              | 21           | 38            | 22                | 25          |
            | 2023        | SemiFinal1   | 13               | Vesna       | My Sister's Crown |            |          | 110            | 4            | 110           | 4                 | 15          |
            | 2023        | GrandFinal   | 14               | Vesna       | My Sister's Crown | 94         | 10       | 35             | 17           | 129           | 10                | 26          |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_7(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_result_listings_with_the_competing_country_code("UA");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_comprise_the_competing_country_code("UA");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | ContestYear | ContestStage | RunningOrderSpot | ActName          | SongTitle      | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition | Competitors |
            |-------------|--------------|------------------|------------------|----------------|------------|----------|----------------|--------------|---------------|-------------------|-------------|
            | 2022        | SemiFinal1   | 6                | Kalush Orchestra | Stefania       | 135        | 3        | 202            | 1            | 337           | 1                 | 17          |
            | 2022        | GrandFinal   | 12               | Kalush Orchestra | Stefania       | 192        | 4        | 439            | 1            | 631           | 1                 | 25          |
            | 2023        | GrandFinal   | 19               | TVORCHI          | Heart Of Steel | 54         | 15       | 189            | 4            | 243           | 6                 | 26          |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_listings_when_no_data_matches_query_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_result_listings_with_the_competing_country_code("ZZ");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_comprise_the_competing_country_code("ZZ");
        await euroFan.Then_the_retrieved_listings_should_be_an_empty_list();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_listings_when_no_data_matches_query_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_result_listings_with_the_competing_country_code("XX");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_comprise_the_competing_country_code("XX");
        await euroFan.Then_the_retrieved_listings_should_be_an_empty_list();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_illegal_competing_country_code_value(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_competing_country_result_listings_with_the_competing_country_code("!");

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

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetCompetingCountryResultListingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_competing_country_result_listings_with_the_competing_country_code(
            string competingCountryCode
        )
        {
            Dictionary<string, object?> queryParams = new() { { nameof(competingCountryCode), competingCountryCode } };

            Request = Kernel.Requests.Listings.GetCompetingCountryResultListings(queryParams);
        }

        public async Task Then_the_retrieved_listings_in_order_should_match(string table)
        {
            CompetingCountryResultListing[] expectedListings = MarkdownParser.ParseTable(table, MapToResultListing);

            await Assert
                .That(SuccessResponse?.Data?.Listings)
                .IsEquivalentTo(expectedListings, new ListingEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_listings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Listings).IsEmpty();

        public async Task Then_the_retrieved_metadata_should_comprise_the_competing_country_code(
            string competingCountryCode
        )
        {
            await Assert
                .That(SuccessResponse?.Data?.Metadata)
                .HasProperty(cc => cc.CompetingCountryCode, competingCountryCode);
        }

        private static CompetingCountryResultListing MapToResultListing(Dictionary<string, string> row)
        {
            return new CompetingCountryResultListing
            {
                ContestYear = int.Parse(row["ContestYear"]),
                ContestStage = Enum.Parse<ContestStage>(row["ContestStage"]),
                RunningOrderSpot = int.Parse(row["RunningOrderSpot"]),
                ActName = row["ActName"],
                SongTitle = row["SongTitle"],
                JuryPoints = int.TryParse(row["JuryPoints"], out int juryPoints) ? juryPoints : null,
                JuryRank = int.TryParse(row["JuryRank"], out int juryRank) ? juryRank : null,
                TelevotePoints = int.Parse(row["TelevotePoints"]),
                TelevoteRank = int.Parse(row["TelevoteRank"]),
                OverallPoints = int.Parse(row["OverallPoints"]),
                FinishingPosition = int.Parse(row["FinishingPosition"]),
                Competitors = int.Parse(row["Competitors"]),
            };
        }

        private sealed class ListingEqualityComparer : IEqualityComparer<CompetingCountryResultListing>
        {
            public bool Equals(CompetingCountryResultListing? x, CompetingCountryResultListing? y)
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

                return x.ContestYear == y.ContestYear
                    && x.ContestStage == y.ContestStage
                    && x.RunningOrderSpot == y.RunningOrderSpot
                    && x.ActName.Equals(y.ActName, StringComparison.Ordinal)
                    && x.SongTitle.Equals(y.SongTitle, StringComparison.Ordinal)
                    && x.JuryPoints == y.JuryPoints
                    && x.JuryRank == y.JuryRank
                    && x.TelevotePoints == y.TelevotePoints
                    && x.TelevoteRank == y.TelevoteRank
                    && x.OverallPoints == y.OverallPoints
                    && x.FinishingPosition == y.FinishingPosition
                    && x.Competitors == y.Competitors;
            }

            public int GetHashCode(CompetingCountryResultListing obj)
            {
                HashCode hashCode = new();
                hashCode.Add(obj.ContestYear);
                hashCode.Add((int)obj.ContestStage);
                hashCode.Add(obj.RunningOrderSpot);
                hashCode.Add(obj.ActName);
                hashCode.Add(obj.SongTitle);
                hashCode.Add(obj.JuryPoints);
                hashCode.Add(obj.JuryRank);
                hashCode.Add(obj.TelevotePoints);
                hashCode.Add(obj.TelevoteRank);
                hashCode.Add(obj.OverallPoints);
                hashCode.Add(obj.FinishingPosition);
                hashCode.Add(obj.Competitors);

                return hashCode.ToHashCode();
            }
        }
    }
}

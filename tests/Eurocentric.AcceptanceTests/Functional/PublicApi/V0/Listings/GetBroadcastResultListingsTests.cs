using Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils;
using Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils.Attributes;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V0.Dtos.Listings;
using Eurocentric.Apis.Public.V0.Enums;
using Eurocentric.Apis.Public.V0.Features.Listings;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V0.Listings;

[Category("public-api")]
public sealed class GetBroadcastResultListingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion0Point2AndUp]
    public async Task Should_retrieve_requested_broadcast_result_listings_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_get_the_broadcast_results_for(contestYear: 2022, contestStage: "SemiFinal2");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_response_metadata_should_match(contestYear: 2022, contestStage: "SemiFinal2");
        await euroFan.Then_the_response_listings_should_match(
            """
            | RunningOrderSpot | CountryCode | CountryName     | ActName           | SongTitle       | JuryPoints | TelevotePoints | OverallPoints | JuryRank | TelevoteRank | FinishingPosition |
            |------------------|-------------|-----------------|-------------------|-----------------|------------|----------------|---------------|----------|--------------|-------------------|
            | 17               | SE          | Sweden          | Cornelia Jakobs   | Hold Me Closer  | 222        | 174            | 396           | 1        | 1            | 1                 |
            | 8                | AU          | Australia       | Sheldon Riley     | Not The Same    | 169        | 74             | 243           | 2        | 8            | 2                 |
            | 3                | RS          | Serbia          | Konstrakta        | In Corpore Sano | 63         | 174            | 237           | 8        | 1            | 3                 |
            | 18               | CZ          | Czechia         | We Are Domi       | Lights Off      | 102        | 125            | 227           | 5        | 3            | 4                 |
            | 12               | EE          | Estonia         | Stefan            | Hope            | 113        | 96             | 209           | 3        | 7            | 5                 |
            | 14               | PL          | Poland          | Ochman            | River           | 84         | 114            | 198           | 7        | 4            | 6                 |
            | 1                | FI          | Finland         | The Rasmus        | Jezebel         | 63         | 99             | 162           | 8        | 6            | 7                 |
            | 16               | BE          | Belgium         | Jérémie Makiese   | Miss You        | 105        | 46             | 151           | 4        | 10           | 8                 |
            | 13               | RO          | Romania         | WRS               | Llámame         | 18         | 100            | 118           | 14       | 5            | 9                 |
            | 4                | AZ          | Azerbaijan      | Nadir Rustamli    | Fade To Black   | 96         | 0              | 96            | 6        | 18           | 10                |
            | 11               | MK          | North Macedonia | Andrea            | Circles         | 56         | 20             | 76            | 10       | 15           | 11                |
            | 9                | CY          | Cyprus          | Andromache        | Ela             | 9          | 54             | 63            | 18       | 9            | 12                |
            | 2                | IL          | Israel          | Michael Ben David | I.M             | 34         | 27             | 61            | 11       | 13           | 13                |
            | 7                | SM          | San Marino      | Achille Lauro     | Stripper        | 21         | 29             | 50            | 13       | 12           | 14                |
            | 10               | IE          | Ireland         | Brooke            | That's Rich     | 12         | 35             | 47            | 16       | 11           | 15                |
            | 6                | MT          | Malta           | Emma Muscat       | I Am What I Am  | 27         | 20             | 47            | 12       | 15           | 16                |
            | 15               | ME          | Montenegro      | Vladana           | Breathe         | 11         | 22             | 33            | 17       | 14           | 17                |
            | 5                | GE          | Georgia         | Circus Mircus     | Lock Me In      | 13         | 9              | 22            | 15       | 17           | 18                |
            """
        );
    }

    [Test]
    [ApiVersion0Point2AndUp]
    public async Task Should_retrieve_requested_broadcast_result_listings_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_get_the_broadcast_results_for(contestYear: 2023, contestStage: "SemiFinal1");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_response_metadata_should_match(contestYear: 2023, contestStage: "SemiFinal1");
        await euroFan.Then_the_response_listings_should_match(
            """
            | RunningOrderSpot | CountryCode | CountryName | ActName                   | SongTitle             | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition |
            |------------------|-------------|-------------|---------------------------|-----------------------|------------|----------|----------------|--------------|---------------|-------------------|
            | 15               | FI          | Finland     | Käärijä                   | Cha Cha Cha           |            |          | 177            | 1            | 177           | 1                 |
            | 11               | SE          | Sweden      | Loreen                    | Tattoo                |            |          | 135            | 2            | 135           | 2                 |
            | 9                | IL          | Israel      | Noa Kirel                 | Unicorn               |            |          | 127            | 3            | 127           | 3                 |
            | 13               | CZ          | Czechia     | Vesna                     | My Sister's Crown     |            |          | 110            | 4            | 110           | 4                 |
            | 10               | MD          | Moldova     | Pasha Parfeni             | Soarele şi Luna       |            |          | 109            | 5            | 109           | 5                 |
            | 1                | NO          | Norway      | Alessandra                | Queen of Kings        |            |          | 102            | 6            | 102           | 6                 |
            | 8                | CH          | Switzerland | Remo Forrer               | Watergun              |            |          | 97             | 7            | 97            | 7                 |
            | 7                | HR          | Croatia     | Let 3                     | Mama ŠČ!              |            |          | 76             | 8            | 76            | 8                 |
            | 5                | PT          | Portugal    | Mimicat                   | Ai Coração            |            |          | 74             | 9            | 74            | 9                 |
            | 3                | RS          | Serbia      | Luke Black                | Samo Mi Se Spava      |            |          | 37             | 10           | 37            | 10                |
            | 4                | LV          | Latvia      | Sudden Lights             | Aijā                  |            |          | 34             | 11           | 34            | 11                |
            | 6                | IE          | Ireland     | Wild Youth                | We Are One            |            |          | 10             | 12           | 10            | 12                |
            | 14               | NL          | Netherlands | Mia Nicolai & Dion Cooper | Burning Daylight      |            |          | 7              | 13           | 7             | 13                |
            | 12               | AZ          | Azerbaijan  | TuralTuranX               | Tell Me More          |            |          | 4              | 14           | 4             | 14                |
            | 2                | MT          | Malta       | The Busker                | Dance (Our Own Party) |            |          | 3              | 15           | 3             | 15                |
            """
        );
    }

    [Test]
    [ApiVersion0Point2AndUp]
    public async Task Should_retrieve_empty_listings_when_no_data_matches_query_params(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_get_the_broadcast_results_for(contestYear: 1666, contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_response_listings_should_be_an_empty_list();
        await euroFan.Then_the_response_metadata_should_match(contestYear: 1666, contestStage: "GrandFinal");
    }

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetBroadcastResultListingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_get_the_broadcast_results_for(string contestStage = "", int contestYear = 0)
        {
            Dictionary<string, object?> queryParameters = new()
            {
                { nameof(contestStage), contestStage },
                { nameof(contestYear), contestYear },
            };

            Request = Kernel.Requests.Listings.GetBroadcastResultListings(queryParameters);
        }

        public async Task Then_the_response_listings_should_match(string table)
        {
            BroadcastResultListing[] expectedListings = MarkdownParser.ParseTable(table, MapToListing).ToArray();

            await Assert.That(SuccessResponse?.Data?.Listings).IsNotNull().And.IsEquivalentTo(expectedListings);
        }

        public async Task Then_the_response_listings_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Listings).IsNotNull().And.IsEmpty();

        public async Task Then_the_response_metadata_should_match(string contestStage = "", int contestYear = 0)
        {
            ContestStage expectedContestStage = Enum.Parse<ContestStage>(contestStage);

            BroadcastResultMetadata metadata = await Assert.That(SuccessResponse?.Data?.Metadata).IsNotNull();

            await Assert.That(metadata.ContestYear).IsEqualTo(contestYear);
            await Assert.That(metadata.ContestStage).IsEqualTo(expectedContestStage);
        }

        private static BroadcastResultListing MapToListing(Dictionary<string, string> row)
        {
            return new BroadcastResultListing
            {
                RunningOrderSpot = int.Parse(row["RunningOrderSpot"]),
                CountryCode = row["CountryCode"],
                CountryName = row["CountryName"],
                ActName = row["ActName"],
                SongTitle = row["SongTitle"],
                JuryPoints = string.IsNullOrEmpty(row["JuryPoints"]) ? null : int.Parse(row["JuryPoints"]),
                TelevotePoints = int.Parse(row["TelevotePoints"]),
                OverallPoints = int.Parse(row["OverallPoints"]),
                JuryRank = string.IsNullOrEmpty(row["JuryRank"]) ? null : int.Parse(row["JuryRank"]),
                TelevoteRank = int.Parse(row["TelevoteRank"]),
                FinishingPosition = int.Parse(row["FinishingPosition"]),
            };
        }
    }
}

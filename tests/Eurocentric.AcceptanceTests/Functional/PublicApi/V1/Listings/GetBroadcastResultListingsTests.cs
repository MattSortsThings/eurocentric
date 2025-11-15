using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.Listings;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.Listings;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.Listings;

[Category("public-api")]
public sealed class GetBroadcastResultListingsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_1(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_broadcast_result_listings(contestYear: 2022, contestStage: "SemiFinal1");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(contestYear: 2022, contestStage: "SemiFinal1");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | RunningOrderSpot | CountryCode | CountryName | ActName                         | SongTitle               | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition |
            |------------------|-------------|-------------|---------------------------------|-------------------------|------------|----------|----------------|--------------|---------------|-------------------|
            | 1                | AL          | Albania     | Ronela Hajati                   | Sekret                  | 12         | 14       | 46             | 9            | 58            | 12                |
            | 2                | LV          | Latvia      | Citi Zēni                       | Eat Your Salad          | 39         | 11       | 16             | 15           | 55            | 14                |
            | 3                | LT          | Lithuania   | Monika Liu                      | Sentimentai             | 56         | 9        | 103            | 5            | 159           | 7                 |
            | 4                | CH          | Switzerland | Marius Bear                     | Boys Do Cry             | 107        | 5        | 11             | 16           | 118           | 9                 |
            | 5                | SI          | Slovenia    | LPS                             | Disko                   | 7          | 16       | 8              | 17           | 15            | 17                |
            | 6                | UA          | Ukraine     | Kalush Orchestra                | Stefania                | 135        | 3        | 202            | 1            | 337           | 1                 |
            | 7                | BG          | Bulgaria    | Intelligent Music Project       | Intention               | 11         | 15       | 18             | 14           | 29            | 16                |
            | 8                | NL          | Netherlands | S10                             | De Diepte               | 142        | 2        | 79             | 7            | 221           | 2                 |
            | 9                | MD          | Moldova     | Zdob şi Zdub & Advahov Brothers | Trenulețul              | 19         | 13       | 135            | 2            | 154           | 8                 |
            | 10               | PT          | Portugal    | MARO                            | Saudade, Saudade        | 121        | 4        | 87             | 6            | 208           | 4                 |
            | 11               | HR          | Croatia     | Mia Dimšić                      | Guilty Pleasure         | 42         | 10       | 33             | 12           | 75            | 11                |
            | 12               | DK          | Denmark     | REDDI                           | The Show                | 35         | 12       | 20             | 13           | 55            | 13                |
            | 13               | AT          | Austria     | LUM!X feat. Pia Maria           | Halo                    | 6          | 17       | 36             | 11           | 42            | 15                |
            | 14               | IS          | Iceland     | Systur                          | Með Hækkandi Sól       | 64         | 8        | 39             | 10           | 103           | 10                |
            | 15               | GR          | Greece      | Amanda Georgiadi Tenfjord       | Die Together            | 151        | 1        | 60             | 8            | 211           | 3                 |
            | 16               | NO          | Norway      | Subwoolfer                      | Give That Wolf A Banana | 73         | 7        | 104            | 4            | 177           | 6                 |
            | 17               | AM          | Armenia     | Rosa Linn                       | Snap                    | 82         | 6        | 105            | 3            | 187           | 5                 |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_2(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_broadcast_result_listings(contestYear: 2022, contestStage: "SemiFinal2");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(contestYear: 2022, contestStage: "SemiFinal2");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | RunningOrderSpot | CountryCode | CountryName     | ActName           | SongTitle       | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition |
            |------------------|-------------|-----------------|-------------------|-----------------|------------|----------|----------------|--------------|---------------|-------------------|
            | 1                | FI          | Finland         | The Rasmus        | Jezebel         | 63         | 8        | 99             | 6            | 162           | 7                 |
            | 2                | IL          | Israel          | Michael Ben David | I.M             | 34         | 11       | 27             | 13           | 61            | 13                |
            | 3                | RS          | Serbia          | Konstrakta        | In Corpore Sano | 63         | 8        | 174            | 1            | 237           | 3                 |
            | 4                | AZ          | Azerbaijan      | Nadir Rustamli    | Fade To Black   | 96         | 6        | 0              | 18           | 96            | 10                |
            | 5                | GE          | Georgia         | Circus Mircus     | Lock Me In      | 13         | 15       | 9              | 17           | 22            | 18                |
            | 6                | MT          | Malta           | Emma Muscat       | I Am What I Am  | 27         | 12       | 20             | 15           | 47            | 16                |
            | 7                | SM          | San Marino      | Achille Lauro     | Stripper        | 21         | 13       | 29             | 12           | 50            | 14                |
            | 8                | AU          | Australia       | Sheldon Riley     | Not The Same    | 169        | 2        | 74             | 8            | 243           | 2                 |
            | 9                | CY          | Cyprus          | Andromache        | Ela             | 9          | 18       | 54             | 9            | 63            | 12                |
            | 10               | IE          | Ireland         | Brooke            | That's Rich     | 12         | 16       | 35             | 11           | 47            | 15                |
            | 11               | MK          | North Macedonia | Andrea            | Circles         | 56         | 10       | 20             | 15           | 76            | 11                |
            | 12               | EE          | Estonia         | Stefan            | Hope            | 113        | 3        | 96             | 7            | 209           | 5                 |
            | 13               | RO          | Romania         | WRS               | Llámame         | 18         | 14       | 100            | 5            | 118           | 9                 |
            | 14               | PL          | Poland          | Ochman            | River           | 84         | 7        | 114            | 4            | 198           | 6                 |
            | 15               | ME          | Montenegro      | Vladana           | Breathe         | 11         | 17       | 22             | 14           | 33            | 17                |
            | 16               | BE          | Belgium         | Jérémie Makiese   | Miss You        | 105        | 4        | 46             | 10           | 151           | 8                 |
            | 17               | SE          | Sweden          | Cornelia Jakobs   | Hold Me Closer  | 222        | 1        | 174            | 1            | 396           | 1                 |
            | 18               | CZ          | Czechia         | We Are Domi       | Lights Off      | 102        | 5        | 125            | 3            | 227           | 4                 |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_3(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_broadcast_result_listings(contestYear: 2022, contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(contestYear: 2022, contestStage: "GrandFinal");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | RunningOrderSpot | CountryCode | CountryName    | ActName                         | SongTitle               | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition |
            |------------------|-------------|----------------|---------------------------------|-------------------------|------------|----------|----------------|--------------|---------------|-------------------|
            | 1                | CZ          | Czechia        | We Are Domi                     | Lights Off              | 33         | 19       | 5              | 21           | 38            | 22                |
            | 2                | RO          | Romania        | WRS                             | Llámame                 | 12         | 21       | 53             | 13           | 65            | 18                |
            | 3                | PT          | Portugal       | MARO                            | Saudade, Saudade        | 171        | 5        | 36             | 15           | 207           | 9                 |
            | 4                | FI          | Finland        | The Rasmus                      | Jezebel                 | 12         | 21       | 26             | 16           | 38            | 21                |
            | 5                | CH          | Switzerland    | Marius Bear                     | Boys Do Cry             | 78         | 12       | 0              | 25           | 78            | 17                |
            | 6                | FR          | France         | Alvan & Ahez                    | Fulenn                  | 9          | 24       | 8              | 19           | 17            | 24                |
            | 7                | NO          | Norway         | Subwoolfer                      | Give That Wolf A Banana | 36         | 17       | 146            | 7            | 182           | 10                |
            | 8                | AM          | Armenia        | Rosa Linn                       | Snap                    | 40         | 16       | 21             | 17           | 61            | 20                |
            | 9                | IT          | Italy          | Mahmood & BLANCO                | Brividi                 | 158        | 6        | 110            | 8            | 268           | 6                 |
            | 10               | ES          | Spain          | Chanel                          | SloMo                   | 231        | 3        | 228            | 3            | 459           | 3                 |
            | 11               | NL          | Netherlands    | S10                             | De Diepte               | 129        | 8        | 42             | 14           | 171           | 11                |
            | 12               | UA          | Ukraine        | Kalush Orchestra                | Stefania                | 192        | 4        | 439            | 1            | 631           | 1                 |
            | 13               | DE          | Germany        | Malik Harris                    | Rockstars               | 0          | 25       | 6              | 20           | 6             | 25                |
            | 14               | LT          | Lithuania      | Monika Liu                      | Sentimentai             | 35         | 18       | 93             | 11           | 128           | 14                |
            | 15               | AZ          | Azerbaijan     | Nadir Rustamli                  | Fade To Black           | 103        | 10       | 3              | 23           | 106           | 16                |
            | 16               | BE          | Belgium        | Jérémie Makiese                 | Miss You                | 59         | 13       | 5              | 21           | 64            | 19                |
            | 17               | GR          | Greece         | Amanda Georgiadi Tenfjord       | Die Together            | 158        | 6        | 57             | 12           | 215           | 8                 |
            | 18               | IS          | Iceland        | Systur                          | Með Hækkandi Sól       | 10         | 23       | 10             | 18           | 20            | 23                |
            | 19               | MD          | Moldova        | Zdob şi Zdub & Advahov Brothers | Trenulețul              | 14         | 20       | 239            | 2            | 253           | 7                 |
            | 20               | SE          | Sweden         | Cornelia Jakobs                 | Hold Me Closer          | 258        | 2        | 180            | 6            | 438           | 4                 |
            | 21               | AU          | Australia      | Sheldon Riley                   | Not The Same            | 123        | 9        | 2              | 24           | 125           | 15                |
            | 22               | GB          | United Kingdom | Sam Ryder                       | SPACE MAN               | 283        | 1        | 183            | 5            | 466           | 2                 |
            | 23               | PL          | Poland         | Ochman                          | River                   | 46         | 14       | 105            | 9            | 151           | 12                |
            | 24               | RS          | Serbia         | Konstrakta                      | In Corpore Sano         | 87         | 11       | 225            | 4            | 312           | 5                 |
            | 25               | EE          | Estonia        | Stefan                          | Hope                    | 43         | 15       | 98             | 10           | 141           | 13                |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_4(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_broadcast_result_listings(contestYear: 2023, contestStage: "SemiFinal1");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(contestYear: 2023, contestStage: "SemiFinal1");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | RunningOrderSpot | CountryCode | CountryName | ActName                   | SongTitle             | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition |
            |------------------|-------------|-------------|---------------------------|-----------------------|------------|----------|----------------|--------------|---------------|-------------------|
            | 1                | NO          | Norway      | Alessandra                | Queen of Kings        |            |          | 102            | 6            | 102           | 6                 |
            | 2                | MT          | Malta       | The Busker                | Dance (Our Own Party) |            |          | 3              | 15           | 3             | 15                |
            | 3                | RS          | Serbia      | Luke Black                | Samo Mi Se Spava      |            |          | 37             | 10           | 37            | 10                |
            | 4                | LV          | Latvia      | Sudden Lights             | Aijā                  |            |          | 34             | 11           | 34            | 11                |
            | 5                | PT          | Portugal    | Mimicat                   | Ai Coração            |            |          | 74             | 9            | 74            | 9                 |
            | 6                | IE          | Ireland     | Wild Youth                | We Are One            |            |          | 10             | 12           | 10            | 12                |
            | 7                | HR          | Croatia     | Let 3                     | Mama ŠČ!              |            |          | 76             | 8            | 76            | 8                 |
            | 8                | CH          | Switzerland | Remo Forrer               | Watergun              |            |          | 97             | 7            | 97            | 7                 |
            | 9                | IL          | Israel      | Noa Kirel                 | Unicorn               |            |          | 127            | 3            | 127           | 3                 |
            | 10               | MD          | Moldova     | Pasha Parfeni             | Soarele şi Luna       |            |          | 109            | 5            | 109           | 5                 |
            | 11               | SE          | Sweden      | Loreen                    | Tattoo                |            |          | 135            | 2            | 135           | 2                 |
            | 12               | AZ          | Azerbaijan  | TuralTuranX               | Tell Me More          |            |          | 4              | 14           | 4             | 14                |
            | 13               | CZ          | Czechia     | Vesna                     | My Sister's Crown     |            |          | 110            | 4            | 110           | 4                 |
            | 14               | NL          | Netherlands | Mia Nicolai & Dion Cooper | Burning Daylight      |            |          | 7              | 13           | 7             | 13                |
            | 15               | FI          | Finland     | Käärijä                   | Cha Cha Cha           |            |          | 177            | 1            | 177           | 1                 |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_5(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_broadcast_result_listings(contestYear: 2023, contestStage: "SemiFinal2");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(contestYear: 2023, contestStage: "SemiFinal2");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | RunningOrderSpot | CountryCode | CountryName | ActName                   | SongTitle              | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition |
            |------------------|-------------|-------------|---------------------------|------------------------|------------|----------|----------------|--------------|---------------|-------------------|
            | 1                | DK          | Denmark     | Reiley                    | Breaking My Heart      |            |          | 6              | 14           | 6             | 14                |
            | 2                | AM          | Armenia     | Brunette                  | Future Lover           |            |          | 99             | 6            | 99            | 6                 |
            | 3                | RO          | Romania     | Theodor Andrei            | D.G.T. (Off and On)    |            |          | 0              | 15           | 0             | 15                |
            | 4                | EE          | Estonia     | Alika                     | Bridges                |            |          | 74             | 10           | 74            | 10                |
            | 5                | BE          | Belgium     | Gustaph                   | Because Of You         |            |          | 90             | 8            | 90            | 8                 |
            | 6                | CY          | Cyprus      | Andrew Lambrou            | Break A Broken Heart   |            |          | 94             | 7            | 94            | 7                 |
            | 7                | IS          | Iceland     | Diljá                     | Power                  |            |          | 44             | 11           | 44            | 11                |
            | 8                | GR          | Greece      | Victor Vernicos           | What They Say          |            |          | 14             | 13           | 14            | 13                |
            | 9                | PL          | Poland      | Blanka                    | Solo                   |            |          | 124            | 3            | 124           | 3                 |
            | 10               | SI          | Slovenia    | Joker Out                 | Carpe Diem             |            |          | 103            | 5            | 103           | 5                 |
            | 11               | GE          | Georgia     | Iru                       | Echo                   |            |          | 33             | 12           | 33            | 12                |
            | 12               | SM          | San Marino  | Piqued Jacks              | Like An Animal         |            |          | 0              | 15           | 0             | 16                |
            | 13               | AT          | Austria     | Teya & Salena             | Who The Hell Is Edgar? |            |          | 137            | 2            | 137           | 2                 |
            | 14               | AL          | Albania     | Albina & Familja Kelmendi | Duje                   |            |          | 83             | 9            | 83            | 9                 |
            | 15               | LT          | Lithuania   | Monika Linkytė            | Stay                   |            |          | 110            | 4            | 110           | 4                 |
            | 16               | AU          | Australia   | Voyager                   | Promise                |            |          | 149            | 1            | 149           | 1                 |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_full_listings_and_metadata_scenario_6(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_broadcast_result_listings(contestYear: 2023, contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(contestYear: 2023, contestStage: "GrandFinal");
        await euroFan.Then_the_retrieved_listings_in_order_should_match(
            """
            | RunningOrderSpot | CountryCode | CountryName    | ActName                   | SongTitle              | JuryPoints | JuryRank | TelevotePoints | TelevoteRank | OverallPoints | FinishingPosition |
            |------------------|-------------|----------------|---------------------------|------------------------|------------|----------|----------------|--------------|---------------|-------------------|
            | 1                | AT          | Austria        | Teya & Salena             | Who The Hell Is Edgar? | 104        | 8        | 16             | 21           | 120           | 15                |
            | 2                | PT          | Portugal       | Mimicat                   | Ai Coração             | 43         | 18       | 16             | 21           | 59            | 23                |
            | 3                | CH          | Switzerland    | Remo Forrer               | Watergun               | 61         | 14       | 31             | 18           | 92            | 20                |
            | 4                | PL          | Poland         | Blanka                    | Solo                   | 12         | 24       | 81             | 8            | 93            | 19                |
            | 5                | RS          | Serbia         | Luke Black                | Samo Mi Se Spava       | 14         | 23       | 16             | 21           | 30            | 24                |
            | 6                | FR          | France         | La Zarra                  | Évidemment             | 54         | 15       | 50             | 14           | 104           | 16                |
            | 7                | CY          | Cyprus         | Andrew Lambrou            | Break A Broken Heart   | 68         | 13       | 58             | 11           | 126           | 12                |
            | 8                | ES          | Spain          | Blanca Paloma             | Eaea                   | 95         | 9        | 5              | 26           | 100           | 17                |
            | 9                | SE          | Sweden         | Loreen                    | Tattoo                 | 340        | 1        | 243            | 2            | 583           | 1                 |
            | 10               | AL          | Albania        | Albina & Familja Kelmendi | Duje                   | 17         | 21       | 59             | 10           | 76            | 22                |
            | 11               | IT          | Italy          | Marco Mengoni             | Due Vite               | 176        | 3        | 174            | 6            | 350           | 4                 |
            | 12               | EE          | Estonia        | Alika                     | Bridges                | 146        | 5        | 22             | 19           | 168           | 8                 |
            | 13               | FI          | Finland        | Käärijä                   | Cha Cha Cha            | 150        | 4        | 376            | 1            | 526           | 2                 |
            | 14               | CZ          | Czechia        | Vesna                     | My Sister's Crown      | 94         | 10       | 35             | 17           | 129           | 10                |
            | 15               | AU          | Australia      | Voyager                   | Promise                | 130        | 6        | 21             | 20           | 151           | 9                 |
            | 16               | BE          | Belgium        | Gustaph                   | Because Of You         | 127        | 7        | 55             | 12           | 182           | 7                 |
            | 17               | AM          | Armenia        | Brunette                  | Future Lover           | 69         | 12       | 53             | 13           | 122           | 14                |
            | 18               | MD          | Moldova        | Pasha Parfeni             | Soarele şi Luna        | 20         | 20       | 76             | 9            | 96            | 18                |
            | 19               | UA          | Ukraine        | TVORCHI                   | Heart Of Steel         | 54         | 15       | 189            | 4            | 243           | 6                 |
            | 20               | NO          | Norway         | Alessandra                | Queen of Kings         | 52         | 17       | 216            | 3            | 268           | 5                 |
            | 21               | DE          | Germany        | Lord of the Lost          | Blood & Glitter        | 3          | 26       | 15             | 24           | 18            | 26                |
            | 22               | LT          | Lithuania      | Monika Linkytė            | Stay                   | 81         | 11       | 46             | 15           | 127           | 11                |
            | 23               | IL          | Israel         | Noa Kirel                 | Unicorn                | 177        | 2        | 185            | 5            | 362           | 3                 |
            | 24               | SI          | Slovenia       | Joker Out                 | Carpe Diem             | 33         | 19       | 45             | 16           | 78            | 21                |
            | 25               | HR          | Croatia        | Let 3                     | Mama ŠČ!               | 11         | 25       | 112            | 7            | 123           | 13                |
            | 26               | GB          | United Kingdom | Mae Muller                | I Wrote A Song         | 15         | 22       | 9              | 25           | 24            | 25                |
            """
        );
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_listings_when_no_data_matches_query(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_broadcast_result_listings(contestYear: 1066, contestStage: "GrandFinal");

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_metadata_should_match(contestYear: 1066, contestStage: "GrandFinal");
        await euroFan.Then_the_retrieved_listings_in_order_should_be_an_empty_list();
    }

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetBroadcastResultListingsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_broadcast_result_listings(string contestStage = "", int contestYear = 0)
        {
            Dictionary<string, object?> queryParams = new()
            {
                { nameof(contestStage), contestStage },
                { nameof(contestYear), contestYear },
            };

            Request = Kernel.Requests.Listings.GetBroadcastResultListings(queryParams);
        }

        public async Task Then_the_retrieved_metadata_should_match(string contestStage = "", int contestYear = 0)
        {
            ContestStage expectedContestStage = Enum.Parse<ContestStage>(contestStage);

            await Assert
                .That(SuccessResponse?.Data?.Metadata)
                .HasProperty(br => br.ContestYear, contestYear)
                .And.HasProperty(br => br.ContestStage, expectedContestStage);
        }

        public async Task Then_the_retrieved_listings_in_order_should_match(string table)
        {
            BroadcastResultListing[] expectedListings = MarkdownParser.ParseTable(table, MapToListing);

            await Assert
                .That(SuccessResponse?.Data?.Listings)
                .IsEquivalentTo(expectedListings, new ListingEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_listings_in_order_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Listings).IsEmpty();

        private static BroadcastResultListing MapToListing(Dictionary<string, string> row)
        {
            return new BroadcastResultListing
            {
                RunningOrderSpot = int.Parse(row["RunningOrderSpot"]),
                CountryCode = row["CountryCode"],
                CountryName = row["CountryName"],
                ActName = row["ActName"],
                SongTitle = row["SongTitle"],
                JuryPoints = int.TryParse(row["JuryPoints"], out int juryPoints) ? juryPoints : null,
                JuryRank = int.TryParse(row["JuryRank"], out int rank) ? rank : null,
                TelevotePoints = int.Parse(row["TelevotePoints"]),
                TelevoteRank = int.Parse(row["TelevoteRank"]),
                OverallPoints = int.Parse(row["OverallPoints"]),
                FinishingPosition = int.Parse(row["FinishingPosition"]),
            };
        }

        private sealed class ListingEqualityComparer : IEqualityComparer<BroadcastResultListing>
        {
            public bool Equals(BroadcastResultListing? x, BroadcastResultListing? y)
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

                return x.RunningOrderSpot == y.RunningOrderSpot
                    && x.CountryCode.Equals(y.CountryCode, StringComparison.Ordinal)
                    && x.CountryName.Equals(y.CountryName, StringComparison.Ordinal)
                    && x.ActName.Equals(y.ActName, StringComparison.Ordinal)
                    && x.SongTitle.Equals(y.SongTitle, StringComparison.Ordinal)
                    && x.JuryPoints == y.JuryPoints
                    && x.JuryRank == y.JuryRank
                    && x.TelevotePoints == y.TelevotePoints
                    && x.TelevoteRank == y.TelevoteRank
                    && x.OverallPoints == y.OverallPoints
                    && x.FinishingPosition == y.FinishingPosition;
            }

            public int GetHashCode(BroadcastResultListing obj)
            {
                HashCode hashCode = new();
                hashCode.Add(obj.RunningOrderSpot);
                hashCode.Add(obj.CountryCode);
                hashCode.Add(obj.CountryName);
                hashCode.Add(obj.ActName);
                hashCode.Add(obj.SongTitle);
                hashCode.Add(obj.JuryPoints);
                hashCode.Add(obj.JuryRank);
                hashCode.Add(obj.TelevotePoints);
                hashCode.Add(obj.TelevoteRank);
                hashCode.Add(obj.OverallPoints);
                hashCode.Add(obj.FinishingPosition);

                return hashCode.ToHashCode();
            }
        }
    }
}

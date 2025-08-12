using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableBroadcasts;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Queryables;

public sealed class GetQueryableBroadcastsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_all_queryable_broadcasts_in_broadcast_date_order(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_queryable_broadcasts();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_retrieved_queryable_broadcasts_should_be(
            """
            | ContestYear | ContestStage | BroadcastDate | Competitors | Juries | Televotes |
            |-------------|--------------|---------------|-------------|--------|-----------|
            | 2022        | SemiFinal1   | 2022-05-10    | 17          | 19     | 19        |
            | 2022        | SemiFinal2   | 2022-05-12    | 18          | 21     | 21        |
            | 2022        | GrandFinal   | 2022-05-14    | 25          | 40     | 40        |
            | 2023        | SemiFinal1   | 2023-05-09    | 15          | 0      | 19        |
            | 2023        | SemiFinal2   | 2023-05-11    | 16          | 0      | 20        |
            | 2023        | GrandFinal   | 2023-05-13    | 26          | 37     | 38        |
            """);
    }

    private sealed class EuroFanActor(IApiDriver apiDriver) : EuroFanActorWithResponse<GetQueryableBroadcastsResponse>(apiDriver)
    {
        public void Given_I_want_to_retrieve_all_queryable_broadcasts() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableBroadcasts();

        public async Task Then_the_retrieved_queryable_broadcasts_should_be(string broadcasts)
        {
            GetQueryableBroadcastsResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            QueryableBroadcast[] expectedQueryableBroadcasts = MarkdownParser.ParseTable(broadcasts, MapRowToQueryableBroadcast)
                .ToArray();

            await Assert.That(responseBody.QueryableBroadcasts)
                .IsEquivalentTo(expectedQueryableBroadcasts, CollectionOrdering.Matching);
        }

        private static QueryableBroadcast MapRowToQueryableBroadcast(Dictionary<string, string> row) => new()
        {
            BroadcastDate = DateOnly.ParseExact(row["BroadcastDate"], "yyyy-MM-dd"),
            ContestYear = int.Parse(row["ContestYear"]),
            ContestStage = Enum.Parse<ContestStage>(row["ContestStage"]),
            Competitors = int.Parse(row["Competitors"]),
            Juries = int.Parse(row["Juries"]),
            Televotes = int.Parse(row["Televotes"])
        };
    }
}

using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.Queryables;
using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Apis.Public.V1.Features.Queryables;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.Queryables;

[Category("public-api")]
public sealed class GetQueryableBroadcastsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [TestUtils.ApiVersion1Point0AndUp]
    public async Task Should_retrieve_all_queryable_broadcasts_in_broadcast_date_order(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_queryable_broadcasts();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_queryable_broadcasts_in_order_should_be(
            """
            | BroadcastDate | ContestYear | CityName  | ContestStage | Competitors | Juries | Televotes |
            |---------------|-------------|-----------|--------------|-------------|--------|-----------|
            | 2022-05-10    | 2022        | Turin     | SemiFinal1   | 17          | 19     | 19        |
            | 2022-05-12    | 2022        | Turin     | SemiFinal2   | 18          | 21     | 21        |
            | 2022-05-14    | 2022        | Turin     | GrandFinal   | 25          | 40     | 40        |
            | 2023-05-09    | 2023        | Liverpool | SemiFinal1   | 15          | 0      | 19        |
            | 2023-05-11    | 2023        | Liverpool | SemiFinal2   | 16          | 0      | 20        |
            | 2023-05-13    | 2023        | Liverpool | GrandFinal   | 26          | 37     | 38        |
            """
        );
    }

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetQueryableBroadcastsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_all_queryable_broadcasts() =>
            Request = Kernel.Requests.Queryables.GetQueryableBroadcasts();

        public async Task Then_the_retrieved_queryable_broadcasts_in_order_should_be(string table)
        {
            QueryableBroadcast[] expected = MarkdownParser.ParseTable(table, MapToQueryableBroadcast);

            await Assert
                .That(SuccessResponse?.Data?.QueryableBroadcasts)
                .IsEquivalentTo(expected, new EqualityComparer(), CollectionOrdering.Matching);
        }

        private static QueryableBroadcast MapToQueryableBroadcast(Dictionary<string, string> row)
        {
            return new QueryableBroadcast
            {
                BroadcastDate = DateOnly.ParseExact(row["BroadcastDate"], TestDefaults.DateFormat),
                ContestYear = int.Parse(row["ContestYear"]),
                CityName = row["CityName"],
                ContestStage = Enum.Parse<ContestStage>(row["ContestStage"]),
                Competitors = int.Parse(row["Competitors"]),
                Juries = int.Parse(row["Juries"]),
                Televotes = int.Parse(row["Televotes"]),
            };
        }

        private sealed class EqualityComparer : IEqualityComparer<QueryableBroadcast>
        {
            public bool Equals(QueryableBroadcast? x, QueryableBroadcast? y)
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

                return x.BroadcastDate.Equals(y.BroadcastDate)
                    && x.ContestYear == y.ContestYear
                    && x.CityName.Equals(y.CityName, StringComparison.Ordinal)
                    && x.ContestStage == y.ContestStage
                    && x.Competitors == y.Competitors
                    && x.Juries == y.Juries
                    && x.Televotes == y.Televotes;
            }

            public int GetHashCode(QueryableBroadcast obj) =>
                HashCode.Combine(
                    obj.BroadcastDate,
                    obj.ContestYear,
                    obj.CityName,
                    (int)obj.ContestStage,
                    obj.Competitors,
                    obj.Juries,
                    obj.Televotes
                );
        }
    }
}

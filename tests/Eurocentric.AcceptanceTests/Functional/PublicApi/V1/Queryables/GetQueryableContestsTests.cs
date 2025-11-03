using Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Public.V1.Dtos.Queryables;
using Eurocentric.Apis.Public.V1.Features.Queryables;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.Queryables;

[Category("public-api")]
public sealed class GetQueryableContestsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_all_queryable_contests_in_contest_year_order(string apiVersion)
    {
        EuroFan euroFan = new(EuroFanKernel.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_queryable_contests();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code(200);
        await euroFan.Then_the_retrieved_queryable_contests_in_order_should_be(
            """
            | ContestYear | CityName  | Participants | UsesRestOfWorldTelevote |
            |-------------|-----------|--------------|-------------------------|
            | 2022        | Turin     | 40           | false                   |
            | 2023        | Liverpool | 37           | true                    |
            """
        );
    }

    private sealed class EuroFan(EuroFanKernel kernel) : EuroFanActor<GetQueryableContestsResponse>
    {
        private protected override EuroFanKernel Kernel { get; } = kernel;

        public void Given_I_want_to_retrieve_all_queryable_contests() =>
            Request = Kernel.Requests.Queryables.GetQueryableContests();

        public async Task Then_the_retrieved_queryable_contests_in_order_should_be(string table)
        {
            QueryableContest[] expected = MarkdownParser.ParseTable(table, MapToQueryableContest);

            await Assert
                .That(SuccessResponse?.Data?.QueryableContests)
                .IsEquivalentTo(expected, new EqualityComparer(), CollectionOrdering.Matching);
        }

        private static QueryableContest MapToQueryableContest(Dictionary<string, string> row)
        {
            return new QueryableContest
            {
                ContestYear = int.Parse(row["ContestYear"]),
                CityName = row["CityName"],
                Participants = int.Parse(row["Participants"]),
                UsesRestOfWorldTelevote = bool.Parse(row["UsesRestOfWorldTelevote"]),
            };
        }

        private sealed class EqualityComparer : IEqualityComparer<QueryableContest>
        {
            public bool Equals(QueryableContest? x, QueryableContest? y)
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
                    && x.CityName.Equals(y.CityName, StringComparison.Ordinal)
                    && x.Participants == y.Participants
                    && x.UsesRestOfWorldTelevote == y.UsesRestOfWorldTelevote;
            }

            public int GetHashCode(QueryableContest obj) =>
                HashCode.Combine(obj.ContestYear, obj.CityName, obj.Participants, obj.UsesRestOfWorldTelevote);
        }
    }
}

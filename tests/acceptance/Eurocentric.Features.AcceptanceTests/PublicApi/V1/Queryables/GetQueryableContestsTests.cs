using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableContests;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Queryables;

public sealed class GetQueryableContestsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_all_queryable_contests_in_contest_year_order(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_queryable_contests();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_retrieved_queryable_contests_should_be(
            """
            | ContestYear | CityName  | Competitors | HasRestOfWorldTelevotes |
            |-------------|-----------|-------------|-------------------------|
            | 2022        | Turin     | 40          | false                   |
            | 2023        | Liverpool | 37          | true                    |
            """);
    }

    private sealed class EuroFanActor(IApiDriver apiDriver) : EuroFanActorWithResponse<GetQueryableContestsResponse>(apiDriver)
    {
        public void Given_I_want_to_retrieve_all_queryable_contests() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableContests();

        public async Task Then_the_retrieved_queryable_contests_should_be(string contests)
        {
            GetQueryableContestsResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            QueryableContest[] expectedQueryableContests =
                MarkdownParser.ParseTable(contests, MapRowToQueryableContest).ToArray();

            await Assert.That(responseBody.QueryableContests)
                .IsEquivalentTo(expectedQueryableContests, CollectionOrdering.Matching);
        }

        private static QueryableContest MapRowToQueryableContest(Dictionary<string, string> row) => new()
        {
            ContestYear = int.Parse(row["ContestYear"]),
            CityName = row["CityName"],
            Competitors = int.Parse(row["Competitors"]),
            HasRestOfWorldTelevotes = bool.Parse(row["HasRestOfWorldTelevotes"])
        };
    }
}

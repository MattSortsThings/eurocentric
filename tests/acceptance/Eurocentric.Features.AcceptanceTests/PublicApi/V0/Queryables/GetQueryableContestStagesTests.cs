using Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils.DataSourceGenerators;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableContestStages;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Queryables;

public sealed class GetQueryableContestStagesTests : SeededParallelAcceptanceTest
{
    [Test]
    [PublicApiV0Point1AndUp]
    public async Task Endpoint_should_retrieve_all_QueryableContestStage_enum_values_in_order(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_QueryableContestStage_enum_values();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_retrieved_QueryableContestStage_enum_values_should_be(
            "Any",
            "SemiFinal1",
            "SemiFinal2",
            "SemiFinals",
            "GrandFinal");
    }

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetQueryableContestStagesResponse>(apiDriver)
    {
        public void Given_I_want_to_retrieve_all_QueryableContestStage_enum_values() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableContestStages();

        public async Task Then_the_retrieved_QueryableContestStage_enum_values_should_be(params string[] values)
        {
            GetQueryableContestStagesResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            IEnumerable<QueryableContestStage> expectedValues = values.Select(Enum.Parse<QueryableContestStage>);

            QueryableContestStage[] actualValues = responseBody.QueryableContestStages;

            await Assert.That(actualValues).IsEquivalentTo(expectedValues, CollectionOrdering.Matching);
        }
    }
}

using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Attributes;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableContestStages;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Queryables;

public sealed class GetQueryableContestStagesTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_all_QueryableContestStage_enum_values(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_QueryableContestStage_enum_values();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_retrieved_QueryableVotingMethod_enum_values_should_be(
            "Any", "SemiFinal1", "SemiFinal2", "SemiFinals", "GrandFinal");
    }

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetQueryableContestStagesResponse>(apiDriver)
    {
        public void Given_I_want_to_retrieve_all_QueryableContestStage_enum_values() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableContestStages();

        public async Task Then_the_retrieved_QueryableVotingMethod_enum_values_should_be(
            params IEnumerable<string> orderedValues)
        {
            GetQueryableContestStagesResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            QueryableContestStage[] expectedValues = orderedValues.Select(Enum.Parse<QueryableContestStage>).ToArray();

            await Assert.That(responseBody.QueryableContestStages).IsEquivalentTo(expectedValues);
        }
    }
}

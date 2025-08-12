using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Attributes;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableVotingMethods;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Queryables;

public sealed class GetQueryableVotingMethodsTests : ParallelSeededAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_all_QueryableVotingMethod_enum_values_in_order(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_QueryableVotingMethod_enum_values();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_retrieved_QueryableVotingMethod_enum_values_should_be("Any", "Jury", "Televote");
    }

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetQueryableVotingMethodsResponse>(apiDriver)
    {
        public void Given_I_want_to_retrieve_all_QueryableVotingMethod_enum_values() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableVotingMethods();

        public async Task Then_the_retrieved_QueryableVotingMethod_enum_values_should_be(
            params IEnumerable<string> orderedValues)
        {
            GetQueryableVotingMethodsResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            QueryableVotingMethod[] expectedValues = orderedValues.Select(Enum.Parse<QueryableVotingMethod>).ToArray();

            await Assert.That(responseBody.QueryableVotingMethods).IsEquivalentTo(expectedValues);
        }
    }
}

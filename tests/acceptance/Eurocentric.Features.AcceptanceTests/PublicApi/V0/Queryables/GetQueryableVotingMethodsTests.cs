using Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;
using Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils.DataSourceGenerators;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableVotingMethods;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Queryables;

public sealed class GetQueryableVotingMethodsTests : SeededParallelAcceptanceTest
{
    [Test]
    [PublicApiV0Point1AndUp]
    public async Task Endpoint_should_retrieve_all_QueryableVotingMethod_enum_values_in_order(string apiVersion)
    {
        EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_QueryableVotingMethod_enum_values();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await euroFan.Then_the_retrieved_QueryableVotingMethod_enum_values_should_be(
            "Any",
            "Jury",
            "Televote");
    }

    private sealed class EuroFanActor(IApiDriver apiDriver)
        : EuroFanActorWithResponse<GetQueryableVotingMethodsResponse>(apiDriver)
    {
        public void Given_I_want_to_retrieve_all_QueryableVotingMethod_enum_values() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableVotingMethods();

        public async Task Then_the_retrieved_QueryableVotingMethod_enum_values_should_be(params string[] values)
        {
            GetQueryableVotingMethodsResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            IEnumerable<QueryableVotingMethod> expectedValues = values.Select(Enum.Parse<QueryableVotingMethod>);

            QueryableVotingMethod[] actualValues = responseBody.QueryableVotingMethods;

            await Assert.That(actualValues).IsEquivalentTo(expectedValues, CollectionOrdering.Matching);
        }
    }
}

using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.PublicApi.V0.Filters;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Filters;

public sealed class GetAvailableVotingMethodsTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v0.1")]
    [InlineData("v0.2")]
    public async Task Should_be_able_to_retrieve_all_available_voting_method_values(string apiVersion)
    {
        EuroFanActor euroFan = new(PublicApiV0Driver.Create(SutRestClient, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_voting_method_values();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        euroFan.Then_my_request_should_succeed_with_status_code_200_OK();
        euroFan.Then_the_retrieved_voting_method_values_should_be("Any", "Jury", "Televote");
    }

    private sealed class EuroFanActor : ActorWithResponse<GetAvailableVotingMethodsResponse>
    {
        public EuroFanActor(IPublicApiV0Driver apiDriver) : base(apiDriver)
        {
        }

        public void Given_I_want_to_retrieve_all_voting_method_values() =>
            SendMyRequest = apiDriver => apiDriver.Filters.GetAvailableVotingMethods(TestContext.Current.CancellationToken);

        public void Then_the_retrieved_voting_method_values_should_be(params string[] expectedValues)
        {
            Assert.NotNull(ResponseObject);

            Assert.Equal(expectedValues.Select(Enum.Parse<VotingMethod>), ResponseObject.VotingMethods);
        }
    }
}

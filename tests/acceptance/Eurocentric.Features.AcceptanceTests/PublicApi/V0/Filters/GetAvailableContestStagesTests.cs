using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.PublicApi.V0.Filters;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Filters;

public sealed class GetAvailableContestStagesTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v0.1")]
    [InlineData("v0.2")]
    public async Task Should_be_able_to_retrieve_all_available_contest_stages_values(string apiVersion)
    {
        EuroFanActor euroFan = new(PublicApiV0Driver.Create(SutRestClient, apiVersion));

        // Given
        euroFan.Given_I_want_to_retrieve_all_available_contest_stages_values();

        // When
        await euroFan.When_I_send_my_request();

        // Then
        euroFan.Then_my_request_should_succeed_with_status_code_200_OK();
        euroFan.Then_the_retrieved_contest_stages_values_should_be("All",
            "SemiFinal1",
            "SemiFinal2",
            "SemiFinals",
            "GrandFinal");
    }

    private sealed class EuroFanActor : ActorWithResponse<GetAvailableContestStagesResponse>
    {
        public EuroFanActor(IPublicApiV0Driver apiDriver) : base(apiDriver)
        {
        }

        public void Given_I_want_to_retrieve_all_available_contest_stages_values() =>
            SendMyRequest = apiDriver => apiDriver.Filters.GetAvailableContestStages(TestContext.Current.CancellationToken);

        public void Then_the_retrieved_contest_stages_values_should_be(params string[] expectedValues)
        {
            Assert.NotNull(ResponseObject);

            Assert.Equal(expectedValues.Select(Enum.Parse<ContestStages>), ResponseObject.ContestStages);
        }
    }
}

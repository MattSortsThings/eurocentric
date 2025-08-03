using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableContestStages;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Queryables;

public static class GetQueryableContestStagesTests
{
    public sealed class GetQueryableContestStages : ParallelSeededAcceptanceTest
    {
        [Test]
        [Arguments("v0.1")]
        [Arguments("v0.2")]
        public async Task Should_retrieve_all_queryable_contest_stage_enum_values(string apiVersion)
        {
            EuroFanActor euroFan = new(ApiDriver.Create(SystemUnderTest, apiVersion));

            // Given
            euroFan.Given_I_want_to_retrieve_all_queryable_contest_stage_enum_values();

            // When
            await euroFan.When_I_send_my_request();

            // Then
            await euroFan.Then_my_request_should_SUCCEED_with_status_code_200_OK();
            await euroFan.Then_the_response_body_should_contain_all_queryable_contest_stage_enum_values_in_order();
        }
    }

    private sealed class EuroFanActor : EuroFanActorWithResponse<GetQueryableContestStagesResponse>
    {
        public EuroFanActor(IApiDriver apiDriver) : base(apiDriver)
        {
        }

        public void Given_I_want_to_retrieve_all_queryable_contest_stage_enum_values() =>
            Request = ApiDriver.RequestFactory.Queryables.GetQueryableContestStages();

        public async Task Then_the_response_body_should_contain_all_queryable_contest_stage_enum_values_in_order()
        {
            GetQueryableContestStagesResponse responseBody = await Assert.That(ResponseBody).IsNotNull();

            await Assert.That(responseBody.QueryableContestStages)
                .IsEquivalentTo(Enum.GetValues<QueryableContestStage>(), CollectionOrdering.Matching);
        }
    }
}

using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.PublicApi.V0.Filters;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Filters;

public static class GetVotingMethodsTests
{
    public sealed class Feature(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v0.1")]
        [InlineData("v0.2")]
        public async Task Should_retrieve_all_contest_stage_filter_enum_values(string apiVersion)
        {
            EuroFan euroFan = new(BackDoor, RestClient, apiVersion);

            // Given
            euroFan.Given_I_want_to_retrieve_all_voting_method_filter_enum_values();

            // When
            await euroFan.When_I_send_my_request();

            // Then
            euroFan.Then_my_request_should_succeed_with_status_code(200);
            euroFan.Then_the_retrieved_voting_method_filter_enum_values_should_be("Any", "Jury", "Televote");
        }
    }

    private sealed class EuroFan : ActorWithResponse<GetVotingMethodsResponse>
    {
        public EuroFan(IWebAppFixtureBackDoor backDoor, IWebAppFixtureRestClient restClient, string apiVersion = "v1.0") :
            base(backDoor, restClient, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_all_voting_method_filter_enum_values()
        {
            Request = new RestRequest("/public/api/{apiVersion}/filters/voting-methods");
            Request.AddUrlSegment("apiVersion", ApiVersion);
        }

        public void Then_the_retrieved_voting_method_filter_enum_values_should_be(params string[] values)
        {
            Assert.NotNull(ResponseObject);

            Assert.Equal(values.Select(Enum.Parse<VotingMethodFilter>), ResponseObject.VotingMethods);
        }
    }
}

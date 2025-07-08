using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.PublicApi.V0.Filters;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Filters;

public static class GetVotingMethodTests
{
    public sealed class Endpoint(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Theory]
        [InlineData("v0.1")]
        [InlineData("v0.2")]
        public async Task Should_retrieve_all_VotingMethodFilter_enum_values(string apiVersion)
        {
            EuroFan euroFan = new(RestClient, BackDoor, apiVersion);

            // Given
            euroFan.Given_I_want_to_retrieve_all_voting_method_filter_values();

            // When
            await euroFan.When_I_send_my_request();

            // Then
            euroFan.Then_my_request_should_succeed_with_status_code(200);
            euroFan.Then_the_retrieved_voting_method_filter_values_should_be(
                "Any",
                "Jury",
                "Televote");
        }
    }

    private sealed class EuroFan : EuroFanActor<GetVotingMethodsResponse>
    {
        public EuroFan(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
            base(restClient, backDoor, apiVersion)
        {
        }

        public void Given_I_want_to_retrieve_all_voting_method_filter_values() =>
            Request = RequestFactory.Filters.GetVotingMethods();

        public void Then_the_retrieved_voting_method_filter_values_should_be(params string[] values)
        {
            Assert.NotNull(ResponseObject);

            IEnumerable<VotingMethodFilter> expectedValues = values.Select(Enum.Parse<VotingMethodFilter>);
            VotingMethodFilter[] actualValues = ResponseObject.VotingMethods;

            Assert.Equal(expectedValues, actualValues);
        }
    }
}

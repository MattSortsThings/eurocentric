using System.Net;
using Eurocentric.PublicApi.Tests.Acceptance.Utils;
using Eurocentric.PublicApi.V0.VotingCountryRankings.GetVotingCountryPointsShareRankings;
using Eurocentric.Tests.Utils.Fixtures;
using RestSharp;

namespace Eurocentric.PublicApi.Tests.Acceptance.V0.VotingCountryRankings;

public static class GetVotingCountryPointsShareRankingsTests
{
    public sealed class PublicApi : SeededWebAppTest
    {
        public PublicApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_return_200_with_fixed_data_given_valid_request()
        {
            // Arrange
            RestRequest restRequest = Requests.Get.To(UriSegments.PublicApi.V0Latest + "voting-country-rankings/points-share")
                .AddQueryParameter("targetCountryCode", "GB")
                .UsePublicApiKey();

            // Act
            RestResponse<GetVotingCountryPointsShareRankingsResponse> result =
                await Sut.ExecuteAsync<GetVotingCountryPointsShareRankingsResponse>(restRequest,
                    TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => result.ShouldHaveStatusCode(HttpStatusCode.OK),
                () => Assert.NotEmpty(result.Data!.Items)
            );
        }
    }
}

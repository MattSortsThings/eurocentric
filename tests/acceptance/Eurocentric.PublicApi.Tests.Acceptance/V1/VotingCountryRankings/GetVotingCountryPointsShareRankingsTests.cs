using System.Net;
using Eurocentric.PublicApi.Tests.Acceptance.Utils;
using Eurocentric.PublicApi.V1.VotingCountryRankings.GetVotingCountryPointsShareRankings;
using Eurocentric.Tests.Utils.Fixtures;
using RestSharp;

namespace Eurocentric.PublicApi.Tests.Acceptance.V1.VotingCountryRankings;

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
            RestRequest restRequest = Requests.Get.To("public/api/v1.0/voting-country-rankings/points-share")
                .AddQueryParameter("targetCountryCode", "GB");

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

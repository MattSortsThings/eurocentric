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
        private const string ResourceUri = Apis.Public.V0.Latest.Uri + "voting-country-rankings/points-share";

        public PublicApi(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_return_200_with_fixed_data_given_valid_request()
        {
            // Arrange
            const string route = Apis.Public.V0.Latest.Uri + "voting-country-rankings/points-share";

            RestRequest request = RestRequestFactory.Get(ResourceUri)
                .AddQueryParameter("targetCountryCode", "GB")
                .UsePublicApiKey();

            // Act
            (HttpStatusCode statusCode, GetVotingCountryPointsShareRankingsResponse response) =
                await Sut.ExecuteAsync<GetVotingCountryPointsShareRankingsResponse>(request,
                    TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.OK),
                () => Assert.NotEmpty(response.Items)
            );
        }
    }
}

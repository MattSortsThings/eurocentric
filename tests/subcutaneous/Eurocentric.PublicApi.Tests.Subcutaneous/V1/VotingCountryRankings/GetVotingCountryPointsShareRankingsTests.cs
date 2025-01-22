using ErrorOr;
using Eurocentric.Domain.Queries.VotingCountryRankings;
using Eurocentric.PublicApi.Tests.Subcutaneous.Utils;
using Eurocentric.PublicApi.V1.VotingCountryRankings.GetVotingCountryPointsShareRankings;
using Eurocentric.Tests.Utils.Attributes;

namespace Eurocentric.PublicApi.Tests.Subcutaneous.V1.VotingCountryRankings;

public static class GetVotingCountryPointsShareRankingsTests
{
    [PlaceholderTest]
    public sealed class Query : SeededWebAppTests
    {
        public Query(SeededWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_return_fixed_data_given_valid_request()
        {
            // Arrange
            GetVotingCountryPointsShareRankingsQuery query = new("GB");

            // Act
            ErrorOr<VotingCountryPointsSharePage> result = await Sut.Send(query, TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => result.ShouldNotBeError(),
                () => Assert.NotEmpty(result.Value.Items)
            );
        }
    }
}

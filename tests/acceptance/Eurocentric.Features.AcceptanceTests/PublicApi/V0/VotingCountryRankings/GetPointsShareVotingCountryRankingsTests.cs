using System.Net;
using Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.PublicApi.V0.VotingCountryRankings;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.VotingCountryRankings;

public abstract class GetPointsShareVotingCountryRankingsTests : AcceptanceTestBase
{
    protected GetPointsShareVotingCountryRankingsTests(WebAppFixture webAppFixture) : base(webAppFixture) { }

    [Theory]
    [InlineData("GB", "All", "Any")]
    [InlineData("GB", "GrandFinal", "Televote")]
    [InlineData("DE", "All", "Any")]
    [InlineData("DE", "GrandFinal", "Televote")]
    public async Task Should_be_able_to_rank_voting_countries_by_points_share_to_competing_country(string competingCountryCode,
        string contestStages,
        string votingMethod)
    {
        EuroFanActor euroFan = new(PublicApiV0Driver.Create(Client, MajorApiVersion, MinorApiVersion));

        // Given
        euroFan.Given_I_want_to_rank_voting_countries_by_points_share(competingCountryCode: competingCountryCode,
            contestStages: contestStages,
            votingMethod: votingMethod);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        euroFan.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
    }

    private sealed class EuroFanActor : ActorWithResponse<GetVotingCountryPointsShareRankingsResponse>
    {
        private readonly PublicApiV0Driver _driver;

        public EuroFanActor(PublicApiV0Driver driver)
        {
            _driver = driver;
        }

        private Dictionary<string, string> MyQueryParams { get; } = [];

        public void Given_I_want_to_rank_voting_countries_by_points_share(string? contestStages = null,
            string? votingMethod = null, string? competingCountryCode = null)
        {
            if (competingCountryCode is not null)
            {
                MyQueryParams.Add(nameof(competingCountryCode), competingCountryCode);
            }

            if (votingMethod is not null)
            {
                MyQueryParams.Add(nameof(votingMethod), votingMethod);
            }

            if (contestStages is not null)
            {
                MyQueryParams.Add(nameof(contestStages), contestStages);
            }

            SendMyRequest = () =>
                _driver.GetPointsShareVotingCountryRankingsAsync(MyQueryParams, TestContext.Current.CancellationToken);
        }
    }

    public sealed class V0Point2 : GetPointsShareVotingCountryRankingsTests
    {
        public V0Point2(WebAppFixture webAppFixture) : base(webAppFixture) { }


        private protected override int MajorApiVersion => 0;

        private protected override int MinorApiVersion => 2;
    }
}

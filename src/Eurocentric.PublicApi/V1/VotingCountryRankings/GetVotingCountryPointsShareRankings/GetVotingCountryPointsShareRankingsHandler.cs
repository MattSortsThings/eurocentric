using ErrorOr;
using Eurocentric.Domain.Queries.Common;
using Eurocentric.Domain.Queries.VotingCountryRankings;
using MediatR;

namespace Eurocentric.PublicApi.V1.VotingCountryRankings.GetVotingCountryPointsShareRankings;

public sealed class GetVotingCountryPointsShareRankingsHandler :
    IRequestHandler<GetVotingCountryPointsShareRankingsQuery, ErrorOr<VotingCountryPointsSharePage>>
{
    public async Task<ErrorOr<VotingCountryPointsSharePage>> Handle(GetVotingCountryPointsShareRankingsQuery query,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var (target, method, pageIndex, pageSize) = query;

        VotingCountryPointsShareMetadata metadata = new()
        {
            TargetCountryCode = target,
            VotingMethod = method.GetValueOrDefault(VotingMethod.Any),
            PageIndex = pageIndex.GetValueOrDefault(0),
            PageSize = pageSize.GetValueOrDefault(5)
        };

        VotingCountryPointsShareItem[] items =
        [
            new()
            {
                Rank = 1,
                CountryCode = "AT",
                PointsShare = 0.8,
                TotalPoints = 80,
                PossiblePoints = 100,
                CountryName = "Austria"
            },
            new()
            {
                Rank = 2,
                CountryCode = "BE",
                PointsShare = 0.75,
                TotalPoints = 75,
                PossiblePoints = 100,
                CountryName = "Belgium"
            },
            new()
            {
                Rank = 3,
                CountryCode = "CZ",
                PointsShare = 0.7,
                TotalPoints = 70,
                PossiblePoints = 100,
                CountryName = "Czechia"
            },
            new()
            {
                Rank = 4,
                CountryCode = "DE",
                PointsShare = 0.65,
                TotalPoints = 65,
                PossiblePoints = 100,
                CountryName = "Germany"
            },
            new()
            {
                Rank = 5,
                CountryCode = "EE",
                PointsShare = 0.6,
                TotalPoints = 60,
                PossiblePoints = 100,
                CountryName = "Estonia"
            }
        ];

        return new VotingCountryPointsSharePage(items, metadata);
    }
}

using System.ComponentModel;
using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Models;
using Eurocentric.Features.PublicApi.V0.CompetitorRankings.Models;
using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.PublicApi.V0.CompetitorRankings.GetPointsInRangeCompetitorRankings;

internal sealed class GetPointsInRangeCompetitorRankingsQueryHandler : IQueryHandler<GetPointsInRangeCompetitorRankingsQuery,
    GetPointsInRangeCompetitorRankingsResponse>
{
    public async Task<ErrorOr<GetPointsInRangeCompetitorRankingsResponse>> OnHandle(
        GetPointsInRangeCompetitorRankingsQuery query,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        Datum[] data =
        [
            new("GB", "United Kingdom", 0.25d),
            new("FR", "France", 0.725d),
            new("IS", "Iceland", 0.2d),
            new("CH", "Switzerland", 0.8d)
        ];

        RankOrder rankOrder = query.Order.GetValueOrDefault(RankOrder.HiLo);

        PointsInRangeCompetitorRanking[] rankings = rankOrder switch
        {
            RankOrder.HiLo => data.OrderByDescending(d => d.PointsInRangeFrequency)
                .Select((d, i) =>
                    new PointsInRangeCompetitorRanking(i + 1, d.CountryCode, d.CountryName, d.PointsInRangeFrequency))
                .ToArray(),
            RankOrder.LoHi => data.OrderBy(d => d.PointsInRangeFrequency)
                .Select((d, i) =>
                    new PointsInRangeCompetitorRanking(i + 1, d.CountryCode, d.CountryName, d.PointsInRangeFrequency))
                .ToArray(),
            _ => throw new InvalidEnumArgumentException(nameof(query.Order), (int)rankOrder, typeof(RankOrder))
        };

        return ErrorOrFactory.From(new GetPointsInRangeCompetitorRankingsResponse(rankings));
    }

    private sealed record Datum(string CountryCode, string CountryName, double PointsInRangeFrequency);
}

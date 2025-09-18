using ErrorOr;

namespace Eurocentric.Domain.V0Analytics.Rankings.CompetingCountries;

public interface ICompetingCountryRankingsGateway
{
    Task<ErrorOr<PointsInRangeRankingsPage>> GetPointsInRangeRankingsPageAsync(
        PointsInRangeQuery query,
        CancellationToken cancellationToken = default);
}

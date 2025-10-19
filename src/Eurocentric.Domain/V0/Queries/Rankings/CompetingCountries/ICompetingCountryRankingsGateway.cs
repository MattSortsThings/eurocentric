using CSharpFunctionalExtensions;
using Eurocentric.Domain.Functional;

namespace Eurocentric.Domain.V0.Queries.Rankings.CompetingCountries;

public interface ICompetingCountryRankingsGateway
{
    Task<Result<PointsAverageRankings, IDomainError>> GetPointsAverageRankingsAsync(
        PointsAverageQuery query,
        CancellationToken cancellationToken = default
    );
}

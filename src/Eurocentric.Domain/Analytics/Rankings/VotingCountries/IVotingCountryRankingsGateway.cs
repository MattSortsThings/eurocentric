using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.Analytics.Rankings.VotingCountries;

/// <summary>
///     Allows the client to run voting country rankings queries.
/// </summary>
public interface IVotingCountryRankingsGateway
{
    /// <summary>
    ///     Asynchronously ranks voting countries by descending points average.
    /// </summary>
    /// <param name="query">The query parameters.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous ranking operation to
    ///     complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous ranking operation. The task's result is the retrieved
    ///     rankings object.
    /// </returns>
    Task<Result<PointsAverageRankings, IDomainError>> GetPointsAverageRankingsAsync(
        PointsAverageQuery query,
        CancellationToken cancellationToken = default
    );
}

using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.Analytics.Rankings.CompetingCountries;

/// <summary>
///     Allows the client to run competing country rankings queries.
/// </summary>
public interface ICompetingCountryRankingsGateway
{
    /// <summary>
    ///     Asynchronously ranks competing countries by descending points average.
    /// </summary>
    /// <param name="query">The query parameters.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous ranking operation to
    ///     complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous ranking operation. The task's result is <i>either</i>
    ///     the rankings result object <i>or</i> an error.
    /// </returns>
    Task<Result<PointsAverageRankings, IDomainError>> GetPointsAverageRankingsAsync(
        PointsAverageQuery query,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Asynchronously ranks competing countries by descending points consensus.
    /// </summary>
    /// <param name="query">The query parameters.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous ranking operation to
    ///     complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous ranking operation. The task's result is <i>either</i>
    ///     the rankings result object <i>or</i> an error.
    /// </returns>
    Task<Result<PointsConsensusRankings, IDomainError>> GetPointsConsensusRankingsAsync(
        PointsConsensusQuery query,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Asynchronously ranks competing countries by descending points share
    /// </summary>
    /// <param name="query">The query parameters.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous ranking operation to
    ///     complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous ranking operation. The task's result is <i>either</i>
    ///     the rankings result object <i>or</i> an error.
    /// </returns>
    Task<Result<PointsShareRankings, IDomainError>> GetPointsShareRankingsAsync(
        PointsShareQuery query,
        CancellationToken cancellationToken = default
    );
}

using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     Allows the client to run listings queries.
/// </summary>
public interface IListingsGateway
{
    /// <summary>
    ///     Asynchronously retrieves the result listings for a specified broadcast.
    /// </summary>
    /// <param name="query">The query parameters.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous listing operation to
    ///     complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous listing operation. The task's result is <i>either</i>
    ///     the listings result object <i>or</i> an error.
    /// </returns>
    Task<Result<BroadcastResultListings, IDomainError>> GetBroadcastResultListingsAsync(
        BroadcastResultQuery query,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Asynchronously retrieves the points listings for a specified competing country in a specified broadcast.
    /// </summary>
    /// <param name="query">The query parameters.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous listing operation to
    ///     complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous listing operation. The task's result is <i>either</i>
    ///     the listings result object <i>or</i> an error.
    /// </returns>
    Task<Result<CompetingCountryPointsListings, IDomainError>> GetCompetingCountryPointsListingsAsync(
        CompetingCountryPointsQuery query,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Asynchronously retrieves the points listings for a specified voting country in a specified broadcast.
    /// </summary>
    /// <param name="query">The query parameters.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous listing operation to
    ///     complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous listing operation. The task's result is <i>either</i>
    ///     the listings result object <i>or</i> an error.
    /// </returns>
    Task<Result<VotingCountryPointsListings, IDomainError>> GetVotingCountryPointsListingsAsync(
        VotingCountryPointsQuery query,
        CancellationToken cancellationToken = default
    );
}

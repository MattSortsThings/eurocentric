using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     Allows the client to run listings queries.
/// </summary>
public interface IListingsGateway
{
    /// <summary>
    ///     Asynchronously retrieves the points listings for the specified competing country in the specified broadcast.
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
}

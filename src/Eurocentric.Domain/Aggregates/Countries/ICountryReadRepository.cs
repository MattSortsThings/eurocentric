using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Read-only repository for <see cref="Country" /> aggregates.
/// </summary>
public interface ICountryReadRepository
{
    /// <summary>
    ///     Asynchronously retrieves all the <see cref="Country" /> aggregates in the system, in an untracked state, ordered by
    ///     <see cref="Country.CountryCode" />.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous retrieval operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous retrieval operation. The task result is the retrieved array
    ///     of untracked <see cref="Country" /> aggregates.
    /// </returns>
    Task<Country[]> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves the <see cref="Country" /> aggregate in the system with the specified
    ///     <see cref="Country.Id" /> value, in an untracked state, if a matching aggregate exists.
    /// </summary>
    /// <param name="countryId">The ID of the requested country.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous retrieval operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous retrieval operation. The task result is <i>either</i>
    ///     the retrieved untracked <see cref="Country" /> <i>or</i> an error.
    /// </returns>
    Task<Result<Country, IDomainError>> GetByIdAsync(
        CountryId countryId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Returns all the <see cref="Country" /> aggregates in the system as an untracked queryable.
    /// </summary>
    /// <returns>An object to allow read-only queries on the <see cref="Country" /> aggregates.</returns>
    IQueryable<Country> GetAsQueryable();
}

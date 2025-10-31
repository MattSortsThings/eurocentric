using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Read-write repository for <see cref="Country" /> aggregates.
/// </summary>
public interface ICountryWriteRepository
{
    /// <summary>
    ///     Adds the specified <see cref="Country" /> aggregate to the repository.
    /// </summary>
    /// <remarks>Changes are not committed until the <see cref="SaveChangesAsync" /> method is invoked.</remarks>
    /// <param name="country">The country to be added.</param>
    void Add(Country country);

    /// <summary>
    ///     Updates the specified <see cref="Country" /> aggregate in the repository.
    /// </summary>
    /// <remarks>Changes are not committed until the <see cref="SaveChangesAsync" /> method is invoked.</remarks>
    /// <param name="country">The country to be updated.</param>
    void Update(Country country);

    /// <summary>
    ///     Asynchronously retrieves the <see cref="Country" /> aggregate in the system with the specified
    ///     <see cref="Country.Id" /> value, in a tracked state, if a matching aggregate exists.
    /// </summary>
    /// <param name="countryId">The ID of the requested country.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous retrieval operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous retrieval operation. The task result is <i>either</i>
    ///     the retrieved tracked <see cref="Country" /> <i>or</i> an error.
    /// </returns>
    Task<Result<Country, IDomainError>> GetByIdAsync(
        CountryId countryId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Asynchronously saves all changes in the current transaction.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous commit
    ///     operation to complete.
    /// </param>
    /// <returns>A <see cref="Task" /> representing the asynchronous commit operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

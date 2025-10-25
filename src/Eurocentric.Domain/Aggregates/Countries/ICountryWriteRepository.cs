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
    ///     Asynchronously saves all changes in the current transaction.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous commit
    ///     operation to complete.
    /// </param>
    /// <returns>A <see cref="Task" /> representing the asynchronous commit operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

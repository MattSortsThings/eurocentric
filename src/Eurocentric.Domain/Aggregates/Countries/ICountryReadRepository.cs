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
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous
    ///     retrieval operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous retrieval operation. The task result is the array of
    ///     retrieved untracked <see cref="Country" /> objects.
    /// </returns>
    Task<Country[]> GetAllAsync(CancellationToken cancellationToken = default);
}

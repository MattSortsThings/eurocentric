using CSharpFunctionalExtensions;

namespace Eurocentric.Domain.Core;

/// <summary>
///     A read-write repository for a domain aggregate.
/// </summary>
/// <typeparam name="TAggregate">The domain aggregate type.</typeparam>
/// <typeparam name="TId">The aggregate ID type.</typeparam>
public interface IWriteRepository<TAggregate, in TId>
    where TAggregate : AggregateRoot<TId>
    where TId : GuidAtomicValueObject
{
    /// <summary>
    ///     Adds the specified aggregate to the repository.
    /// </summary>
    /// <remarks>The added aggregate is not persisted until changes are saved for the unit of work.</remarks>
    /// <param name="aggregate">The aggregate to be added.</param>
    /// <exception cref="ArgumentNullException"><paramref name="aggregate" /> is <see langword="null" />.</exception>
    void Add(TAggregate aggregate);

    /// <summary>
    ///     Asynchronously retrieves the requested aggregate in a tracked state.
    /// </summary>
    /// <param name="id">The aggregate ID.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous retrieval operation to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous retrieval operation. The task's result is <i>either</i>
    ///     the retrieved aggregate <i>or</i> an error.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="id" /> is <see langword="null" />.</exception>
    Task<Result<TAggregate, IDomainError>> GetTrackedAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Removes the specified aggregate from the repository.
    /// </summary>
    /// <remarks>The removed aggregate is not persisted until changes are saved for the unit of work.</remarks>
    /// <param name="aggregate">The aggregate to be removed.</param>
    /// <exception cref="ArgumentNullException"><paramref name="aggregate" /> is <see langword="null" />.</exception>
    void Remove(TAggregate aggregate);

    /// <summary>
    ///     Updates the specified aggregate in the repository.
    /// </summary>
    /// <remarks>The updated aggregate is not persisted until changes are saved for the unit of work.</remarks>
    /// <param name="aggregate">The aggregate to be updated.</param>
    /// <exception cref="ArgumentNullException"><paramref name="aggregate" /> is <see langword="null" />.</exception>
    void Update(TAggregate aggregate);
}

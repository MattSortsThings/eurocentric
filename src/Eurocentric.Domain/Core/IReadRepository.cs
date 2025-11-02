using System.Linq.Expressions;
using CSharpFunctionalExtensions;

namespace Eurocentric.Domain.Core;

/// <summary>
///     A read-only repository for a domain aggregate.
/// </summary>
/// <typeparam name="TAggregate">The domain aggregate type.</typeparam>
/// <typeparam name="TId">The aggregate ID type.</typeparam>
public interface IReadRepository<TAggregate, in TId>
    where TAggregate : AggregateRoot<TId>
    where TId : GuidAtomicValueObject
{
    /// <summary>
    ///     Asynchronously retrieves the requested aggregate in an untracked state.
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
    Task<Result<TAggregate, IDomainError>> GetUntrackedAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves a sorted array of all the aggregates in an untracked state.
    /// </summary>
    /// <param name="sortKey">Specifies the sort key.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous retrieval operation to complete.
    /// </param>
    /// <typeparam name="TKey">The sort key type.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous retrieval operation. The task's result is the retrieved
    ///     array of aggregates.
    /// </returns>
    Task<TAggregate[]> GetAllUntrackedAsync<TKey>(
        Expression<Func<TAggregate, TKey>> sortKey,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Retrieves an untracked queryable for the aggregates.
    /// </summary>
    /// <returns>An interface against which the aggregates may be queried.</returns>
    IQueryable<TAggregate> GetUntrackedQueryable();
}

namespace Eurocentric.Domain.Core;

/// <summary>
///     A unit of work that must be committed or rolled back as a single transaction.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///     Asynchronously saves all changes to tracked aggregates as part of the unit of work.
    /// </summary>
    /// <remarks>
    ///     When this method is invoked, all domain events in tracked aggregates are published and handled before the
    ///     entire transaction is committed or rolled back.
    /// </remarks>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the asynchronous save
    ///     changes operation to complete.
    /// </param>
    /// <returns>A <see cref="Task" /> representing the asynchronous save changes operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

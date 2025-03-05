namespace Eurocentric.Domain.BaseTypes;

/// <summary>
///     Abstract base class for a domain aggregate root entity type.
/// </summary>
/// <typeparam name="TId">The identifier type.</typeparam>
public abstract class AggregateRoot<TId> : Entity where TId : ValueObject
{
    private protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    /// <summary>
    ///     Gets an equality comparer that determines two <see cref="AggregateRoot{TId}" /> instances to be equal if they are
    ///     the same instance or if they are two different instances with equal <see cref="AggregateRoot{TId}.Id" /> values.
    /// </summary>
    public static IEqualityComparer<AggregateRoot<TId>> IdComparer { get; } = new IdEqualityComparer();

    /// <summary>
    ///     Gets the aggregate's system identifier.
    /// </summary>
    public TId Id { get; protected set; } = null!;

    private sealed class IdEqualityComparer : IEqualityComparer<AggregateRoot<TId>>
    {
        public bool Equals(AggregateRoot<TId>? x, AggregateRoot<TId>? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null)
            {
                return false;
            }

            if (y is null)
            {
                return false;
            }

            return x.GetType() == y.GetType() && EqualityComparer<TId>.Default.Equals(x.Id, y.Id);
        }

        public int GetHashCode(AggregateRoot<TId> obj) => EqualityComparer<TId>.Default.GetHashCode(obj.Id);
    }
}

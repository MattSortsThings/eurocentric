namespace Eurocentric.Domain.BaseTypes;

/// <summary>
///     Abstract base class for a domain entity type.
/// </summary>
/// <remarks>
///     An entity only ever exists as part of an aggregate root entity (its owner). An owned entity has an identity
///     local to its aggregate.
/// </remarks>
public abstract class Entity
{
    private protected Entity() { }
}

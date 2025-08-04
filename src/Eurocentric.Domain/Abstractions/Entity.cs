using JetBrains.Annotations;

namespace Eurocentric.Domain.Abstractions;

/// <summary>
///     Abstract base class for a domain entity type.
/// </summary>
public abstract class Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private protected Entity() { }
}

using JetBrains.Annotations;

namespace Eurocentric.Domain.Core;

/// <summary>
///     Abstract base class for a domain entity.
/// </summary>
public abstract class Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private protected Entity() { }
}

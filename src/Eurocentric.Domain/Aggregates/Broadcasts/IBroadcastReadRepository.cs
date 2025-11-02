using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Read-only repository for <see cref="Broadcast" /> aggregates.
/// </summary>
public interface IBroadcastReadRepository : IReadRepository<Broadcast, BroadcastId>;

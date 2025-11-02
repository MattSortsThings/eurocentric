using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Read-write repository for <see cref="Broadcast" /> aggregates.
/// </summary>
public interface IBroadcastWriteRepository : IWriteRepository<Broadcast, BroadcastId>;

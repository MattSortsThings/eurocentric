namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Repository for <see cref="Broadcast" /> aggregates.
/// </summary>
public interface IBroadcastRepository : IBroadcastReadRepository, IBroadcastWriteRepository;

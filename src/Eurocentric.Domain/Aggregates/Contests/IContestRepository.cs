namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Repository for <see cref="Contest" /> aggregates.
/// </summary>
public interface IContestRepository : IContestReadRepository, IContestWriteRepository;

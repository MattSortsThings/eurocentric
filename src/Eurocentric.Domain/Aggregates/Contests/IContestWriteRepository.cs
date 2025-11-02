using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Read-write repository for <see cref="Contest" /> aggregates.
/// </summary>
public interface IContestWriteRepository : IWriteRepository<Contest, ContestId>;

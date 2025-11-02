using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Read-only repository for <see cref="Contest" /> aggregates.
/// </summary>
public interface IContestReadRepository : IReadRepository<Contest, ContestId>;

using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Infrastructure.IdGenerators;

/// <summary>
///     Generates <see cref="ContestId" /> value objects.
/// </summary>
/// <param name="timeProvider">Provides the system time.</param>
internal sealed class ContestIdGenerator(TimeProvider timeProvider) : IContestIdGenerator
{
    /// <inheritdoc />
    public ContestId CreateSingle() => ContestId.Create(timeProvider.GetUtcNow());
}

using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Infrastructure.IdGenerators;

/// <summary>
///     Generates a <see cref="ContestId" /> instance on demand.
/// </summary>
internal sealed class ContestIdGenerator(TimeProvider timeProvider) : IContestIdGenerator
{
    /// <inheritdoc />
    public ContestId Generate() => ContestId.Create(timeProvider.GetUtcNow());
}

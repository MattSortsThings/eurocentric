using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Infrastructure.IdProviders;

internal sealed class ContestIdProvider(TimeProvider timeProvider) : IContestIdProvider
{
    public ContestId CreateSingle() => ContestId.FromValue(Guid.CreateVersion7(timeProvider.GetUtcNow()));
}

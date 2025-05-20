using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Infrastructure.IdProviders;

internal sealed class ContestIdProvider(TimeProvider timeProvider) : IContestIdProvider
{
    public ContestId Create() => ContestId.FromValue(Guid.CreateVersion7(timeProvider.GetUtcNow()));
}

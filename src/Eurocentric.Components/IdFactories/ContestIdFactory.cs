using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Components.IdFactories;

internal sealed class ContestIdFactory(TimeProvider timeProvider) : IContestIdFactory
{
    public ContestId Create() => ContestId.FromValue(Guid.CreateVersion7(timeProvider.GetUtcNow()));
}

using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.Utilities;

public sealed class FixedBroadcastIdGenerator(BroadcastId fixedId) : IBroadcastIdGenerator
{
    public BroadcastId Generate() => fixedId;
}

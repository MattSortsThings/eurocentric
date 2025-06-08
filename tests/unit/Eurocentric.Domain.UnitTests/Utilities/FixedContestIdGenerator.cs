using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.Utilities;

public sealed class FixedContestIdGenerator(ContestId fixedId) : IContestIdGenerator
{
    public ContestId Generate() => fixedId;
}

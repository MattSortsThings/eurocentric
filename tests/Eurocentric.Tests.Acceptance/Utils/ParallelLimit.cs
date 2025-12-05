using TUnit.Core.Interfaces;

namespace Eurocentric.Tests.Acceptance.Utils;

public sealed class ParallelLimit : IParallelLimit
{
    public int Limit => 12;
}

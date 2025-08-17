using TUnit.Core.Interfaces;

namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

public sealed class ParallelLimit : IParallelLimit
{
    public int Limit => 8;
}

using TUnit.Core.Interfaces;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public sealed class ParallelLimit : IParallelLimit
{
    public int Limit => 8;
}

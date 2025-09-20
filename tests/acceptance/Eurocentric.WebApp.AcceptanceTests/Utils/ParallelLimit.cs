using TUnit.Core.Interfaces;

namespace Eurocentric.WebApp.AcceptanceTests.Utils;

public sealed class ParallelLimit : IParallelLimit
{
    public int Limit => 8;
}

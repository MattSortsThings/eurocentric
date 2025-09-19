using TUnit.Core.Interfaces;

namespace Eurocentric.WebApp.AcceptanceTests;

public sealed class ParallelLimit : IParallelLimit
{
    public int Limit => 8;
}

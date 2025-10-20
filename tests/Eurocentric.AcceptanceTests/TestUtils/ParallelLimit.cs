using TUnit.Core.Interfaces;

namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     Limits the number of tests that may be run in parallel.
/// </summary>
public sealed class ParallelLimit : IParallelLimit
{
    public int Limit => 10;
}

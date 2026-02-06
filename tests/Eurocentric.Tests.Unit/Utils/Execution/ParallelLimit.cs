using TUnit.Core.Interfaces;

namespace Eurocentric.Tests.Unit.Utils.Execution;

/// <summary>
///     Imposes a limit on the number of tests that can be run in parallel.
/// </summary>
public sealed class ParallelLimit : IParallelLimit
{
    public int Limit => Environment.ProcessorCount;
}

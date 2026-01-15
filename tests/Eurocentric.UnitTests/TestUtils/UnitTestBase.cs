namespace Eurocentric.UnitTests.TestUtils;

/// <summary>
///     Abstract base class for all unit tests.
/// </summary>
[Category("unit")]
[UseQualifiedDisplayName]
[ParallelLimiter<ParallelLimit>]
public abstract class UnitTestBase;

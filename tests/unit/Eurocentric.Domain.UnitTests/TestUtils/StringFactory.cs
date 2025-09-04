namespace Eurocentric.Domain.UnitTests.TestUtils;

public static class StringFactory
{
    public static string WithLength(int length) => new('A', length);
}

namespace Eurocentric.Tests.Assertions;

public static class BooleanExtensions
{
    public static void ShouldBeTrue(this bool value) => Assert.True(value);

    public static void ShouldBeFalse(this bool value) => Assert.False(value);
}

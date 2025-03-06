namespace Eurocentric.Domain.Tests.Unit.Utils.Assertions;

public static class IntegerExtensions
{
    public static void ShouldBeZero(this int subject) => Assert.Equal(0, subject);

    public static void ShouldBePositive(this int subject) => Assert.True(subject > 0);

    public static void ShouldBeNegative(this int subject) => Assert.True(subject < 0);
}

namespace Eurocentric.Tests.Assertions;

public static class ExceptionExtensions
{
    public static void ShouldHaveMessage(this Exception exception, string expectedMessage) =>
        Assert.Equal(expectedMessage, exception.Message);
}

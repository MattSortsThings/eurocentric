namespace Eurocentric.Tests.Assertions;

public static class ActionExtensions
{
    public static T ShouldThrow<T>(this Action action) where T : Exception => Assert.Throws<T>(action);
}

namespace Eurocentric.Tests.Assertions;

public static class EnumerableExtensions
{
    public static void ShouldNotContain<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) =>
        Assert.All(enumerable, item => Assert.False(predicate(item)));


    public static void ShouldContainSingle<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) =>
        Assert.Single(enumerable, item => predicate(item));

    public static void ShouldBeEmpty<T>(this IEnumerable<T> enumerable) => Assert.Empty(enumerable);

    public static void ShouldNotBeEmpty<T>(this IEnumerable<T> enumerable) => Assert.NotEmpty(enumerable);
}

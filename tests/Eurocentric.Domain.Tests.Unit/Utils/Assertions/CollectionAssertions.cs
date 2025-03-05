namespace Eurocentric.Domain.Tests.Unit.Utils.Assertions;

public static class CollectionAssertions
{
    public static void ShouldNotContain<T>(this IEnumerable<T> collection, T value) =>
        Assert.All(collection, item => Assert.NotEqual(value, item));

    public static void ShouldContainSingle<T>(this IEnumerable<T> collection, T value) =>
        Assert.Single(collection, item => item is not null && item.Equals(value));
}

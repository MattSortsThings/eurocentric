namespace Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;

public static class CollectionExtensions
{
    public static void ShouldBeEmpty<T>(this ICollection<T> collection) => Assert.Empty(collection);
}

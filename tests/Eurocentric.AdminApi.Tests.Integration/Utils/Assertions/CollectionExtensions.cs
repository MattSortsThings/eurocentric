namespace Eurocentric.AdminApi.Tests.Integration.Utils.Assertions;

public static class CollectionExtensions
{
    public static void ShouldBeEmpty<T>(this ICollection<T> collection) => Assert.Empty(collection);
}

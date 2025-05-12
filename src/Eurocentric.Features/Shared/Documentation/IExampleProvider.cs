namespace Eurocentric.Features.Shared.Documentation;

/// <summary>
///     Provides a fixed example of a type.
/// </summary>
/// <remarks>
///     This interface is adapted from a much more sophisticated pattern in the
///     <a href="https://github.com/martincostello/openapi-extensions">MartinCostello.OpenApi.Extensions</a> library by
///     Martin Costello.
/// </remarks>
/// <typeparam name="T">The exemplified type.</typeparam>
internal interface IExampleProvider<out T>
{
    /// <summary>
    ///     Creates and returns an example of type <typeparamref name="T" />.
    /// </summary>
    /// <returns>An instance of type <typeparamref name="T" />.</returns>
    public static abstract T CreateExample();
}

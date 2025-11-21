namespace Eurocentric.Components.OpenApi;

/// <summary>
///     Creates an OpenAPI schema example for a DTO class.
/// </summary>
/// <remarks>
///     This interface is adapted from a much more sophisticated pattern in the
///     <a href="https://github.com/martincostello/openapi-extensions">MartinCostello.OpenApi.Extensions</a> library by
///     Martin Costello.
/// </remarks>
/// <typeparam name="T">The schema type.</typeparam>
public interface IDtoSchemaExampleProvider<out T>
    where T : class
{
    /// <summary>
    ///     Creates and returns an example of the schema type.
    /// </summary>
    /// <returns>A new instance of type <typeparamref name="T" />.</returns>
    static abstract T CreateExample();
}

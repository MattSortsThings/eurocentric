namespace Eurocentric.PublicApi.V0.Greetings.GetGreetings;

internal static class Extensions
{
    internal static GetGreetingsQuery ToQuery(this GetGreetingsRequest request) => new(request.Quantity, request.Language);
}

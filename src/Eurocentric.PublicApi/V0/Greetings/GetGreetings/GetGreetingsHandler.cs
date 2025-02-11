using Eurocentric.PublicApi.V0.Greetings.Common;
using MediatR;

namespace Eurocentric.PublicApi.V0.Greetings.GetGreetings;

internal sealed class GetGreetingsHandler : IRequestHandler<GetGreetingsQuery, GetGreetingsResponse>
{
    public Task<GetGreetingsResponse> Handle(GetGreetingsQuery request, CancellationToken cancellationToken)
    {
        string message = request.Language switch
        {
            Language.English => "hi!",
            Language.French => "bonjour!",
            Language.Dutch => "hoi!",
            _ => throw new InvalidOperationException("Invalid enum value.")
        };

        Greeting[] greetings = Enumerable.Repeat(new Greeting(message, request.Language), request.Quantity).ToArray();

        return Task.FromResult(new GetGreetingsResponse(greetings));
    }
}

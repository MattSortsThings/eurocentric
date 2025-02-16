using ErrorOr;
using Eurocentric.PublicApi.V0.Greetings.Models;
using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.PublicApi.V0.Greetings.GetGreetings;

internal sealed class GetGreetingsHandler : QueryHandler<GetGreetingsQuery, GetGreetingsResult>
{
    public override async Task<ErrorOr<GetGreetingsResult>> Handle(GetGreetingsQuery query,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var (quantity, language, clientName) = query;

        string salutation = language switch
        {
            Language.English => "Hi",
            Language.French => "Bonjour",
            Language.Dutch => "Hoi",
            Language.Swedish => "Hej",
            _ => throw new InvalidOperationException("Invalid enum value")
        };

        string greeting = clientName is null ? $"{salutation}!" : $"{salutation} {clientName}!";

        return new GetGreetingsResult(Enumerable.Repeat(greeting, quantity).ToArray());
    }
}

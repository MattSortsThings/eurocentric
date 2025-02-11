using Eurocentric.PublicApi.V0.Greetings.Common;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.PublicApi.V0.Greetings.GetGreetings;

public sealed record GetGreetingsRequest
{
    [FromQuery(Name = "quantity")]
    public required int Quantity { get; init; }

    [FromQuery(Name = "language")]
    public required Language Language { get; init; }
}

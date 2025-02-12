using System.ComponentModel;
using Eurocentric.PublicApi.V0.Greetings.Common;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.PublicApi.V0.Greetings.GetGreetings;

public sealed record GetGreetingsRequest
{
    [DefaultValue(1)]
    [FromQuery(Name = "quantity")]
    [Description("The quantity of greetings to be generated.")]
    public required int Quantity { get; init; }

    [DefaultValue(Language.English)]
    [FromQuery(Name = "language")]
    [Description("The language of the greetings to be generated.")]
    public required Language Language { get; init; }
}

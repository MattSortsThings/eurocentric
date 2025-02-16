using System.ComponentModel.DataAnnotations;
using Eurocentric.PublicApi.V0.Greetings.Models;
using Eurocentric.Shared.AppPipeline;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.PublicApi.V0.Greetings.GetGreetings;

public sealed record GetGreetingsQuery : Query<GetGreetingsResult>
{
    [Required]
    [FromQuery(Name = "quantity")]
    public required int Quantity { get; init; }

    [Required]
    [FromQuery(Name = "language")]
    public required Language Language { get; init; }

    [FromQuery(Name = "clientName")]
    public string? ClientName { get; init; }

    internal void Deconstruct(out int quantity, out Language language, out string? clientName)
    {
        quantity = Quantity;
        language = Language;
        clientName = ClientName;
    }
}

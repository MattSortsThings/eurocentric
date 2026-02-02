using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Apis.Public.V0.Placeholders;

public sealed record GetBlobbiesQueryParams
{
    [Required]
    [FromQuery(Name = "count")]
    public required int Count { get; init; }
}

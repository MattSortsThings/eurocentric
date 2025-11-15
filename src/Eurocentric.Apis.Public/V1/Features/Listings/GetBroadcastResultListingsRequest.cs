using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Eurocentric.Apis.Public.V1.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Apis.Public.V1.Features.Listings;

public sealed record GetBroadcastResultListingsRequest
{
    [Required]
    [FromQuery(Name = "contestYear")]
    [Description("Filters voting data by inclusive maximum contest year.")]
    public required int ContestYear { get; init; }

    [Required]
    [FromQuery(Name = "contestStage")]
    [Description("Filters voting data by contest stage.")]
    public required ContestStage ContestStage { get; init; }
}

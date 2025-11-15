using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Eurocentric.Apis.Public.V1.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Apis.Public.V1.Features.Listings;

public sealed record GetCompetingCountryPointsListingsRequest
{
    [Required]
    [FromQuery(Name = "contestYear")]
    [Description("Filters voting data by inclusive maximum contest year.")]
    public required int ContestYear { get; init; }

    [Required]
    [FromQuery(Name = "contestStage")]
    [Description("Filters voting data by contest stage.")]
    public required ContestStage ContestStage { get; init; }

    [Required]
    [RegularExpression("^[A-Z]{2}$")]
    [FromQuery(Name = "competingCountryCode")]
    [Description("Filters voting data by competing country code.")]
    public required string CompetingCountryCode { get; init; }
}

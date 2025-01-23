using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Eurocentric.PublicApi.V0.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.PublicApi.V0.VotingCountryRankings.GetVotingCountryPointsShareRankings;

public sealed record GetVotingCountryPointsShareRankingsRequest
{
    [FromQuery(Name = "targetCountryCode")]
    [Required]
    public required string TargetCountryCode { get; init; }

    [FromQuery(Name = "votingMethod")]
    [DefaultValue(V0.Models.VotingMethod.Any)]
    public VotingMethod? VotingMethod { get; init; }

    [FromQuery(Name = "pageIndex")]
    [DefaultValue(0)]
    public int? PageIndex { get; init; }

    [FromQuery(Name = "pageSize")]
    [DefaultValue(5)]
    public int? PageSize { get; init; }
}

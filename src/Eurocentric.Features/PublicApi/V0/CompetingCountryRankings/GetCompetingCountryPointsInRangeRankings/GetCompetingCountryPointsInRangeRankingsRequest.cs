using System.ComponentModel.DataAnnotations;
using Eurocentric.Features.PublicApi.V0.Common.Dtos;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V0.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings;

public sealed record GetCompetingCountryPointsInRangeRankingsRequest : PaginatedRequest
{
    [FromQuery(Name = "minPoints")]
    [Required]
    public required int MinPoints { get; init; }

    [FromQuery(Name = "maxPoints")]
    [Required]
    public required int MaxPoints { get; init; }

    [FromQuery(Name = "minYear")]
    public int? MinYear { get; init; }

    [FromQuery(Name = "maxYear")]
    public int? MaxYear { get; init; }

    [FromQuery(Name = "contestStage")]
    public QueryableContestStage? ContestStage { get; init; }

    [FromQuery(Name = "votingMethod")]
    public QueryableVotingMethod? VotingMethod { get; init; }

    [FromQuery(Name = "votingCountryCode")]
    public string? VotingCountryCode { get; init; }
}

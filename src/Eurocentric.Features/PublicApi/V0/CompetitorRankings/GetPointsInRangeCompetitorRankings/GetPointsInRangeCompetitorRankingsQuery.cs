using System.ComponentModel.DataAnnotations;
using Eurocentric.Features.PublicApi.V0.Common.Models;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V0.CompetitorRankings.GetPointsInRangeCompetitorRankings;

public sealed record GetPointsInRangeCompetitorRankingsQuery : IQuery<GetPointsInRangeCompetitorRankingsResponse>
{
    [FromQuery(Name = "minValue")]
    [Required]
    public required int MinValue { get; init; }

    [FromQuery(Name = "maxValue")]
    [Required]
    public required int MaxValue { get; init; }

    [FromQuery(Name = "votingCountryCode")]
    public string? VotingCountryCode { get; init; }

    [FromQuery(Name = "order")]
    public RankOrder? Order { get; init; }

    [FromQuery(Name = "pageIndex")]
    public int? PageIndex { get; init; }
}

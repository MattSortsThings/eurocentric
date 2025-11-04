using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Eurocentric.Apis.Public.V1.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Apis.Public.V1.Features.Rankings.VotingCountries;

public sealed record GetVotingCountryPointsAverageRankingsRequest
{
    [Required]
    [FromQuery(Name = "competingCountryCode")]
    public required string CompetingCountryCode { get; init; }

    [FromQuery(Name = "minYear")]
    public int? MinYear { get; init; }

    [FromQuery(Name = "maxYear")]
    public int? MaxYear { get; init; }

    [FromQuery(Name = "contestStage")]
    public ContestStageFilter? ContestStage { get; init; }

    [FromQuery(Name = "votingMethod")]
    public VotingMethodFilter? VotingMethod { get; init; }

    [DefaultValue(0)]
    [FromQuery(Name = "pageIndex")]
    public int? PageIndex { get; init; }

    [DefaultValue(10)]
    [FromQuery(Name = "pageSize")]
    public int? PageSize { get; init; }

    [DefaultValue(false)]
    [FromQuery(Name = "descending")]
    public bool? Descending { get; init; }
}

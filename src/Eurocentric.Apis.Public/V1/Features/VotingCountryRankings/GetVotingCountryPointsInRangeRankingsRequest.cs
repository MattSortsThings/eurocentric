using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Eurocentric.Apis.Public.V1.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Apis.Public.V1.Features.VotingCountryRankings;

public sealed record GetVotingCountryPointsInRangeRankingsRequest
{
    [Required]
    [RegularExpression("^[A-Z]{2}$")]
    [FromQuery(Name = "competingCountryCode")]
    [Description("Filters voting data by competing country code.")]
    public required string CompetingCountryCode { get; init; }

    [FromQuery(Name = "minPoints")]
    [Description("Specifies the inclusive minimum points value.")]
    public required int MinPoints { get; init; }

    [FromQuery(Name = "maxPoints")]
    [Description("Specifies the inclusive maximum points value.")]
    public required int MaxPoints { get; init; }

    [FromQuery(Name = "minYear")]
    [Description("Filters voting data by inclusive minimum contest year.")]
    public int? MinYear { get; init; }

    [FromQuery(Name = "maxYear")]
    [Description("Filters voting data by inclusive maximum contest year.")]
    public int? MaxYear { get; init; }

    [FromQuery(Name = "contestStage")]
    [Description("Filters voting data by contest stage.")]
    public ContestStageFilter? ContestStage { get; init; }

    [FromQuery(Name = "votingMethod")]
    public VotingMethodFilter? VotingMethod { get; init; }

    [DefaultValue(0)]
    [FromQuery(Name = "pageIndex")]
    [Description("Sets the zero-based pagination page index.")]
    public int? PageIndex { get; init; }

    [DefaultValue(10)]
    [FromQuery(Name = "pageSize")]
    [Description("Sets the pagination page size.")]
    public int? PageSize { get; init; }

    [DefaultValue(false)]
    [FromQuery(Name = "descending")]
    [Description("Specifies descending rank (true) or ascending rank (false) initial sort before pagination.")]
    public bool? Descending { get; init; }
}

using System.ComponentModel;
using Eurocentric.Apis.Public.V1.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Apis.Public.V1.Features.CompetitorRankings;

public sealed record GetCompetitorPointsAverageRankingsRequest
{
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
    [Description("Filters voting data by voting method.")]
    public VotingMethodFilter? VotingMethod { get; init; }

    [DefaultValue(0)]
    [FromQuery(Name = "pageIndex")]
    [Description("Sets the pagination page index.")]
    public int? PageIndex { get; init; }

    [DefaultValue(10)]
    [FromQuery(Name = "pageSize")]
    [Description("Sets the pagination page size.")]
    public int? PageSize { get; init; }

    [DefaultValue(false)]
    [FromQuery(Name = "descending")]
    [Description("Sorts rankings initially by descending rank (true) or ascending rank (false).")]
    public bool? Descending { get; init; }
}

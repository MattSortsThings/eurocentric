using System.ComponentModel;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsInRangeRankings;

public sealed record GetCompetingCountryPointsInRangeRankingsRequest : PaginatedRequest
{
    [FromQuery(Name = "minPoints")]
    [Description("Sets the inclusive minimum points value for the queried data.")]
    public required int MinPoints { get; init; }

    [FromQuery(Name = "maxPoints")]
    [Description("Sets the inclusive maximum points value for the queried data.")]
    public required int MaxPoints { get; init; }

    [FromQuery(Name = "contestStage")]
    [Description("Filters the queried data to points awarded in a specific contest stage.")]
    [DefaultValue(typeof(QueryableContestStage), nameof(QueryableContestStage.Any))]
    public QueryableContestStage? ContestStage { get; init; }

    [FromQuery(Name = "minYear")]
    [Description("Filters the queried data to points awarded in or after a specific contest year.")]
    public int? MinYear { get; init; }

    [FromQuery(Name = "maxYear")]
    [Description("Filters the queried data to points awarded in or before a specific contest year.")]
    public int? MaxYear { get; init; }

    [FromQuery(Name = "votingCountryCode")]
    [Description("Filters the queried data to points awarded by a specific voting country.")]
    public string? VotingCountryCode { get; init; }

    [FromQuery(Name = "votingMethod")]
    [Description("Filters the queried data to points awarded using a specific voting method.")]
    [DefaultValue(typeof(QueryableVotingMethod), nameof(QueryableVotingMethod.Any))]
    public QueryableVotingMethod? VotingMethod { get; init; }
}

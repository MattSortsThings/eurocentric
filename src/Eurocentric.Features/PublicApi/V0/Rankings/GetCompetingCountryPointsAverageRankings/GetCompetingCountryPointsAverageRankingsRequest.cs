using System.ComponentModel;
using Eurocentric.Features.PublicApi.V0.Common.Dtos;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V0.Rankings.GetCompetingCountryPointsAverageRankings;

public sealed record GetCompetingCountryPointsAverageRankingsRequest : PaginatedQueryParams
{
    [FromQuery(Name = "votingMethod")]
    [DefaultValue(typeof(QueryableVotingMethod), nameof(QueryableVotingMethod.Any))]
    public QueryableVotingMethod? VotingMethod { get; init; }

    [FromQuery(Name = "contestStage")]
    [DefaultValue(typeof(QueryableContestStage), nameof(QueryableContestStage.Any))]
    public QueryableContestStage? ContestStage { get; init; }

    [FromQuery(Name = "minYear")]
    public int? MinYear { get; init; }

    [FromQuery(Name = "maxYear")]
    public int? MaxYear { get; init; }

    [FromQuery(Name = "votingCountryCode")]
    public string? VotingCountryCode { get; init; }
}

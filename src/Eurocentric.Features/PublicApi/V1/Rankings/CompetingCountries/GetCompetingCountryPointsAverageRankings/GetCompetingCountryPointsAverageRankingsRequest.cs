using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsAverageRankings;

public sealed record GetCompetingCountryPointsAverageRankingsRequest : PaginatedRequest
{
    [FromQuery(Name = "contestStage")]
    [Description("Filters voting data by contest stage.")]
    [DefaultValue(typeof(QueryableContestStage), nameof(QueryableContestStage.Any))]
    public QueryableContestStage? ContestStage { get; init; }

    [FromQuery(Name = "minYear")]
    [Description("Filters voting data by inclusive minimum contest year.")]
    public int? MinYear { get; init; }

    [FromQuery(Name = "maxYear")]
    [Description("Filters voting data by inclusive maximum contest year.")]
    public int? MaxYear { get; init; }

    [FromQuery(Name = "votingCountryCode")]
    [RegularExpression("[A-Z]{2}", ErrorMessage = "Voting country code must be a string of 2 upper-case letters.")]
    [Description("Filters voting data by voting country code.")]
    public string? VotingCountryCode { get; init; }

    [FromQuery(Name = "votingMethod")]
    [Description("Filters voting data by voting method.")]
    [DefaultValue(typeof(QueryableVotingMethod), nameof(QueryableVotingMethod.Any))]
    public QueryableVotingMethod? VotingMethod { get; init; }
}

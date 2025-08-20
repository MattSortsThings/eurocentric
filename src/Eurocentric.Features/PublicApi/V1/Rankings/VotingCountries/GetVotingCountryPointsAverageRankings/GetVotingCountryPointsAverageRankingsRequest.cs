using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsAverageRankings;

public sealed record GetVotingCountryPointsAverageRankingsRequest : PaginatedRequest
{
    [Required]
    [FromQuery(Name = "competingCountryCode")]
    [RegularExpression("[A-Z]{2}", ErrorMessage = "Competing country code must be a string of 2 upper-case letters.")]
    [Description("Sets the competing country for the queried data.")]
    public required string CompetingCountryCode { get; init; }

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

    [FromQuery(Name = "votingMethod")]
    [Description("Filters the queried data to points awarded using a specific voting method.")]
    [DefaultValue(typeof(QueryableVotingMethod), nameof(QueryableVotingMethod.Any))]
    public QueryableVotingMethod? VotingMethod { get; init; }
}

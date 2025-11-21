using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Eurocentric.Apis.Public.V1.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Apis.Public.V1.Features.CompetingCountryRankings;

public sealed record GetCompetingCountryPointsAverageRankingsRequest
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

    [RegularExpression("^[A-Z]{2}$")]
    [FromQuery(Name = "votingCountryCode")]
    [Description("Filters voting data by voting country code.")]
    public string? VotingCountryCode { get; init; }

    [FromQuery(Name = "votingMethod")]
    [Description("Filters voting data by voting method.")]
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

    public bool Equals(GetCompetingCountryPointsAverageRankingsRequest? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return MinYear == other.MinYear
            && MaxYear == other.MaxYear
            && ContestStage == other.ContestStage
            && VotingCountryCode == other.VotingCountryCode
            && VotingMethod == other.VotingMethod
            && PageIndex == other.PageIndex
            && PageSize == other.PageSize
            && Descending == other.Descending;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            MinYear,
            MaxYear,
            ContestStage,
            VotingCountryCode,
            VotingMethod,
            PageIndex,
            PageSize,
            Descending
        );
}

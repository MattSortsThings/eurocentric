using System.ComponentModel;
using Eurocentric.Apis.Public.V0.Config;
using Eurocentric.Apis.Public.V0.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Apis.Public.V0.Features.Rankings.CompetingCountries;

public sealed record GetCompetingCountryPointsAverageRankingsRequest
{
    [FromQuery(Name = "minYear")]
    public int? MinYear { get; init; }

    [FromQuery(Name = "maxYear")]
    public int? MaxYear { get; init; }

    [FromQuery(Name = "contestStage")]
    public ContestStageFilter? ContestStage { get; init; }

    [FromQuery(Name = "votingCountryCode")]
    public string? VotingCountryCode { get; init; }

    [FromQuery(Name = "votingMethod")]
    public VotingMethodFilter? VotingMethod { get; init; }

    [FromQuery(Name = "pageIndex")]
    [DefaultValue(V0PaginationDefaults.PageIndex)]
    public int? PageIndex { get; init; }

    [FromQuery(Name = "pageSize")]
    [DefaultValue(V0PaginationDefaults.PageSize)]
    public int? PageSize { get; init; }

    [FromQuery(Name = "descending")]
    [DefaultValue(V0PaginationDefaults.Descending)]
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

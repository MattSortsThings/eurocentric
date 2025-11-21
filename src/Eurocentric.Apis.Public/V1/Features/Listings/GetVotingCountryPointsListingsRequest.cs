using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Eurocentric.Apis.Public.V1.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Apis.Public.V1.Features.Listings;

public sealed record GetVotingCountryPointsListingsRequest
{
    [Required]
    [FromQuery(Name = "contestYear")]
    [Description("Filters voting data by inclusive maximum contest year.")]
    public required int ContestYear { get; init; }

    [Required]
    [FromQuery(Name = "contestStage")]
    [Description("Filters voting data by contest stage.")]
    public required ContestStage ContestStage { get; init; }

    [Required]
    [RegularExpression("^[A-Z]{2}$")]
    [FromQuery(Name = "votingCountryCode")]
    [Description("Filters voting data by voting country code.")]
    public required string VotingCountryCode { get; init; }

    public bool Equals(GetVotingCountryPointsListingsRequest? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ContestYear == other.ContestYear
            && ContestStage == other.ContestStage
            && VotingCountryCode == other.VotingCountryCode;
    }

    public override int GetHashCode() => HashCode.Combine(ContestYear, (int)ContestStage, VotingCountryCode);
}

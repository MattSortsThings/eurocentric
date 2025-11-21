using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Apis.Public.V1.Features.Listings;

public sealed record GetCompetingCountryResultListingsRequest
{
    [Required]
    [RegularExpression("^[A-Z]{2}$")]
    [FromQuery(Name = "competingCountryCode")]
    [Description("Filters voting data by competing country code.")]
    public required string CompetingCountryCode { get; init; }

    public bool Equals(GetCompetingCountryResultListingsRequest? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return CompetingCountryCode == other.CompetingCountryCode;
    }

    public override int GetHashCode() => CompetingCountryCode.GetHashCode();
}

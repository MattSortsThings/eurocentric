using System.ComponentModel.DataAnnotations;
using Eurocentric.Apis.Public.V0.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Apis.Public.V0.Features.Listings;

public sealed record GetBroadcastResultListingsRequest
{
    [Required]
    [FromQuery(Name = "contestYear")]
    public required int ContestYear { get; init; }

    [Required]
    [FromQuery(Name = "contestStage")]
    public required ContestStage ContestStage { get; init; }

    public bool Equals(GetBroadcastResultListingsRequest? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ContestYear == other.ContestYear && ContestStage == other.ContestStage;
    }

    public override int GetHashCode() => HashCode.Combine(ContestYear, (int)ContestStage);
}

using Eurocentric.Apis.Public.V0.Config;
using Eurocentric.Apis.Public.V0.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V0.Dtos.Rankings.CompetingCountries;

public sealed record CompetingCountryPointsAverageMetadata
    : IDtoSchemaExampleProvider<CompetingCountryPointsAverageMetadata>
{
    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public ContestStageFilter? ContestStage { get; init; }

    public string? VotingCountryCode { get; init; }

    public VotingMethodFilter? VotingMethod { get; init; }

    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public bool Descending { get; init; }

    public int TotalItems { get; init; }

    public int TotalPages { get; init; }

    public static CompetingCountryPointsAverageMetadata CreateExample() =>
        new()
        {
            PageIndex = V0PaginationDefaults.PageIndex,
            PageSize = V0PaginationDefaults.PageSize,
            Descending = V0PaginationDefaults.Descending,
            TotalItems = 50,
            TotalPages = 5,
        };

    public bool Equals(CompetingCountryPointsAverageMetadata? other)
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
            && Descending == other.Descending
            && TotalItems == other.TotalItems
            && TotalPages == other.TotalPages;
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(MinYear);
        hashCode.Add(MaxYear);
        hashCode.Add(ContestStage);
        hashCode.Add(VotingCountryCode);
        hashCode.Add(VotingMethod);
        hashCode.Add(PageIndex);
        hashCode.Add(PageSize);
        hashCode.Add(Descending);
        hashCode.Add(TotalItems);
        hashCode.Add(TotalPages);

        return hashCode.ToHashCode();
    }
}

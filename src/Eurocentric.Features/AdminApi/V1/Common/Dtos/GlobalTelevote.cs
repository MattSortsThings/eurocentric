using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

/// <summary>
///     Represents a global televote in a contest.
/// </summary>
public sealed record GlobalTelevote : IExampleProvider<GlobalTelevote>
{
    /// <summary>
    ///     The ID of the participating country.
    /// </summary>
    public Guid ParticipatingCountryId { get; init; }

    public static GlobalTelevote CreateExample() => new() { ParticipatingCountryId = ExampleValues.CountryId2Of2 };
}

using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

/// <summary>
///     Represents a single country or pseudo-country.
/// </summary>
public sealed record Country : IExampleProvider<Country>
{
    /// <summary>
    ///     The country's ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     The IDs of all the contests in which the country is a participant.
    /// </summary>
    public Guid[] ParticipatingContestIds { get; init; } = [];

    public static Country CreateExample() => new()
    {
        Id = ExampleValues.CountryId1Of2,
        CountryCode = ExampleValues.CountryCode,
        CountryName = ExampleValues.CountryName,
        ParticipatingContestIds = [ExampleValues.ContestId]
    };
}

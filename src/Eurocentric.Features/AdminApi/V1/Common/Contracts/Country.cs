using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Contracts;

/// <summary>
///     Represents a country aggregate.
/// </summary>
public sealed record Country : IExampleProvider<Country>
{
    /// <summary>
    ///     The country's unique ID.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; }

    /// <summary>
    ///     The IDs of all the contests in which the country has a participant.
    /// </summary>
    public required Guid[] ParticipatingContestIds { get; init; }

    public static Country CreateExample() => new()
    {
        Id = ExampleIds.CountryAt,
        CountryCode = "AT",
        CountryName = "Austria",
        ParticipatingContestIds = [ExampleIds.Contest]
    };
}

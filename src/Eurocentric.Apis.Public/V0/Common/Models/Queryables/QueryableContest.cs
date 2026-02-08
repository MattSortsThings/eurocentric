using Eurocentric.Apis.Public.V0.Common.Enums;

namespace Eurocentric.Apis.Public.V0.Common.Models.Queryables;

public sealed record QueryableContest
{
    /// <summary>
    ///     The contest's year.
    /// </summary>
    public required int ContestYear { get; init; }

    /// <summary>
    ///     The UK English name of the contest's host city.
    /// </summary>
    public required string CityName { get; init; }

    /// <summary>
    ///     The contest's Semi-Final voting format.
    /// </summary>
    public required VotingFormat SemiFinalVotingFormat { get; init; }

    /// <summary>
    ///     The contest's Grand Final voting format.
    /// </summary>
    public required VotingFormat GrandFinalVotingFormat { get; init; }

    /// <summary>
    ///     The contest's optional global televote country code.
    /// </summary>
    public string? GlobalTelevoteCountryCode { get; init; }

    /// <summary>
    ///     An ordered array of the contest's participating country codes.
    /// </summary>
    public required string[] ParticipatingCountryCodes { get; init; }
}

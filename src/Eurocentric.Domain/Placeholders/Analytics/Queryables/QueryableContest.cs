using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders.Analytics.Queryables;

public sealed record QueryableContest
{
    /// <summary>
    ///     Gets or initializes the contest's year.
    /// </summary>
    public required int ContestYear { get; init; }

    /// <summary>
    ///     Gets or initializes the UK English name of the contest's host city.
    /// </summary>
    public required string CityName { get; init; }

    /// <summary>
    ///     Gets or initializes the contest's Semi-Final voting format.
    /// </summary>
    public required VotingFormat SemiFinalVotingFormat { get; init; }

    /// <summary>
    ///     Gets or initializes the contest's Grand Final voting format.
    /// </summary>
    public required VotingFormat GrandFinalVotingFormat { get; init; }

    /// <summary>
    ///     Gets or initializes the contest's optional global televote country code.
    /// </summary>
    public string? GlobalTelevoteCountryCode { get; init; }

    /// <summary>
    ///     Gets or initializes an ordered list of the contest's participating country codes.
    /// </summary>
    public required List<string> ParticipatingCountryCodes { get; init; }
}

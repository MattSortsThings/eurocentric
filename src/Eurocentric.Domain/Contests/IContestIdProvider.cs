using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Contests;

/// <summary>
///     Generates <see cref="ContestId" /> value objects.
/// </summary>
public interface IContestIdProvider
{
    /// <summary>
    ///     Creates and returns a new <see cref="ContestId" /> instance with a unique <see cref="ContestId.Value" />.
    /// </summary>
    /// <returns>A new <see cref="ContestId" /> instance.</returns>
    public ContestId Create();
}

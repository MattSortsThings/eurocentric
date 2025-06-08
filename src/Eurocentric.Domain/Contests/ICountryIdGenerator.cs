using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Contests;

/// <summary>
///     Generates a <see cref="ContestId" /> instance on demand.
/// </summary>
public interface IContestIdGenerator
{
    /// <summary>
    ///     Creates and returns a new <see cref="ContestId" /> instance with a random <see cref="ContestId.Value" />.
    /// </summary>
    /// <returns>A new <see cref="ContestId" /> instance.</returns>
    public ContestId Generate();
}

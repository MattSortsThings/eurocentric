using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Generates <see cref="ContestId" /> value objects on request.
/// </summary>
public interface IContestIdGenerator
{
    /// <summary>
    ///     Creates and returns a new <see cref="ContestId" /> value object.
    /// </summary>
    /// <returns>A new <see cref="ContestId" /> instance.</returns>
    public ContestId CreateSingle();
}

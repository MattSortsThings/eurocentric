using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

public interface IContestIdFactory
{
    /// <summary>
    ///     Creates and returns a new <see cref="ContestId" /> instance.
    /// </summary>
    /// <returns>A new <see cref="ContestId" /> instance.</returns>
    ContestId Create();
}

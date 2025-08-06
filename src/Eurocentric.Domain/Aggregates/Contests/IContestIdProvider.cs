using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Creates a <see cref="ContestId" /> instance on demand.
/// </summary>
public interface IContestIdProvider
{
    /// <summary>
    ///     Creates and returns a new <see cref="ContestId" /> instance with a <see cref="ContestId.Value" /> generated
    ///     according to RFC 9562, following the Version 7 format
    /// </summary>
    /// <returns>A new <see cref="ContestId" /> object.</returns>
    public ContestId CreateSingle();
}

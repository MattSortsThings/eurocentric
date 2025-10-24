using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     A country's role in a contest.
/// </summary>
public sealed class ContestRole : CompoundValueObject
{
    /// <summary>
    ///     Initializes a new <see cref="ContestRole" /> instance.
    /// </summary>
    /// <param name="contestId">The ID of the contest.</param>
    /// <param name="contestRoleType">The type of the contest role.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    public ContestRole(ContestId contestId, ContestRoleType contestRoleType)
    {
        ContestId = contestId ?? throw new ArgumentNullException(nameof(contestId));
        ContestRoleType = contestRoleType;
    }

    /// <summary>
    ///     Gets the ID of the contest.
    /// </summary>
    public ContestId ContestId { get; }

    /// <summary>
    ///     Gets the type of the contest role.
    /// </summary>
    public ContestRoleType ContestRoleType { get; }

    private protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ContestId;
        yield return ContestRoleType;
    }
}

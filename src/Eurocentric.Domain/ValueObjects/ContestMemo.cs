using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Summarizes a contest aggregate.
/// </summary>
public sealed class ContestMemo : ValueObject
{
    /// <summary>
    ///     Creates a new <see cref="ContestMemo" /> instance.
    /// </summary>
    /// <param name="contestId">Identifies the contest aggregate.</param>
    /// <param name="status">The current status of the contest aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    public ContestMemo(ContestId contestId, ContestStatus status)
    {
        ContestId = contestId ?? throw new ArgumentNullException(nameof(contestId));
        Status = status;
    }

    /// <summary>
    ///     Gets the ID of the contest aggregate.
    /// </summary>
    public ContestId ContestId { get; }

    /// <summary>
    ///     Gets the current status of the contest aggregate.
    /// </summary>
    public ContestStatus Status { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ContestId;
        yield return Status;
    }
}

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
    /// <param name="contestStatus">The current status of the contest aggregate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="contestId" /> is <see langword="null" />.</exception>
    public ContestMemo(ContestId contestId, ContestStatus contestStatus)
    {
        ContestId = contestId ?? throw new ArgumentNullException(nameof(contestId));
        ContestStatus = contestStatus;
    }

    /// <summary>
    ///     Gets the ID of the contest aggregate.
    /// </summary>
    public ContestId ContestId { get; }

    /// <summary>
    ///     Gets the current status of the contest aggregate.
    /// </summary>
    public ContestStatus ContestStatus { get; }

    /// <summary>
    ///     Creates and returns a new <see cref="ContestMemo" /> instance with the same <see cref="ContestMemo.ContestId" />
    ///     and a <see cref="ContestMemo.ContestStatus" /> value of <see cref="ContestStatus.InProgress" />.
    /// </summary>
    /// <returns>A new <see cref="ContestMemo" /> instance.</returns>
    public ContestMemo CloneAsInProgress() => new(ContestId, ContestStatus.InProgress);

    /// <summary>
    ///     Creates and returns a new <see cref="ContestMemo" /> instance with the same <see cref="ContestMemo.ContestId" />
    ///     and a <see cref="ContestMemo.ContestStatus" /> value of <see cref="ContestStatus.Completed" />.
    /// </summary>
    /// <returns>A new <see cref="ContestMemo" /> instance.</returns>
    public ContestMemo CloneAsCompleted() => new(ContestId, ContestStatus.Completed);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ContestId;
        yield return ContestStatus;
    }
}

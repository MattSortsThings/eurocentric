using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public sealed class ContestCollection
{
    private readonly List<Contest> _contests = new(1);

    /// <summary>
    ///     Adds the provided contest to the collection
    /// </summary>
    /// <param name="contest"></param>
    public void Add(Contest contest) => _contests.Add(contest);

    /// <summary>
    ///     Enumerates all the contests in the collection.
    /// </summary>
    /// <remarks>No assumptions should be made regarding the order of the contests returned by this method.</remarks>
    /// <returns>A sequence of contests.</returns>
    public IEnumerable<Contest> GetAll() => _contests;

    /// <summary>
    ///     Retrieves the single contest in the collection.
    /// </summary>
    /// <returns>A contest.</returns>
    public Contest GetSingle()
    {
        Assert.Single(_contests);

        return _contests[0];
    }

    /// <summary>
    ///     Removes the contest with the matching ID from the collection and replaces it with the provided contest.
    /// </summary>
    /// <param name="contest">The contest to be added.</param>
    public void Replace(Contest contest)
    {
        Assert.Contains(_contests, existingContest => existingContest.Id == contest.Id);
        _contests.RemoveAll(existingContest => existingContest.Id == contest.Id);
        _contests.Add(contest);
    }
}

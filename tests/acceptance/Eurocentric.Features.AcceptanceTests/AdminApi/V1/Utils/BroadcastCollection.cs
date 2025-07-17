using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public sealed class BroadcastCollection
{
    private readonly List<Broadcast> _broadcasts = new(1);

    /// <summary>
    ///     Adds the provided broadcast to the collection
    /// </summary>
    /// <param name="broadcast"></param>
    public void Add(Broadcast broadcast) => _broadcasts.Add(broadcast);

    /// <summary>
    ///     Enumerates all the broadcasts in the collection.
    /// </summary>
    /// <remarks>No assumptions should be made regarding the order of the broadcasts returned by this method.</remarks>
    /// <returns>A sequence of broadcasts.</returns>
    public IEnumerable<Broadcast> GetAll() => _broadcasts;

    /// <summary>
    ///     Retrieves the single broadcast in the collection.
    /// </summary>
    /// <returns>A broadcast.</returns>
    public Broadcast GetSingle()
    {
        Assert.Single(_broadcasts);

        return _broadcasts[0];
    }

    /// <summary>
    ///     Removes the broadcast with the matching ID from the collection and replaces it with the provided broadcast.
    /// </summary>
    /// <param name="broadcast">The broadcast to be added.</param>
    public void Replace(Broadcast broadcast)
    {
        Assert.Contains(_broadcasts, existingBroadcast => existingBroadcast.Id == broadcast.Id);
        _broadcasts.RemoveAll(existingBroadcast => existingBroadcast.Id == broadcast.Id);
        _broadcasts.Add(broadcast);
    }
}

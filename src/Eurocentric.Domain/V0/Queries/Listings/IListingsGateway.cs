namespace Eurocentric.Domain.V0.Queries.Listings;

public interface IListingsGateway
{
    Task<BroadcastResultListings> GetBroadcastResultListingsAsync(
        BroadcastResultQuery query,
        CancellationToken cancellationToken = default
    );
}

using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

/// <summary>
///     Stores contest stage and broadcast DTO key-value pairs.
/// </summary>
public sealed class BroadcastLookup
{
    private readonly Dictionary<ContestStage, Broadcast> _dictionary = new(3);

    /// <summary>
    ///     Adds the provided broadcast to this instance, overwriting any existing broadcast with the same contest stage.
    /// </summary>
    /// <param name="broadcast">The broadcast to be added.</param>
    public void AddOrReplace(Broadcast broadcast) => _dictionary[broadcast.ContestStage] = broadcast;

    /// <summary>
    ///     Retrieves the broadcast matching the provided contest stage value.
    /// </summary>
    /// <param name="contestStage">The contest stage.</param>
    /// <returns>A broadcast object.</returns>
    public Broadcast GetSingle(ContestStage contestStage) => _dictionary[contestStage];
}

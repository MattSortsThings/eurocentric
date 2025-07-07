namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

internal static class ActorMixins
{
    internal static async Task Given_the_system_is_populated_with_the_sample_queryable_data(this IActor actor)
    {
        await actor.BackDoor.ExecuteScopedAsync(BackDoorOperations.PopulateSampleQueryableCountriesAsync);
        await actor.BackDoor.ExecuteScopedAsync(BackDoorOperations.PopulateSampleQueryableJuryAwardsAsync);
        await actor.BackDoor.ExecuteScopedAsync(BackDoorOperations.PopulateSampleQueryableTelevoteAwardsAsync);
    }
}

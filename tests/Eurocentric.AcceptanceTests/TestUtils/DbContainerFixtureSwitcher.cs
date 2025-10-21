namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     A service that can pause and unpause a <see cref="DbContainerFixture" />.
/// </summary>
/// <param name="dbContainer">The fixture to be paused and unpaused.</param>
public sealed class DbContainerFixtureSwitcher(DbContainerFixture dbContainer)
{
    /// <summary>
    ///     Asynchronously pauses the fixture.
    /// </summary>
    public async Task PauseAsync() => await dbContainer.PauseAsync();

    /// <summary>
    ///     Asynchronously unpauses the fixture if it is currently paused.
    /// </summary>
    public async Task EnsureUnpausedAsync() => await dbContainer.EnsureUnpausedAsync();
}

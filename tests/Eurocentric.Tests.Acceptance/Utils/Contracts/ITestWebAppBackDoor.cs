namespace Eurocentric.Tests.Acceptance.Utils.Contracts;

public interface ITestWebAppBackDoor
{
    void ExecuteScoped(Action<IServiceProvider> func);

    Task ExecuteScopedAsync(Func<IServiceProvider, Task> func);

    Task<T> ExecuteScopedAsync<T>(Func<IServiceProvider, Task<T>> func);

    Task ResetAsync();
}

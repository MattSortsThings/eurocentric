using Eurocentric.WebApp;

await WebApplication.CreateBuilder(args)
    .ConfigureServices()
    .Build()
    .ConfigureMiddleware()
    .RunAsync();

using Eurocentric.Features;
using Eurocentric.WebApp;

WebApplication.CreateBuilder(args)
    .ConfigureServices()
    .Build()
    .ConfigureMiddleware()
    .Run();

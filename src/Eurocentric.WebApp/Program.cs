using Eurocentric.WebApp;

WebApplication.CreateBuilder(args)
    .ConfigureServices()
    .Build()
    .ConfigureRequestPipeline()
    .Run();

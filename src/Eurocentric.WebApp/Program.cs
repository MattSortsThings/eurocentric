using Eurocentric.WebApp.Startup;

WebApplication.CreateBuilder(args).ConfigureAllServices().Build().ConfigureHttpRequestPipeline().Run();

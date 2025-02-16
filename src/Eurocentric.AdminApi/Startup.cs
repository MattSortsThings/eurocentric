using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.AdminApi.V0.Calculations.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;

namespace Eurocentric.AdminApi;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the Admin API services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddAdminApiServices(this IServiceCollection services)
    {
        services.AddTransient<Action<MediatRServiceConfiguration>>(_ => configuration =>
            configuration.RegisterServicesFromAssemblyContaining(typeof(Startup)));

        services.AddTransient<Action<IEndpointRouteBuilder>>(_ => builder => builder.MapCreateCalculation());

        services.AddOpenApi("admin-api-v0.1",
            options =>
            {
                options.ShouldInclude = description => description.GroupName == "admin-api-v0.1";

                options.AddDocumentTransformer((document, _, _) =>
                {
                    document.Info.Title = "Eurocentric Public API";
                    document.Info.Version = "v0.1";
                    document.Info.Description = "A web API for (over)analysing the Eurovision Song Contest, 2016-present.";

                    return Task.CompletedTask;
                });

                options.AddSchemaTransformer((schema, context, _) =>
                {
                    if (context.JsonTypeInfo.Type == typeof(CreateCalculationCommand))
                    {
                        schema.Example = new OpenApiObject
                        {
                            ["x"] = new OpenApiInteger(10),
                            ["y"] = new OpenApiInteger(3),
                            ["operation"] = new OpenApiString(Operation.Modulus.ToString())
                        };
                    }

                    if (context.JsonTypeInfo.Type == typeof(CreateCalculationResult))
                    {
                        schema.Example = new OpenApiObject
                        {
                            ["calculation"] = new OpenApiObject
                            {
                                ["x"] = new OpenApiInteger(10),
                                ["y"] = new OpenApiInteger(3),
                                ["operation"] = new OpenApiString(Operation.Modulus.ToString()),
                                ["result"] = new OpenApiInteger(1),
                                ["dateRequested"] = new OpenApiDate(DateTime.UtcNow)
                            }
                        };
                    }

                    return Task.CompletedTask;
                });
            });

        return services;
    }
}

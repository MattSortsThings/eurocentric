using Eurocentric.PublicApi.V0.Greetings.GetGreetings;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Eurocentric.PublicApi;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the Public API services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddPublicApiServices(this IServiceCollection services)
    {
        services.AddTransient<Action<MediatRServiceConfiguration>>(_ => configuration =>
            configuration.RegisterServicesFromAssemblyContaining(typeof(Startup)));

        services.AddTransient<Action<IEndpointRouteBuilder>>(_ => builder => builder.MapGetGreetings());

        services.AddOpenApi("public-api-v0.1",
            options =>
            {
                options.ShouldInclude = description => description.GroupName == "public-api-v0.1";

                options.AddDocumentTransformer((document, _, _) =>
                {
                    document.Info.Title = "Eurocentric Public API";
                    document.Info.Version = "v0.1";
                    document.Info.Description = "A web API for (over)analysing the Eurovision Song Contest, 2016-present.";

                    return Task.CompletedTask;
                });

                options.AddOperationTransformer((operation, _, _) =>
                {
                    foreach (OpenApiParameter parameter in operation.Parameters)
                    {
                        parameter.Example = parameter.Name switch
                        {
                            "quantity" => new OpenApiInteger(2),
                            "language" => new OpenApiString("Swedish"),
                            "clientName" => new OpenApiString("Bjorn"),
                            _ => parameter.Example
                        };
                    }

                    return Task.CompletedTask;
                });

                options.AddSchemaTransformer((schema, context, _) =>
                {
                    if (context.JsonTypeInfo.Type == typeof(GetGreetingsResult))
                    {
                        schema.Example = new OpenApiObject
                        {
                            ["greetings"] = new OpenApiArray
                            {
                                new OpenApiString("Hej Bjorn!"),
                                new OpenApiString("Hej Bjorn!"),
                                new OpenApiString("Hej Bjorn!")
                            }
                        };
                    }

                    return Task.CompletedTask;
                });
            });

        return services;
    }
}

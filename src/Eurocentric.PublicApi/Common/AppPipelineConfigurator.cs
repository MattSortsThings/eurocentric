using Eurocentric.Shared.AppPipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.PublicApi.Common;

internal sealed class AppPipelineConfigurator : IAppPipelineConfigurator
{
    public void Configure(MediatRServiceConfiguration configuration) =>
        configuration.RegisterServicesFromAssemblyContaining<AppPipelineConfigurator>();
}

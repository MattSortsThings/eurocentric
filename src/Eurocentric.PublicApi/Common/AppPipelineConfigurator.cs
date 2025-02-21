using Eurocentric.Shared.AppPipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.PublicApi.Common;

internal sealed class AppPipelineConfigurator : IAppPipelineConfigurator
{
    public void Modify(MediatRServiceConfiguration configuration) =>
        configuration.RegisterServicesFromAssemblyContaining<AppPipelineConfigurator>();
}

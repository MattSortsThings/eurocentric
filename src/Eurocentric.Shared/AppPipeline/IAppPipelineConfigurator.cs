using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.AppPipeline;

/// <summary>
///     Defines a method for configuring <see cref="MediatR" /> app pipeline services.
/// </summary>
/// <remarks>Register an implementation of this interface as a transient service in each API project.</remarks>
public interface IAppPipelineConfigurator
{
    /// <summary>
    ///     Modifies the <see cref="MediatRServiceConfiguration" /> object.
    /// </summary>
    /// <param name="configuration">Contains <see cref="MediatR" /> service configuration properties.</param>
    public void Configure(MediatRServiceConfiguration configuration);
}

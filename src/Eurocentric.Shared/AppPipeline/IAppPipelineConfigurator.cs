using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.AppPipeline;

/// <summary>
///     Defines a method for modifying a <see cref="MediatR" /> app pipeline service configuration.
/// </summary>
/// <remarks>Register an implementation of this interface as a transient service in each API project.</remarks>
public interface IAppPipelineConfigurator
{
    /// <summary>
    ///     Modifies the specified <see cref="MediatR" /> app pipeline service configuration.
    /// </summary>
    /// <param name="configuration">Contains <see cref="MediatR" /> service configuration properties.</param>
    public void Modify(MediatRServiceConfiguration configuration);
}

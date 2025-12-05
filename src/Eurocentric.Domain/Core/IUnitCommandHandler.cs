using CSharpFunctionalExtensions;
using SlimMessageBus;

namespace Eurocentric.Domain.Core;

/// <summary>
///     Handles a command that <i>either</i> succeeds and returns no value <i>or</i> fails and returns an error.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
public interface IUnitCommandHandler<in TCommand> : IRequestHandler<TCommand, UnitResult<IError>>
    where TCommand : IUnitCommand;

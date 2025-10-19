using CSharpFunctionalExtensions;
using SlimMessageBus;

namespace Eurocentric.Domain.Functional;

/// <summary>
///     A handler for a command that <i>either</i> succeeds and returns no value <i>or</i> fails and returns an error.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
public interface IUnitCommandHandler<in TCommand> : IRequestHandler<TCommand, UnitResult<IDomainError>>
    where TCommand : IUnitCommand;

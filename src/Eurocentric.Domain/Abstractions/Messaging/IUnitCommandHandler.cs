using CSharpFunctionalExtensions;
using Eurocentric.Domain.Abstractions.Errors;
using SlimMessageBus;

namespace Eurocentric.Domain.Abstractions.Messaging;

/// <summary>
///     Handles a command that <i>either</i> succeeds and does not return a value <i>or</i> fails and returns an error.
/// </summary>
/// <typeparam name="TUnitCommand">The command type.</typeparam>
public interface IUnitCommandHandler<in TUnitCommand> : IRequestHandler<TUnitCommand, UnitResult<IDomainError>>
    where TUnitCommand : IUnitCommand;

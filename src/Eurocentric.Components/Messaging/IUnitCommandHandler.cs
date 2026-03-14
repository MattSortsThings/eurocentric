using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     Handles an internal unit command that <i>either</i> succeeds, changes the system state, and returns no value,
///     <i>or</i> fails and returns a <see cref="DomainError" /> object.
/// </summary>
/// <typeparam name="TUnitCommand">The unit command type.</typeparam>
public interface IUnitCommandHandler<in TUnitCommand> : IRequestHandler<TUnitCommand, UnitResult<DomainError>>
    where TUnitCommand : class, IUnitCommand;

using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     Handler for an internal unit command that <i>either</i> succeeds and does not return a value <i>or</i> fails
///     and returns a domain error object.
/// </summary>
/// <typeparam name="TUnitCommand">The unit command type.</typeparam>
public interface IUnitCommandHandler<in TUnitCommand> : IRequestHandler<TUnitCommand, UnitResult<DomainError>>;

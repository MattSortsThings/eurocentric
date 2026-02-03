using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     An internal unit command that <i>either</i> successfully changes the system state and does not return a value
///     <i>or</i> fails and returns a domain error.
/// </summary>
public interface IUnitCommand : IRequest<UnitResult<DomainError>>;

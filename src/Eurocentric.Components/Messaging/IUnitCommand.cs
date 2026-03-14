using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     An internal unit command that <i>either</i> succeeds, changes the system state, and returns no value,
///     <i>or</i> fails and returns a <see cref="DomainError" /> object.
/// </summary>
public interface IUnitCommand : IRequest<UnitResult<DomainError>>;

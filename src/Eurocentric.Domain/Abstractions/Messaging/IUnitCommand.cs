using CSharpFunctionalExtensions;
using Eurocentric.Domain.Abstractions.Errors;
using SlimMessageBus;

namespace Eurocentric.Domain.Abstractions.Messaging;

/// <summary>
///     A command that <i>either</i> succeeds and does not return a value <i>or</i> fails and returns an error.
/// </summary>
public interface IUnitCommand : IRequest<UnitResult<IDomainError>>;

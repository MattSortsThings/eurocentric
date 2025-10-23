using CSharpFunctionalExtensions;
using SlimMessageBus;

namespace Eurocentric.Domain.Core;

/// <summary>
///     A command that <i>either</i> succeeds and returns no value <i>or</i> fails and returns an error.
/// </summary>
public interface IUnitCommand : IRequest<UnitResult<IDomainError>>;

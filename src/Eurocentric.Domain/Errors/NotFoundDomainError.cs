using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Errors;

/// <summary>
///     A domain error that occurs when the request operates on a non-existent aggregate.
/// </summary>
public sealed record NotFoundDomainError : DomainError
{
    public override required DomainErrorType Type { get; init; } = DomainErrorType.NotFound;
}

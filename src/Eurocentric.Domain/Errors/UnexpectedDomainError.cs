using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Errors;

/// <summary>
///     A domain error that occurs due to a bug or another unexpected situation.
/// </summary>
public sealed record UnexpectedDomainError : DomainError
{
    public override required DomainErrorType Type { get; init; } = DomainErrorType.Unexpected;
}

using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Errors;

/// <summary>
///     A domain error that occurs when the request in itself violates one or more domain invariants, irrespective
///     of the current state of any aggregates.
/// </summary>
public sealed record IntrinsicDomainError : DomainError
{
    public override required DomainErrorType Type { get; init; } = DomainErrorType.Intrinsic;
}

using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Errors;

/// <summary>
///     A domain error that occurs when the request violates one or more domain invariants given the current state
///     of one or more aggregates.
/// </summary>
public sealed record ExtrinsicDomainError : DomainError
{
    public override required DomainErrorType Type { get; init; } = DomainErrorType.Extrinsic;
}

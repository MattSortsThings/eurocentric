namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies a domain error's type.
/// </summary>
public enum DomainErrorType
{
    /// <summary>
    ///     A domain error that occurs due to a bug or another unexpected situation.
    /// </summary>
    Unexpected,

    /// <summary>
    ///     A domain error that occurs when the request operates on a non-existent aggregate.
    /// </summary>
    NotFound,

    /// <summary>
    ///     A domain error that occurs when the request violates one or more domain invariants given the current state
    ///     of one or more aggregates.
    /// </summary>
    Extrinsic,

    /// <summary>
    ///     A domain error that occurs when the request in itself violates one or more domain invariants, irrespective
    ///     of the current state of any aggregates.
    /// </summary>
    Intrinsic,
}

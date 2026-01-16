namespace Eurocentric.Domain.Errors;

/// <summary>
///     Specifies a domain error's type.
/// </summary>
public enum DomainErrorType
{
    /// <summary>
    ///     The "Unexpected" domain error type.
    /// </summary>
    /// <remarks>This error type only ever occurs in the event of a source code bug or system failure.</remarks>
    Unexpected,

    /// <summary>
    ///     The "Not Found" domain error type.
    /// </summary>
    /// <remarks>This error type occurs when a request operates on a domain aggregate that does not exist.</remarks>
    NotFound,

    /// <summary>
    ///     The "Extrinsic" domain error type.
    /// </summary>
    /// <remarks>
    ///     This error type occurs when a request violates one or more domain invariants given the current state of all
    ///     existing aggregates.
    /// </remarks>
    Extrinsic,

    /// <summary>
    ///     The "Intrinsic" domain error type.
    /// </summary>
    /// <remarks>
    ///     This error type occurs when a request in itself violates one or more domain invariants, irrespective of any
    ///     existing aggregates.
    /// </remarks>
    Intrinsic,
}

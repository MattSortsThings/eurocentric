namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies a domain error's type.
/// </summary>
public enum DomainErrorType
{
    /// <summary>
    ///     Denotes an undefined domain error.
    /// </summary>
    Undefined,

    /// <summary>
    ///     Denotes a domain error that occurs when the request operates on an aggregate that does not exist.
    /// </summary>
    NotFound,

    /// <summary>
    ///     Denotes a domain error that occurs when the request, if completed, would violate a domain invariant
    ///     given the current state of all existing aggregates.
    /// </summary>
    Extrinsic,

    /// <summary>
    ///     Denotes a domain error that occurs when the request, if completed, would violate a domain invariant
    ///     irrespective of the current state of any aggregates.
    /// </summary>
    Intrinsic,
}

namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies a domain error's type.
/// </summary>
public enum DomainErrorType
{
    /// <summary>
    ///     The error was unexpected.
    /// </summary>
    Unexpected,

    /// <summary>
    ///     The error arose because the requested aggregate was not found.
    /// </summary>
    NotFound,

    /// <summary>
    ///     The error arose because the request was impossible given the current state of all existing aggregates.
    /// </summary>
    Conflict,

    /// <summary>
    ///     The error arose because the request was illegal, irrespective of the current state of all existing
    ///     aggregates.
    /// </summary>
    IllegalRequest,
}

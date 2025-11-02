using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Domain errors that may occur when working with <see cref="Country" /> aggregates.
/// </summary>
public static class CountryErrors
{
    /// <summary>
    ///     Creates and returns a new error indicating that the client has requested a <see cref="Country" /> that does not
    ///     exist in the system.
    /// </summary>
    /// <param name="countryId">The ID of the requested country.</param>
    /// <returns>A new <see cref="NotFoundError" /> instance.</returns>
    public static NotFoundError CountryNotFound(CountryId countryId)
    {
        return new NotFoundError
        {
            Title = "Country not found",
            Detail = "The requested country does not exist.",
            Extensions = new Dictionary<string, object?> { { nameof(countryId), countryId.Value } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to delete a <see cref="Country" />
    ///     that has one or more contest roles.
    /// </summary>
    /// <param name="countryId">The ID of the requested country.</param>
    /// <returns>A new <see cref="ConflictError" /> instance.</returns>
    public static ConflictError CountryDeletionNotPermitted(CountryId countryId)
    {
        return new ConflictError
        {
            Title = "Country deletion not permitted",
            Detail = "The requested country has one or more contest roles.",
            Extensions = new Dictionary<string, object?> { { nameof(countryId), countryId.Value } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has tried to create a <see cref="Country" /> with a
    ///     non-unique <see cref="Country.CountryCode" />.
    /// </summary>
    /// <param name="countryCode">The country code.</param>
    /// <returns>A new <see cref="ConflictError" /> instance.</returns>
    public static ConflictError CountryCodeConflict(CountryCode countryCode)
    {
        return new ConflictError
        {
            Title = "Country code conflict",
            Detail = "A country already exists with the provided country code.",
            Extensions = new Dictionary<string, object?> { { nameof(countryCode), countryCode.Value } },
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="Country" /> without
    ///     setting its <see cref="Country.CountryCode" /> property.
    /// </summary>
    /// <returns>A new <see cref="UnexpectedError" /> instance.</returns>
    public static UnexpectedError CountryCodePropertyNotSet()
    {
        return new UnexpectedError
        {
            Title = "CountryCode property not set",
            Detail = "Client attempted to create a Country aggregate without setting its CountryCode property.",
            Extensions = null,
        };
    }

    /// <summary>
    ///     Creates and returns a new error indicating that the client has attempted to create a <see cref="Country" />
    ///     without setting its <see cref="Country.CountryName" /> property.
    /// </summary>
    /// <returns>A new <see cref="UnexpectedError" /> instance.</returns>
    public static UnexpectedError CountryNamePropertyNotSet()
    {
        return new UnexpectedError
        {
            Title = "CountryName property not set",
            Detail = "Client attempted to create a Country aggregate without setting its CountryName property.",
            Extensions = null,
        };
    }
}

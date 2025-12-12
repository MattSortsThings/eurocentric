using Eurocentric.Domain.Abstractions.Errors;

namespace Eurocentric.Domain.Aggregates.V0;

public static class CountryErrors
{
    public static NotFoundError CountryNotFound(Guid countryId) => new CountryNotFoundError(countryId);

    public static ConflictError CountryDeletionConflict(Guid countryId) => new CountryDeletionConflictError(countryId);

    public static ConflictError CountryCodeConflict(string countryCode) => new CountryCodeConflictError(countryCode);

    public static IllegalRequestError IllegalCountryCodeValue(string countryCode) =>
        new IllegalCountryCodeValueError(countryCode);

    private sealed record CountryNotFoundError : NotFoundError
    {
        public CountryNotFoundError(Guid countryId)
        {
            Extensions = new Dictionary<string, object?> { { nameof(countryId), countryId } };
        }

        public override string Title => "Country not found";

        public override string Detail => "The requested country does not exist.";

        public override IReadOnlyDictionary<string, object?>? Extensions { get; }
    }

    private sealed record CountryDeletionConflictError : ConflictError
    {
        public CountryDeletionConflictError(Guid countryId)
        {
            Extensions = new Dictionary<string, object?> { { nameof(countryId), countryId } };
        }

        public override string Title => "Country deletion conflict";

        public override string Detail =>
            "The requested country cannot be deleted, because it has one or more contest roles.";

        public override IReadOnlyDictionary<string, object?>? Extensions { get; }
    }

    private sealed record CountryCodeConflictError : ConflictError
    {
        public CountryCodeConflictError(string countryCode)
        {
            Extensions = new Dictionary<string, object?> { { nameof(countryCode), countryCode } };
        }

        public override string Title => "Country code conflict";

        public override string Detail => "A country exists with the provided country code.";

        public override IReadOnlyDictionary<string, object?>? Extensions { get; }
    }

    private sealed record IllegalCountryCodeValueError : IllegalRequestError
    {
        public IllegalCountryCodeValueError(string countryCode)
        {
            Extensions = new Dictionary<string, object?> { { nameof(countryCode), countryCode } };
        }

        public override string Title => "Illegal country code value";

        public override string Detail => "Country code value must be a string of 2 upper-case letters.";

        public override IReadOnlyDictionary<string, object?>? Extensions { get; }
    }
}

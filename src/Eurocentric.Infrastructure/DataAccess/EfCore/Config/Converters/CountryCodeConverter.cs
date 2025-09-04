using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.Converters;

internal sealed class CountryCodeConverter : ValueConverter<CountryCode, string>
{
    public CountryCodeConverter() : base(src => src.Value, value => CountryCode.FromValue(value).Value) { }
}

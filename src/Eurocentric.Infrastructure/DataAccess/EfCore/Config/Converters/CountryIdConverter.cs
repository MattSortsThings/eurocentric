using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.Converters;

internal sealed class CountryIdConverter : ValueConverter<CountryId, Guid>
{
    public CountryIdConverter() : base(src => src.Value, value => CountryId.FromValue(value)) { }
}

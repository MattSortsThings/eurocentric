using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.Converters;

internal sealed class CityNameConverter : ValueConverter<CityName, string>
{
    public CityNameConverter() : base(src => src.Value, value => CityName.FromValue(value).Value) { }
}

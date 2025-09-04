using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.Converters;

internal sealed class CountryNameConverter : ValueConverter<CountryName, string>
{
    public CountryNameConverter() : base(src => src.Value, value => CountryName.FromValue(value).Value) { }
}

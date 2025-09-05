using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.Converters;

internal sealed class ActNameConverter : ValueConverter<ActName, string>
{
    public ActNameConverter() : base(src => src.Value, value => ActName.FromValue(value).Value) { }
}

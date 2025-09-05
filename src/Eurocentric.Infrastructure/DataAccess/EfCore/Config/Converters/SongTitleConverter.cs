using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.Converters;

internal sealed class SongTitleConverter : ValueConverter<SongTitle, string>
{
    public SongTitleConverter() : base(src => src.Value, value => SongTitle.FromValue(value).Value) { }
}

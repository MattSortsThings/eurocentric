using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Config.Converters;

internal sealed class BroadcastIdConverter : ValueConverter<BroadcastId, Guid>
{
    public BroadcastIdConverter() : base(src => src.Value, value => BroadcastId.FromValue(value)) { }
}

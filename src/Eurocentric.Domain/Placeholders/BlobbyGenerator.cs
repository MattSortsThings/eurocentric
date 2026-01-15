namespace Eurocentric.Domain.Placeholders;

public static class BlobbyGenerator
{
    public static List<string> Generate(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

        return Enumerable.Repeat("Blobby", count).ToList();
    }
}

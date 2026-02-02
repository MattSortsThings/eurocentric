namespace Eurocentric.Domain.Placeholders;

public static class BlobbyGenerator
{
    public static string[] Generate(int count)
    {
        return count > 0
            ? Enumerable.Repeat("Blobby", count).ToArray()
            : throw new ArgumentOutOfRangeException(nameof(count));
    }
}

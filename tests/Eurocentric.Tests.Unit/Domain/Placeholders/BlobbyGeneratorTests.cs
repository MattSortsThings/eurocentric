using Eurocentric.Domain.Placeholders;
using Eurocentric.Tests.Unit.Utils;

namespace Eurocentric.Tests.Unit.Domain.Placeholders;

public static class BlobbyGeneratorTests
{
    [Category("placeholder")]
    public sealed class Generate : UnitTests
    {
        [Test]
        [MethodDataSource(nameof(HappyPathTestCases))]
        public async Task Should_generate_requested_Blobby_strings(int count)
        {
            // Act
            string[] result = BlobbyGenerator.Generate(count);

            // Assert
            await Assert.That(result).Count().IsEqualTo(count).And.ContainsOnly(s => s == "Blobby");
        }

        public static IEnumerable<int> HappyPathTestCases() => Enumerable.Range(1, 100);
    }
}

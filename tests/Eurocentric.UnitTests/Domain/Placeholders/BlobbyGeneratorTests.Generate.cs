using Eurocentric.Domain.Placeholders;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.Placeholders;

public static class BlobbyGeneratorTests
{
    [Category("placeholder")]
    public sealed class Generate : UnitTestBase
    {
        [Test]
        [MethodDataSource(nameof(HappyPathTestCases))]
        public async Task Should_generate_requested_quantity_identical_blobby_strings(int count)
        {
            // Act
            List<string> results = BlobbyGenerator.Generate(count);

            // Assert
            await Assert.That(results).Count().IsEqualTo(count).And.ContainsOnly(v => v == "Blobby");
        }

        public static IEnumerable<int> HappyPathTestCases() => Enumerable.Range(1, 100);
    }
}

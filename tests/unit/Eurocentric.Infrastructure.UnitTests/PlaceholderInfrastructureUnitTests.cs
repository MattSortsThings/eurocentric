using Eurocentric.Infrastructure.DataAccess.Dapper;
using Eurocentric.Infrastructure.UnitTests.TestUtils;

namespace Eurocentric.Infrastructure.UnitTests;

[Category("placeholder")]
public sealed class PlaceholderInfrastructureUnitTests : UnitTest
{
    [Test]
    public async Task Should_be_able_to_instantiate_DbStoredProcedureRunner_class_using_factory_method()
    {
        // Arrange
        const string dummyConnectionString = "DUMMY_CONNECTION_STRING";

        // Act
        DbStoredProcedureRunner result = DbStoredProcedureRunner.Create(dummyConnectionString);

        // Assert
        await Assert.That(result).IsNotNull()
            .And.IsAssignableTo<IDbStoredProcedureRunner>();
    }
}

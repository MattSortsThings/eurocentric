using ArchUnitNET.TUnit;

namespace Eurocentric.Tests.Architecture.Assemblies.Apis.Any;

public abstract partial class AnyApiAssemblyTests
{
    public sealed class FeatureContractClasses : AnyApiAssemblyTests
    {
        [Test]
        public void Should_have_name_ending_QueryParams_or_RequestBody_or_ResponseBody()
        {
            Classes()
                .That()
                .Are(FeatureContractClassesInAnyApiAssembly)
                .Should()
                .HaveNameEndingWith("QueryParams")
                .OrShould()
                .HaveNameEndingWith("RequestBody")
                .OrShould()
                .HaveNameEndingWith("ResponseBody")
                .Check(ArchitectureUnderTest);
        }

        [Test]
        public void Should_be_public()
        {
            Classes()
                .That()
                .Are(FeatureContractClassesInAnyApiAssembly)
                .Should()
                .BePublic()
                .Check(ArchitectureUnderTest);
        }

        [Test]
        public void Should_be_immutable_records()
        {
            Classes()
                .That()
                .Are(FeatureContractClassesInAnyApiAssembly)
                .Should()
                .BeImmutable()
                .AndShould()
                .BeRecord()
                .Check(ArchitectureUnderTest);
        }
    }
}

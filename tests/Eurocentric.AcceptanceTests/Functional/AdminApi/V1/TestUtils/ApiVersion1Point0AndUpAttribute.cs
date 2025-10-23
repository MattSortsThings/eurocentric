namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public sealed class ApiVersion1Point0AndUpAttribute : DataSourceGeneratorAttribute<string>
{
    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => "v1.0";
    }
}

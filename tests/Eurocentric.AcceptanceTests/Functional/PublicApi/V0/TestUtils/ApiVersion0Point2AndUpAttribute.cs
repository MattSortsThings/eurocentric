namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils;

public sealed class ApiVersion0Point2AndUpAttribute : DataSourceGeneratorAttribute<string>
{
    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => "v0.2";
    }
}

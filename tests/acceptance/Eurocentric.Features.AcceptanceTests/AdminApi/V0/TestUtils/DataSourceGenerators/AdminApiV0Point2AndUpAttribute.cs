namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils.DataSourceGenerators;

public sealed class AdminApiV0Point2AndUpAttribute : DataSourceGeneratorAttribute<string>
{
    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => "v0.2";
    }
}

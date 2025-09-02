namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils.DataSourceGenerators;

public sealed class PublicApiV0Point2AndUpAttribute : DataSourceGeneratorAttribute<string>
{
    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => "v0.2";
    }
}

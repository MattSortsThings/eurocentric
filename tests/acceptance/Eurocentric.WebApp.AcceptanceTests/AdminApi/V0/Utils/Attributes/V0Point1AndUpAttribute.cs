namespace Eurocentric.WebApp.AcceptanceTests.AdminApi.V0.Utils.Attributes;

public sealed class V0Point1AndUpAttribute : DataSourceGeneratorAttribute<string>
{
    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => "v0.1";
        yield return () => "v0.2";
    }
}

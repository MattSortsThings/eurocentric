namespace Eurocentric.WebApp.AcceptanceTests.AdminApi.V0.Utils.Attributes;

public sealed class V0Point2AndUpAttribute : DataSourceGeneratorAttribute<string>
{
    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => "v0.2";
    }
}

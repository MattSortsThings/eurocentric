namespace Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Utils;

public sealed class V0Point1UpwardAttribute : DataSourceGeneratorAttribute<string>
{
    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => "v0.1";
        yield return () => "v0.2";
    }
}

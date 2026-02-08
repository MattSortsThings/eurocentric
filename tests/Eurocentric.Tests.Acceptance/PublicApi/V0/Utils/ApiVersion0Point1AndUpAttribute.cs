namespace Eurocentric.Tests.Acceptance.PublicApi.V0.Utils;

public sealed class ApiVersion0Point1AndUpAttribute : DataSourceGeneratorAttribute<string>
{
    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => "v0.1";
    }
}

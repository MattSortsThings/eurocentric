namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Attributes;

public class ApiVersion1Point0AndUpAttribute : DataSourceGeneratorAttribute<string>
{
    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => "v1.0";
    }
}

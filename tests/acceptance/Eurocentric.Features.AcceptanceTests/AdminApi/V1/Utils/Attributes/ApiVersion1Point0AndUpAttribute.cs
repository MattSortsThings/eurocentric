namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;

public class ApiVersion1Point0AndUpAttribute : DataSourceGeneratorAttribute<string>
{
    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => "v1.0";
    }
}

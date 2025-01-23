using Eurocentric.Shared.ApiModules;

namespace Eurocentric.PublicApi;

public class PublicApiModule : ApiModule
{
    public override string ApiName => "public-api";

    public override string Prefix => "public/api";
}

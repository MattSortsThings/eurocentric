using Eurocentric.Tests.Acceptance.Utils.Contracts;

namespace Eurocentric.Tests.Acceptance.PublicApi.V0.Utils;

public abstract partial class EuroFan
{
    public static IApiVersionSetter Testing(ITestWebApp testWebApp) => new Builder(testWebApp);

    public interface IApiVersionSetter
    {
        IApiKeySetter UsingApiVersion(string apiVersion);
    }

    public interface IApiKeySetter
    {
        IResponseSetter UsingDemoApiKey();
    }

    public interface IResponseSetter
    {
        IEuroFanExpecting200OKBuilder<TBody> Expecting200OK<TBody>()
            where TBody : class;
    }

    public interface IEuroFanExpecting200OKBuilder<TBody>
        where TBody : class
    {
        TEuroFan Build<TEuroFan>(Func<IEuroFanKernel, TEuroFan> factory)
            where TEuroFan : EuroFanExpecting200OK<TBody>;
    }

    private sealed class Builder(ITestWebApp testWebApp) : IApiVersionSetter, IApiKeySetter, IResponseSetter
    {
        private string _apiKey = string.Empty;
        private string _apiVersion = string.Empty;

        public IResponseSetter UsingDemoApiKey()
        {
            _apiKey = "TEST_DEMO_API_KEY";

            return this;
        }

        public IApiKeySetter UsingApiVersion(string apiVersion)
        {
            _apiVersion = apiVersion;

            return this;
        }

        public IEuroFanExpecting200OKBuilder<TBody> Expecting200OK<TBody>()
            where TBody : class
        {
            RestRequestFactory requestFactory = new(_apiVersion, _apiKey);
            EuroFanKernel kernel = new(testWebApp, requestFactory);

            return new EuroFanExpecting200OKBuilder<TBody>(kernel);
        }
    }

    private sealed class EuroFanExpecting200OKBuilder<TBody>(IEuroFanKernel kernel)
        : IEuroFanExpecting200OKBuilder<TBody>
        where TBody : class
    {
        public TEuroFan Build<TEuroFan>(Func<IEuroFanKernel, TEuroFan> factory)
            where TEuroFan : EuroFanExpecting200OK<TBody> => factory(kernel);
    }
}

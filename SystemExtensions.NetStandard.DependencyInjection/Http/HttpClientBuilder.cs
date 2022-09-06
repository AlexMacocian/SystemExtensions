using Slim;
using System.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;

namespace System.DependencyInjection.Http;

public sealed class HttpClientBuilder<T>
{
    private readonly IServiceProducer serviceProducer;

    private Uri baseAddress;
    private HttpMessageHandler httpMessageHandler;
    private bool disposeMessageHandler;
    private Action<HttpRequestHeaders> defaultRequestHeadersSetup;
    private long maxResponseBufferSize = 2147483647L; //2GB default HttpClient value [https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.maxresponsecontentbuffersize?view=net-6.0]
    private TimeSpan timeout = TimeSpan.FromSeconds(100); //100 seconds default HttpClient value [https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.timeout?view=net-6.0]

    internal HttpClientBuilder(IServiceProducer serviceProducer)
    {
        serviceProducer.ThrowIfNull(nameof(serviceProducer));

        this.serviceProducer = serviceProducer;
    }

    public HttpClientBuilder<T> WithMessageHandler(HttpMessageHandler httpMessageHandler)
    {
        httpMessageHandler.ThrowIfNull(nameof(httpMessageHandler));

        this.httpMessageHandler = httpMessageHandler;
        return this;
    }

    public HttpClientBuilder<T> WithDisposeMessageHandler(bool disposeMessageHandler)
    {
        this.disposeMessageHandler = disposeMessageHandler;
        return this;
    }

    public HttpClientBuilder<T> WithBaseAddress(Uri baseAddress)
    {
        baseAddress.ThrowIfNull(nameof(baseAddress));

        this.baseAddress = baseAddress;
        return this;
    }

    public HttpClientBuilder<T> WithDefaultRequestHeadersSetup(Action<HttpRequestHeaders> setup)
    {
        setup.ThrowIfNull(nameof(setup));

        this.defaultRequestHeadersSetup = setup;
        return this;
    }

    public HttpClientBuilder<T> WithMaxResponseBufferSize(long responseBufferSize)
    {
        this.maxResponseBufferSize = responseBufferSize;
        return this;
    }

    public HttpClientBuilder<T> WithTimeout(TimeSpan timeout)
    {
        this.timeout = timeout;
        return this;
    }

    public IServiceProducer Build()
    {
        this.serviceProducer.RegisterScoped<IHttpClient<T>>(_ =>
        {
            var client = this.httpMessageHandler is not null ?
                new HttpClient<T>(this.httpMessageHandler, this.disposeMessageHandler) :
                new HttpClient<T>();

            client.BaseAddress = this.baseAddress;
            this.defaultRequestHeadersSetup?.Invoke(client.DefaultRequestHeaders);
            client.MaxResponseContentBufferSize = this.maxResponseBufferSize;
            client.Timeout = this.timeout;
            return client;
        });

        return this.serviceProducer;
    }
}

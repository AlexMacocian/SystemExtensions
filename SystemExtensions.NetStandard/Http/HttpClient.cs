using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public sealed class HttpClient<Tscope> : IHttpClient<Tscope>, IDisposable
{
    private readonly HttpClient httpClient;
    private readonly Type scope;
    private EventHandler<HttpClientEventMessage> eventEmitted;

    public event EventHandler<HttpClientEventMessage> EventEmitted
    {
        add
        {
            this.eventEmitted += value;
        }
        remove
        {
            this.eventEmitted -= value;
        }
    }
    public Uri BaseAddress { get => this.httpClient.BaseAddress; set => this.httpClient.BaseAddress = value; }
    public HttpRequestHeaders DefaultRequestHeaders => this.httpClient.DefaultRequestHeaders;
    public long MaxResponseContentBufferSize { get => this.httpClient.MaxResponseContentBufferSize; set => this.httpClient.MaxResponseContentBufferSize = value; }
    public TimeSpan Timeout { get => this.httpClient.Timeout; set => this.httpClient.Timeout = value; }

    public HttpClient()
    {
        this.httpClient = new HttpClient();
        this.scope = typeof(Tscope);
    }

    public HttpClient(
        HttpMessageHandler handler)
    {
        this.httpClient = new HttpClient(handler);
        this.scope = typeof(Tscope);
    }

    public HttpClient(
        HttpMessageHandler handler,
        bool disposeHandler)
    {
        this.httpClient = new HttpClient(handler, disposeHandler);
        this.scope = typeof(Tscope);
    }

    public void CancelPendingRequests()
    {
        this.LogInformation(string.Empty, "Canceling request");
        this.httpClient.CancelPendingRequests();
    }
    public Task<HttpResponseMessage> DeleteAsync(string requestUri)
    {
        this.LogInformation(requestUri, nameof(DeleteAsync));
        return this.httpClient.DeleteAsync(requestUri);
    }
    public Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
    {
        this.LogInformation(requestUri, nameof(DeleteAsync));
        return this.httpClient.DeleteAsync(requestUri, cancellationToken);
    }
    public Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
    {
        this.LogInformation(requestUri.ToString(), nameof(DeleteAsync));
        return this.httpClient.DeleteAsync(requestUri);
    }
    public Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
    {
        this.LogInformation(requestUri.ToString(), nameof(DeleteAsync));
        return this.httpClient.DeleteAsync(requestUri, cancellationToken);
    }
    public Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        this.LogInformation(requestUri, nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri);
    }
    public Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption)
    {
        this.LogInformation(requestUri, nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri, completionOption);
    }
    public Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
    {
        this.LogInformation(requestUri, nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri, completionOption, cancellationToken);
    }
    public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
    {
        this.LogInformation(requestUri, nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri, cancellationToken);
    }
    public Task<HttpResponseMessage> GetAsync(Uri requestUri)
    {
        this.LogInformation(requestUri.ToString(), nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri);
    }
    public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)
    {
        this.LogInformation(requestUri.ToString(), nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri, completionOption);
    }
    public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
    {
        this.LogInformation(requestUri.ToString(), nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri, completionOption, cancellationToken);
    }
    public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
    {
        this.LogInformation(requestUri.ToString(), nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri, cancellationToken);
    }
    public Task<byte[]> GetByteArrayAsync(string requestUri)
    {
        this.LogInformation(requestUri, nameof(GetByteArrayAsync));
        return this.httpClient.GetByteArrayAsync(requestUri);
    }
    public Task<byte[]> GetByteArrayAsync(Uri requestUri)
    {
        this.LogInformation(requestUri.ToString(), nameof(GetByteArrayAsync));
        return this.httpClient.GetByteArrayAsync(requestUri);
    }
    public Task<Stream> GetStreamAsync(string requestUri)
    {
        this.LogInformation(requestUri, nameof(GetStreamAsync));
        return this.httpClient.GetStreamAsync(requestUri);
    }
    public Task<Stream> GetStreamAsync(Uri requestUri)
    {
        this.LogInformation(requestUri.ToString(), nameof(GetStreamAsync));
        return this.httpClient.GetStreamAsync(requestUri);
    }
    public Task<string> GetStringAsync(string requestUri)
    {
        this.LogInformation(requestUri, nameof(GetStringAsync));
        return this.httpClient.GetStringAsync(requestUri);
    }
    public Task<string> GetStringAsync(Uri requestUri)
    {
        this.LogInformation(requestUri.ToString(), nameof(GetStringAsync));
        return this.httpClient.GetStringAsync(requestUri);
    }
    public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
    {
        this.LogInformation(requestUri, nameof(PostAsync));
        return this.httpClient.PostAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
    {
        this.LogInformation(requestUri, nameof(PostAsync));
        return this.httpClient.PostAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
    {
        this.LogInformation(requestUri.ToString(), nameof(PostAsync));
        return this.httpClient.PostAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
    {
        this.LogInformation(requestUri.ToString(), nameof(PostAsync));
        return this.httpClient.PostAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
    {
        this.LogInformation(requestUri, nameof(PutAsync));
        return this.httpClient.PutAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
    {
        this.LogInformation(requestUri, nameof(PutAsync));
        return this.httpClient.PutAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
    {
        this.LogInformation(requestUri.ToString(), nameof(PutAsync));
        return this.httpClient.PutAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
    {
        this.LogInformation(requestUri.ToString(), nameof(PutAsync));
        return this.httpClient.PutAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        this.LogInformation(request.RequestUri.ToString(), nameof(SendAsync));
        return this.httpClient.SendAsync(request);
    }
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
    {
        this.LogInformation(request.RequestUri.ToString(), nameof(SendAsync));
        return this.httpClient.SendAsync(request, completionOption);
    }
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
    {
        this.LogInformation(request.RequestUri.ToString(), nameof(SendAsync));
        return this.httpClient.SendAsync(request, completionOption, cancellationToken);
    }
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        this.LogInformation(request.RequestUri.ToString(), nameof(SendAsync));
        return this.httpClient.SendAsync(request, cancellationToken);
    }
    public void Dispose()
    {
        this.httpClient.Dispose();
    }

    private void LogInformation(string url, string method)
    {
        this.eventEmitted?.Invoke(this, new HttpClientEventMessage(this.scope, method, url));
    }
}

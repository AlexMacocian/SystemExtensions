using System.Extensions;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public sealed class HttpClient<Tscope> : IHttpClient<Tscope>, IDisposable
{
    private readonly bool disposeInnerClient = true;
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
#pragma warning disable CS8601 // Possible null reference assignment.
            this.eventEmitted -= value;
#pragma warning restore CS8601 // Possible null reference assignment.
        }
    }
    public Uri BaseAddress { get => this.httpClient.BaseAddress; set => this.httpClient.BaseAddress = value; }
    public HttpRequestHeaders DefaultRequestHeaders => this.httpClient.DefaultRequestHeaders;
    public long MaxResponseContentBufferSize { get => this.httpClient.MaxResponseContentBufferSize; set => this.httpClient.MaxResponseContentBufferSize = value; }
    public TimeSpan Timeout { get => this.httpClient.Timeout; set => this.httpClient.Timeout = value; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public HttpClient(bool disposeInnerClient = true)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        this.disposeInnerClient = disposeInnerClient;
        this.httpClient = new HttpClient();
        this.scope = typeof(Tscope);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public HttpClient(
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        HttpMessageHandler handler,
        bool disposeInnerClient = true)
    {
        this.disposeInnerClient = disposeInnerClient;
        this.httpClient = new HttpClient(handler);
        this.scope = typeof(Tscope);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public HttpClient(
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        HttpMessageHandler handler,
        bool disposeHandler,
        bool disposeInnerClient = true)
    {
        this.disposeInnerClient = disposeInnerClient;
        this.httpClient = new HttpClient(handler, disposeHandler);
        this.scope = typeof(Tscope);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public HttpClient(
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        HttpClient httpClient,
        bool disposeInnerClient = true)
    {
        this.disposeInnerClient = disposeInnerClient;
        this.httpClient = httpClient.ThrowIfNull(nameof(httpClient));
        this.scope = typeof(Tscope);
    }

    public void CancelPendingRequests()
    {
        this.LogDebug(string.Empty, "Canceling request");
        this.httpClient.CancelPendingRequests();
    }
    public Task<HttpResponseMessage> DeleteAsync(string requestUri)
    {
        this.LogDebug(requestUri, nameof(DeleteAsync));
        return this.httpClient.DeleteAsync(requestUri);
    }
    public Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
    {
        this.LogDebug(requestUri, nameof(DeleteAsync));
        return this.httpClient.DeleteAsync(requestUri, cancellationToken);
    }
    public Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
    {
        this.LogDebug(requestUri.ToString(), nameof(DeleteAsync));
        return this.httpClient.DeleteAsync(requestUri);
    }
    public Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
    {
        this.LogDebug(requestUri.ToString(), nameof(DeleteAsync));
        return this.httpClient.DeleteAsync(requestUri, cancellationToken);
    }
    public Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        this.LogDebug(requestUri, nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri);
    }
    public Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption)
    {
        this.LogDebug(requestUri, nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri, completionOption);
    }
    public Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
    {
        this.LogDebug(requestUri, nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri, completionOption, cancellationToken);
    }
    public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
    {
        this.LogDebug(requestUri, nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri, cancellationToken);
    }
    public Task<HttpResponseMessage> GetAsync(Uri requestUri)
    {
        this.LogDebug(requestUri.ToString(), nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri);
    }
    public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)
    {
        this.LogDebug(requestUri.ToString(), nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri, completionOption);
    }
    public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
    {
        this.LogDebug(requestUri.ToString(), nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri, completionOption, cancellationToken);
    }
    public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
    {
        this.LogDebug(requestUri.ToString(), nameof(GetAsync));
        return this.httpClient.GetAsync(requestUri, cancellationToken);
    }
    public Task<byte[]> GetByteArrayAsync(string requestUri)
    {
        this.LogDebug(requestUri, nameof(GetByteArrayAsync));
        return this.httpClient.GetByteArrayAsync(requestUri);
    }
    public Task<byte[]> GetByteArrayAsync(Uri requestUri)
    {
        this.LogDebug(requestUri.ToString(), nameof(GetByteArrayAsync));
        return this.httpClient.GetByteArrayAsync(requestUri);
    }
    public Task<Stream> GetStreamAsync(string requestUri)
    {
        this.LogDebug(requestUri, nameof(GetStreamAsync));
        return this.httpClient.GetStreamAsync(requestUri);
    }
    public Task<Stream> GetStreamAsync(Uri requestUri)
    {
        this.LogDebug(requestUri.ToString(), nameof(GetStreamAsync));
        return this.httpClient.GetStreamAsync(requestUri);
    }
    public Task<string> GetStringAsync(string requestUri)
    {
        this.LogDebug(requestUri, nameof(GetStringAsync));
        return this.httpClient.GetStringAsync(requestUri);
    }
    public Task<string> GetStringAsync(Uri requestUri)
    {
        this.LogDebug(requestUri.ToString(), nameof(GetStringAsync));
        return this.httpClient.GetStringAsync(requestUri);
    }
    public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
    {
        this.LogDebug(requestUri, nameof(PostAsync));
        return this.httpClient.PostAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
    {
        this.LogDebug(requestUri, nameof(PostAsync));
        return this.httpClient.PostAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
    {
        this.LogDebug(requestUri.ToString(), nameof(PostAsync));
        return this.httpClient.PostAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
    {
        this.LogDebug(requestUri.ToString(), nameof(PostAsync));
        return this.httpClient.PostAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
    {
        this.LogDebug(requestUri, nameof(PutAsync));
        return this.httpClient.PutAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
    {
        this.LogDebug(requestUri, nameof(PutAsync));
        return this.httpClient.PutAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
    {
        this.LogDebug(requestUri.ToString(), nameof(PutAsync));
        return this.httpClient.PutAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
    {
        this.LogDebug(requestUri.ToString(), nameof(PutAsync));
        return this.httpClient.PutAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        this.LogDebug(request.RequestUri.ToString(), nameof(SendAsync));
        return this.httpClient.SendAsync(request);
    }
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
    {
        this.LogDebug(request.RequestUri.ToString(), nameof(SendAsync));
        return this.httpClient.SendAsync(request, completionOption);
    }
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
    {
        this.LogDebug(request.RequestUri.ToString(), nameof(SendAsync));
        return this.httpClient.SendAsync(request, completionOption, cancellationToken);
    }
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        this.LogDebug(request.RequestUri.ToString(), nameof(SendAsync));
        return this.httpClient.SendAsync(request, cancellationToken);
    }
    public void Dispose()
    {
        if (this.disposeInnerClient)
        {
            this.httpClient.Dispose();
        }
    }

    private void LogDebug(string url, string method)
    {
        this.eventEmitted?.Invoke(this, new HttpClientEventMessage(this.scope, method, url));
    }
}

namespace System.Net.Http;

public sealed class HttpClientEventMessage
{
    public Type Scope { get; }
    public string Method { get; }
    public string Url { get; }

    internal HttpClientEventMessage(Type scope, string method, string url)
    {
        this.Scope = scope;
        this.Method = method;
        this.Url = url;
    }
}

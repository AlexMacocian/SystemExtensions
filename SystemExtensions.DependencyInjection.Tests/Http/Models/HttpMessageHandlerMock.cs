using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SystemExtensions.NetStandard.DependencyInjection.Tests.Http.Models;
public sealed class HttpMessageHandlerMock : HttpMessageHandler
{
    public bool Called { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        this.Called = true;

        return Task.FromResult(new HttpResponseMessage());
    }
}

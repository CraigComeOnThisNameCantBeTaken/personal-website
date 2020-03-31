using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GitIntegration.Tests.Fixtures
{
    public class FakeMessageHandler : HttpMessageHandler
    {
        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            throw new NotImplementedException("Mock was not setup correctly");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }
}

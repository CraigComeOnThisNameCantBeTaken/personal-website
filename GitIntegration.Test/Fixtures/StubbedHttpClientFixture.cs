﻿using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Moq;
using Newtonsoft.Json;

namespace GitIntegration.Tests.Fixtures
{
    public class StubbedHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient client;

        public StubbedHttpClientFactory(HttpClient client)
        {
            this.client = client;
        }

        public HttpClient CreateClient(string name)
        {
            return client;
        }
    }

    public class StubbedHttpClientFixture
    {
        private Mock<FakeMessageHandler> mockedHttpMessageHandler;
        public HttpClient HttpClient;
        public readonly IHttpClientFactory httpClientFactory;

        public StubbedHttpClientFixture()
        {
            mockedHttpMessageHandler = new Mock<FakeMessageHandler>() { CallBase = true };
            HttpClient = new HttpClient(mockedHttpMessageHandler.Object);
            httpClientFactory = new StubbedHttpClientFactory(HttpClient);
        }

        public StubbedHttpClientFixture WithResponse(string uri, object response, HttpMethod verb)
        {
            mockedHttpMessageHandler
                .Setup(f =>
                    f.Send(It.Is<HttpRequestMessage>(m =>
                            m.RequestUri.ToString() == uri &&
                            m.Method == verb
                        )
                    )
                )
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(response))
                });

            return this;
        }

        public StubbedHttpClientFixture WithResponse(string uri, HttpStatusCode statusCode)
        {

            mockedHttpMessageHandler
                .Setup(f =>
                    f.Send(It.Is<HttpRequestMessage>(m => m.RequestUri.ToString() == uri))
                )
                .Returns(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(string.Empty)
                });

            return this;
        }
    }
}

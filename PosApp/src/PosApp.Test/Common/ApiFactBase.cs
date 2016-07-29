using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using PosApp.Test.DomainFixtures;
using Xunit.Abstractions;

namespace PosApp.Test.Common
{
    public class ApiFactBase : IDisposable
    {
        readonly HttpConfiguration m_httpConfiguration;
        readonly HttpServer m_httpServer;
        readonly List<HttpClient> m_httpClients = new List<HttpClient>();
        readonly OutputRedirector m_outputRedirector;

        protected Fixtures Fixtures { get; }

        public ApiFactBase(ITestOutputHelper outputHelper)
        {
            DatabaseHelper.ResetDatabase();
            m_outputRedirector = new OutputRedirector(outputHelper);
            m_httpConfiguration = new HttpConfiguration();
            var bootstrap = new Bootstrap();
            bootstrap.Initialize(m_httpConfiguration);
            m_httpServer = new HttpServer(m_httpConfiguration);
            Fixtures = new Fixtures(bootstrap.CreateLifetimeScope());
        }

        protected HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient(m_httpServer)
            {
                BaseAddress = new Uri("http://www.gandong.com")
            };

            m_httpClients.Add(httpClient);
            return httpClient;
        }

        public void Dispose()
        {
            m_httpClients.ForEach(c => c.Dispose());
            m_httpServer.Dispose();
            m_httpConfiguration.Dispose();
            m_outputRedirector.Dispose();
        }
    }
}
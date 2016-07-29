using System;
using Autofac;
using PosApp.Test.DomainFixtures;
using Xunit.Abstractions;

namespace PosApp.Test.Common
{
    public class FactBase : IDisposable
    {
        readonly IContainer m_container;
        readonly ILifetimeScope m_testScope;
        readonly OutputRedirector m_outputRedirector;

        protected Fixtures Fixtures { get; }

        public FactBase(ITestOutputHelper outputHelper)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new PosAppModule());
            m_container = containerBuilder.Build();
            m_testScope = m_container.BeginLifetimeScope();

            DatabaseHelper.ResetDatabase();
            m_outputRedirector = new OutputRedirector(outputHelper);

            Fixtures = new Fixtures(m_container.BeginLifetimeScope());
        }

        protected ILifetimeScope GetScope()
        {
            return m_testScope;
        }

        public void Dispose()
        {
            m_outputRedirector.Dispose();
            m_container.Dispose();
        }
    }
}
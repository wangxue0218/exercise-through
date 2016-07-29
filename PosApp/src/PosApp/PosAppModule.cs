using System.Configuration;
using System.Reflection;
using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using PosApp.Repositories;
using PosApp.Services;
using Module = Autofac.Module;

namespace PosApp
{
    public class PosAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PosService>().InstancePerLifetimeScope();
            builder.RegisterType<PromotionService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductRespository>().As<IProductRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DiscountProductRespository>().InstancePerLifetimeScope();
            builder.Register(CreateSessionFactory).InstancePerLifetimeScope();
            builder.Register(CreateSession).InstancePerLifetimeScope();
        }

        static ISession CreateSession(IComponentContext container)
        {
            var sessionFactory = container.Resolve<ISessionFactory>();
            return sessionFactory.OpenSession();
        }

        static ISessionFactory CreateSessionFactory(IComponentContext container)
        {
            MsSqlConfiguration databaseConfig = MsSqlConfiguration.MsSql2008
                .ConnectionString(ConfigurationManager.ConnectionStrings["Default"].ConnectionString)
                .ShowSql()
                .FormatSql();
            return Fluently
                .Configure()
                .Database(databaseConfig)
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildSessionFactory();
        }
    }
}
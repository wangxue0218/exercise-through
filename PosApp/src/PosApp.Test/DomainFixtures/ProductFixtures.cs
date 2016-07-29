using Autofac;
using NHibernate.Util;
using PosApp.Domain;
using PosApp.Repositories;

namespace PosApp.Test.DomainFixtures
{
    public class ProductFixtures
    {
        readonly ILifetimeScope m_scope;

        public ProductFixtures(ILifetimeScope scope)
        {
            m_scope = scope;
        }

        public void Create(params Product[] product)
        {
            var productRepository = m_scope.Resolve<IProductRepository>();
            product.ForEach(p => productRepository.Save(p));
        }
    }
}
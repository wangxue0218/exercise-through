using Autofac;
using NHibernate.Util;
using PosApp.Domain;
using PosApp.Repositories;

namespace PosApp.Test.DomainFixtures
{
    public class PromotionFixtures
    {
        readonly ILifetimeScope m_scope;

        public PromotionFixtures(ILifetimeScope scope)
        {
            m_scope = scope;
        }

        public void Create(params Promotion[] promotions)
        {
            var discountProductRespository = m_scope.Resolve<PromotionRespository>();
            promotions.ForEach(p => discountProductRespository.Save(p));
        }
    }
}
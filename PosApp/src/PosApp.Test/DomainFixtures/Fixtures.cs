using Autofac;

namespace PosApp.Test.DomainFixtures
{
    public class Fixtures
    {
        public Fixtures(ILifetimeScope scope)
        {
            Products = new ProductFixtures(scope);
            Promotions = new PromotionFixtures(scope);
        }

        public ProductFixtures Products { get; }
        public PromotionFixtures Promotions { get;}
    }
}
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using PosApp.Domain;

namespace PosApp.Repositories
{
    public class ProductRespository : IProductRepository
    {
        readonly ISession m_session;

        public ProductRespository(ISession session)
        {
            m_session = session;
        }

        public IList<Product> GetByBarcodes(params string[] barcodes)
        {
            return m_session.Query<Product>()
                .Where(p => barcodes.Contains(p.Barcode))
                .ToArray();
        }

        public int CountByBarcodes(IList<string> barcodes)
        {
            return m_session.Query<Product>()
                .Count(p => barcodes.Contains(p.Barcode));
        }

        public void Save(Product product)
        {
            m_session.Save(product);
            m_session.Flush();
        }
    }
}
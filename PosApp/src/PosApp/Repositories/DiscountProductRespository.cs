using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using PosApp.Domain;

namespace PosApp.Repositories
{
    public class DiscountProductRespository
    {
        readonly ISession m_session;

        public DiscountProductRespository(ISession session)
        {
            m_session = session;
        }

        public List<Promotion> GetPromotionsByBarcode(string barcodes)
        {
            return m_session.Query<Promotion>()
                .Where(p => barcodes.Contains(p.Barcode))
                .ToList();
        } 
        public List<Promotion> GetBarcodesByType(string type)
        {
            return m_session.Query<Promotion>().Where(p => p.Type == type)
                .ToList();
        }

        public List<Promotion> GetByBarcode(string[] barcodes)
        {
            return m_session.Query<Promotion>()
                .Where(p => barcodes.Contains(p.Barcode))
                .ToList();
        } 

        public void Save(List<Promotion> promotions)
        {
            promotions.ForEach(p => m_session.Save(p));
            m_session.Flush();
        }

        public void Save(Promotion promotion)
        {
            m_session.Save(promotion);
            m_session.Flush();
        }
        public void Delete(List<Promotion> promotions)
        {
            promotions.ForEach(p => m_session.Delete(p));
            m_session.Flush();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using NHibernate.Util;
using PosApp.Domain;
using PosApp.Repositories;

namespace PosApp.Services
{
    public class PromotionService
    {
        readonly DiscountProductRespository m_discountProductRespository;
        readonly IProductRepository m_productRespository;

        public PromotionService(DiscountProductRespository discountProductRespository,
            IProductRepository productRespository)
        {
            m_discountProductRespository = discountProductRespository;
            m_productRespository = productRespository;
        }

        public List<string> GetBarcodes(string type)
        {
            return m_discountProductRespository.GetBarcodesByType(type)
                .Select(p => p.Barcode).ToList();
        } 
        public IList<PromotionItem> BuildPromotionItems(IList<ReceiptItem> receiptItems)
        {
            List<Promotion> promotions =
                m_discountProductRespository.GetBarcodesByType("BUY_TWO_GET_ONE");
            return receiptItems.Where(r => promotions.Any(p => p.Barcode == r.Product.Barcode))
                .Select(pi => new PromotionItem(pi.Product, pi.Amount / 3)).ToArray();
        }
        public void AddBarcode(string type,string[]barcodes)
        {
            Validate(type, barcodes);
            List<string> typeBarcodes = GetBarcodes(type);
            barcodes.Where(b => typeBarcodes.Contains(b)==false)
                .Select(nb => new Promotion {Type = type,Barcode = nb})
                .ForEach(f => m_discountProductRespository.Save(f));
        }

        public void DeleteBarcode(string type, string[] barcodes)
        {
            List<Promotion> promotions = m_discountProductRespository.GetByBarcode(barcodes)
                .Where(p => p.Type == type)
                .ToList();
            m_discountProductRespository.Delete(promotions);
        }

        void Validate(string type, string[] barcodes)
        {
//            if(barcodes.Length == 0)
//                throw new ArgumentNullException();
            string[] uniqueBarcodes = barcodes.Distinct().ToArray();
            if (m_productRespository.CountByBarcodes(uniqueBarcodes) != uniqueBarcodes.Length)
            {
                throw new ArgumentException("Some of the products cannot be found.");
            }
        }
    }
}
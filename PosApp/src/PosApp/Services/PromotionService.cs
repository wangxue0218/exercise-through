using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using PosApp.Domain;
using PosApp.Repositories;

namespace PosApp.Services
{
    public class PromotionService
    {
        readonly DiscountProductRespository m_promotoionRepository;
        readonly IProductRepository m_productRespository;

        public PromotionService(DiscountProductRespository promotoionRepository,
            IProductRepository productRespository)
        {
            m_promotoionRepository = promotoionRepository;
            m_productRespository = productRespository;
        }

        public List<string> GetBarcodes(string type)
        {
            return m_promotoionRepository.GetBarcodesByType(type)
                .Select(p => p.Barcode).ToList();
        } 
       
        public void BuyTwoGetOne(Receipt receipt)
        {
            List<Promotion> promotions =
                m_promotoionRepository.GetBarcodesByType("BUY_TWO_GET_ONE");
            receipt.ReceiptItems.Where(r => promotions.Any(p => p.Barcode == r.Product.Barcode))
                .ForEach(r => r.Promoted = r.Product.Price * (r.Amount /3));
        } 
        public void AddBarcode(string type,string[]barcodes)
        {
            Validate(barcodes);
            List<string> typeBarcodes = GetBarcodes(type);
            List<Promotion> promotions = barcodes.Where(b => typeBarcodes.Contains(b) == false)
                .Select(nb => new Promotion {Type = type, Barcode = nb}).ToList();
            m_promotoionRepository.Save(promotions);
        }

        public void DeleteBarcode(string type, string[] barcodes)
        {
            List<Promotion> promotions = m_promotoionRepository.GetByBarcode(barcodes)
                .Where(p => p.Type == type)
                .ToList();
            m_promotoionRepository.Delete(promotions);
        }

        void Validate(string[] barcodes)
        {
            string[] uniqueBarcodes = barcodes.Distinct().ToArray();
            if (m_productRespository.CountByBarcodes(uniqueBarcodes) != uniqueBarcodes.Length)
            {
                throw new ArgumentException("Some of the products cannot be found.");
            }
        }

        public Receipt BuildPromotion(Receipt receipt)
        {
            return m_promotoionRepository.GetAllTypes()
                .Select(type => CreatePromotion(type))
                .Aggregate(receipt,(r, promotion) => promotion.GetPromotedReceipt(r, promotion);


//            BuyTwoGetOne(receipt);
//            List<string> barcodes = GetBarcodes("BUY_HUNDRED_CUT_FIFTY");
//            decimal total_hundred_free_fifty = receipt.ReceiptItems.Where(
//                r => barcodes.Contains(r.Product.Barcode))
//                .Sum(p => p.Total - p.Promoted);
//            receipt.Promoted = receipt.ReceiptItems.Sum(p => p.Promoted)
//                               + (total_hundred_free_fifty/100)*50;
//            receipt.Total -= receipt.Promoted;
//            return receipt;
        }
    }
}
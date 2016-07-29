using System;
using System.Collections.Generic;
using System.Linq;
using PosApp.Domain;
using PosApp.Repositories;

namespace PosApp.Services
{
    public class PosService
    {
        readonly IProductRepository m_productRepository;
        readonly PromotionService m_promotionService;
        public PosService(IProductRepository productRepository,PromotionService promotionService)
        {
            m_productRepository = productRepository;
            m_promotionService = promotionService;
        }

        public Receipt GetReceipt(IList<BoughtProduct> boughtProducts)
        {
            Validate(boughtProducts);
            IList<ReceiptItem> receiptItems = MergeReceiptItems(boughtProducts);
            Receipt receipt = new Receipt(receiptItems);
            return m_promotionService.BuildPromotion(receipt);
        }
        IList<ReceiptItem> MergeReceiptItems(IList<BoughtProduct> boughtProducts)
        {
            string[] barcodes = boughtProducts.Select(bp => bp.Barcode).Distinct().ToArray();
            Dictionary<string, Product> boughtProductSet = m_productRepository
                .GetByBarcodes(barcodes)
                .ToDictionary(p => p.Barcode, p => p);
            return boughtProducts
                .GroupBy(bp => bp.Barcode)
                .Select(g => new ReceiptItem(boughtProductSet[g.Key], g.Sum(bp => bp.Amount)))
                .ToArray();
        }
        void Validate(IList<BoughtProduct> boughtProducts)
        {
            if (boughtProducts == null) { throw new ArgumentNullException(nameof(boughtProducts)); }
            if (boughtProducts.Any(bp => bp.Amount <= 0))
            {
                throw new ArgumentException(nameof(boughtProducts));
            }

            string[] uniqueBarcodes = boughtProducts.Select(bp => bp.Barcode).Distinct().ToArray();
            if (m_productRepository.CountByBarcodes(uniqueBarcodes) != uniqueBarcodes.Length)
            {
                throw new ArgumentException("Some of the products cannot be found.");
            }
        }
    }
}
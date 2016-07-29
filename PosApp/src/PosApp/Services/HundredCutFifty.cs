using System.Collections.Generic;
using System.Linq;
using PosApp.Domain;
using PosApp.Repositories;

namespace PosApp.Services
{
    class HundredCutFifty : ICaculatePromotions
    {
        readonly PromotionRespository m_promotoionRepository;

        public HundredCutFifty(PromotionRespository m_promotoionRepository)
        {
            this.m_promotoionRepository = m_promotoionRepository;
        }

        public Receipt GetPromotionReceipt(Receipt receipt)
        {
            List<string> barcodes = m_promotoionRepository.GetBarcodesByType("BUY_HUNDRED_CUT_FIFTY")
                .Select(p => p.Barcode).ToList();
            decimal total_hundred_free_fifty = receipt.ReceiptItems.Where(
                r => barcodes.Contains(r.Product.Barcode))
                .Sum(p => p.Total - p.Promoted);
            receipt.Promoted += (total_hundred_free_fifty/100)*50;
            receipt.Total -= (total_hundred_free_fifty / 100) * 50;
            return receipt;

        }
    }
}
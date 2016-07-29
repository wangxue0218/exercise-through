using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using PosApp.Domain;
using PosApp.Repositories;

namespace PosApp.Services
{
    class BuyTwoGetOne : ICaculatePromotions
    {
        readonly PromotionRespository m_promotionRespository;
        public BuyTwoGetOne(PromotionRespository promotoionRepository)
        {
            m_promotionRespository = promotoionRepository;
        }

        public Receipt GetPromotionReceipt(Receipt receipt)
        {
            List<Promotion> promotions =
                m_promotionRespository.GetBarcodesByType("BUY_TWO_GET_ONE");
            receipt.ReceiptItems.Where(r => promotions.Any(p => p.Barcode == r.Product.Barcode))
                .ForEach(r => r.Promoted = r.Product.Price * (r.Amount / 3));
            receipt.Promoted = receipt.ReceiptItems.Sum(r => r.Promoted);
            receipt.Total -= receipt.Promoted;
            return receipt;
        }
    }
}
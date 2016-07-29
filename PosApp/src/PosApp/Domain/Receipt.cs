using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PosApp.Domain
{
    public class Receipt
    {
        public Receipt(IList<ReceiptItem> receiptItems,IList<PromotionItem> promotionItems)
        {
            PromotionItems = new ReadOnlyCollection<PromotionItem>(promotionItems);
            ReceiptItems = new ReadOnlyCollection<ReceiptItem>(receiptItems);
            Promoted = promotionItems.Sum(p => p.promoted);
            Total = receiptItems.Sum(r => r.Total) - Promoted;
        }
        public IList<ReceiptItem> ReceiptItems { get; }
        public IList<PromotionItem> PromotionItems { get; }
        public decimal Total { get; }
        public decimal Promoted { get; }
    }
}
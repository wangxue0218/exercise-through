using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PosApp.Domain
{
    public class Receipt
    {
        public Receipt(IList<ReceiptItem> receiptItems)
        {
            ReceiptItems = receiptItems;
            Promoted = receiptItems.Sum(r => r.Promoted);
            Total = receiptItems.Sum(r => r.Total);
        }

        public IList<ReceiptItem> ReceiptItems { get; set; }
//        public IList<PromotionItem> PromotionItems { get; set; }
        public decimal Total { get; set; }
        public decimal Promoted { get; set; }
    }
}
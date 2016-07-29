namespace PosApp.Domain
{
    public class ReceiptItem
    {
        public ReceiptItem(Product product, int amount )
        {
            Product = product;
            Amount = amount;
            Promoted = 0;
            Total = product.Price * amount;
        }

        public Product Product { get; }
        public int Amount { get; }
        public decimal Promoted { get; set; }
        public decimal Total { get; }
    }
}
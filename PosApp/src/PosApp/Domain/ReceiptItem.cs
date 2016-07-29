namespace PosApp.Domain
{
    public class ReceiptItem
    {
        public ReceiptItem(Product product, int amount ,int promotedAmount = 0)
        {
            Product = product;
            Amount = amount;
            Promoted = promotedAmount*product.Price;
            Total = product.Price * amount;
        }

        public Product Product { get; }
        public int Amount { get; }
        public decimal Promoted { get; }
        public decimal Total { get; }
    }
}
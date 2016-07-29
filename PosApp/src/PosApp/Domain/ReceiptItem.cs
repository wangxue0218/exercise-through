namespace PosApp.Domain
{
    public class ReceiptItem
    {
        public ReceiptItem(Product product, int amount)
        {
            Product = product;
            Amount = amount;
            Total = product.Price * amount;
        }

        public Product Product { get; }
        public int Amount { get; }
        public decimal Total { get; }
    }
}
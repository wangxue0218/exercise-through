namespace PosApp.Domain
{
    public class PromotionItem
    {

        public PromotionItem(Product product, int amount)
        {
            Product = product;
            Amount = amount;
            promoted = Amount*Product.Price;

        }

        public Product Product { get; set; }
        public int Amount { get; set; }
        public decimal promoted { get; set; }
    }
}
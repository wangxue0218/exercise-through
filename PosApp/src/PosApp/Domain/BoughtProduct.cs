namespace PosApp.Domain
{
    public class BoughtProduct
    {
        public string Barcode { get; }
        public int Amount { get; }

        public BoughtProduct(string barcode, int amount)
        {
            Barcode = barcode;
            Amount = amount;
        }
    }
}
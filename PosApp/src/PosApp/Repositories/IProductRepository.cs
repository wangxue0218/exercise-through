using System.Collections.Generic;
using PosApp.Domain;

namespace PosApp.Repositories
{
    public interface IProductRepository
    {
        IList<Product> GetByBarcodes(params string[] barcodes);
        int CountByBarcodes(IList<string> barcodes);
        void Save(Product product);
    }
}
using System;
using FluentNHibernate.Mapping;

namespace PosApp.Domain
{
    public class Product
    {
        public virtual Guid Id { get; set; }
        public virtual string Barcode { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string Name { get; set; }
    }

    class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Table("products");
            Id(p => p.Id);
            Map(p => p.Barcode);
            Map(p => p.Price);
            Map(p => p.Name);
        }
    }
}
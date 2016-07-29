using FluentMigrator;

namespace Pos.Migration
{
    [Migration(1)]
    public class CreateProductTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("products")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("barcode").AsString(32)
                .WithColumn("price").AsDecimal();

            Create.Index("idx_products_barcode")
                .OnTable("products")
                .OnColumn("barcode")
                .Unique();
        }

        public override void Down()
        {
            Delete.Table("products");
        }
    }
}

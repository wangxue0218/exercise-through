using FluentMigrator;

namespace Pos.Migration
{
    [Migration(2)]
    public class AddNameToProductTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            Alter.Table("products")
                .AddColumn("name")
                .AsString(64)
                .NotNullable()
                .WithDefaultValue("");
        }

        public override void Down()
        {
            Delete.Column("name")
                .FromTable("products");
        }
    }
}
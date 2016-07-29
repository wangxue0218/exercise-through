using FluentMigrator;

namespace Pos.Migration
{
    [Migration(3)]
    public class CreatePromotionTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("promotions")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("type").AsString(64)
                .WithColumn("barcode").AsString(32);
        }

        public override void Down()
        {
            Delete.Table("promotions");
        }
    }
}
using System;
using System.Linq;
using Autofac;
using FluentNHibernate.Conventions;
using PosApp.Domain;
using PosApp.Services;
using PosApp.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace PosApp.Test
{
    public class PromotionAppFacts : FactBase
    {
        public PromotionAppFacts(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

      
        [Fact]
        public void should_fail_if_promotions_products_are_not_exist()
        {
            CreateProductFixture(
                new Product { Barcode = "barcode_coca", Id = Guid.NewGuid(), Name = "I do not care", Price = 1M });
            PromotionService promotionsService = CreatePromotionsService();
            string[] barcodes = {"barcode_not_exist"};
            Assert.Throws<ArgumentException>(
                () => promotionsService.AddBarcode("BUY_TWO_GET_ONE", barcodes));
        }
        [Fact]
        public void should_add_barcode_if_it_not_exist()
        {
            CreateProductFixture(
                new Product { Barcode = "barcode_exist", Id = Guid.NewGuid(), Name = "I do not care", Price = 1M });
            PromotionService promotionsService = CreatePromotionsService();
            string type = "BUY_TWO_GET_ONE";
            string[] barcodes = new[] { "barcode_exist"};
            promotionsService.AddBarcode(type,barcodes);
            Assert.Equal("barcode_exist",promotionsService.GetBarcodes(type).Single());
        }
        [Fact]
        public void should_not_add_barcode_if_it_exist()
        {
            CreateProductFixture(
                new Product { Barcode = "barcode_exist", Id = Guid.NewGuid(), Name = "I do not care", Price = 1M });
            CreateDiscountProductFixture(
                new Promotion {Type = "BUY_TWO_GET_ONE",Barcode = "barcode_exist"});
            string type = "BUY_TWO_GET_ONE";
            string[] barcodes = { "barcode_exist"};

            PromotionService promotionsService = CreatePromotionsService();
            promotionsService.AddBarcode(type,barcodes);

            Assert.Equal("barcode_exist", promotionsService.GetBarcodes(type).Single());
        }

        [Fact]
        public void delete_one_barcode_exist()
        {
            CreateProductFixture(
                new Product { Barcode = "barcode_exist", Id = Guid.NewGuid(), Name = "I do not care", Price = 1M });
            CreateDiscountProductFixture(
                new Promotion { Type = "BUY_TWO_GET_ONE", Barcode = "barcode_exist" });
            string type = "BUY_TWO_GET_ONE";
            string[] barcodes = { "barcode_exist" };

            PromotionService promotionsService = CreatePromotionsService();
            promotionsService.DeleteBarcode(type,barcodes);

            Assert.Equal(true,promotionsService.GetBarcodes(type).IsEmpty());
        }
        [Fact]
        public void delete_barcode_not_exist()
        {
            CreateProductFixture(
                new Product { Barcode = "barcode_exist", Id = Guid.NewGuid(), Name = "I do not care", Price = 1M });
            CreateDiscountProductFixture(
                new Promotion { Type = "BUY_TWO_GET_ONE", Barcode = "barcode_exist" });
            PromotionService promotionsService = CreatePromotionsService();
            string type = "BUY_TWO_GET_ONE";
            string[] barcodes = { "barcode_not_exist" };
            promotionsService.DeleteBarcode(type, barcodes);
            Assert.Equal(false, promotionsService.GetBarcodes(type).IsEmpty());
        }
        [Fact]
        public void delete_barcodes_some_exist_and_some_not()
        {
            CreateProductFixture(
                new Product { Barcode = "barcode_exist_1", Id = Guid.NewGuid(), Name = "I do not care", Price = 1M },
                new Product { Barcode = "barcode_exist_2", Id = Guid.NewGuid(), Name = "I do not care", Price = 10M });
            CreateDiscountProductFixture(
                new Promotion { Type = "BUY_TWO_GET_ONE", Barcode = "barcode_exist_1" },
                new Promotion { Type = "BUY_TWO_GET_ONE", Barcode = "barcode_exist_2" });
            PromotionService promotionsService = CreatePromotionsService();
            string type = "BUY_TWO_GET_ONE";
            string[] barcodes = { "barcode_exist" ,"barcode_exist_1"};
            promotionsService.DeleteBarcode(type,barcodes);
            Assert.Equal(0,promotionsService.GetBarcodes(type).Count(p => p.Equals("barcode_exist_1")));
            Assert.Equal("barcode_exist_2",promotionsService.GetBarcodes(type).Single(p => p.Equals("barcode_exist_2")));
        }
        PromotionService CreatePromotionsService()
        {
            var promotionService = GetScope().Resolve<PromotionService>();
            return promotionService;
        }
        void CreateProductFixture(params Product[] products)
        {
            Array.ForEach(products, p => Fixtures.Products.Create(p));
        }
        void CreateDiscountProductFixture(params Promotion[] promotions)
        {
            Array.ForEach(promotions, p => Fixtures.Promotions.Create(p));
        }
    }
}
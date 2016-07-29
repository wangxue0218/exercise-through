using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PosApp.Domain;
using PosApp.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace PosApp.Test.Apis
{
    public class ReceiptControllerFacts : ApiFactBase
    {
        public ReceiptControllerFacts(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public async Task should_get_400_if_request_is_not_valid()
        {
            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "receipt",
                new string[0]);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task should_get_400_if_product_does_not_exist()
        {
            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "receipt",
                new[] {"barcode-does-not-exist"});

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task should_get_receipt_with_promotions()
        {
            Fixtures.Products.Create(
                new Product { Barcode = "barcode_coca", Id = Guid.NewGuid(), Name = "Coca Cola", Price = 1M },
                new Product { Barcode = "barcode_poky", Id = Guid.NewGuid(), Name = "Poky", Price = 10M });
            Fixtures.Promotions.Create(
                new Promotion { Type = "BUY_TWO_GET_ONE", Barcode = "barcode_coca" },
                new Promotion { Type = "BUY_TWO_GET_ONE", Barcode = "barcode_poky" });
            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "receipt",
                new[] { "barcode_coca-2", "barcode_poky-3", "barcode_coca", "barcode_poky" });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string receipt = await response.Content.ReadAsStringAsync();
            Assert.Equal(
                "Receipt:\r\n" +
                "--------------------------------------------------\r\n" +
                "Product: Coca Cola, Amount: 3, Price: 3.00, Promoted: 1.00\r\n" +
                "Product: Poky, Amount: 4, Price: 40.00, Promoted: 10.00\r\n" +
                "--------------------------------------------------\r\n" +
                "Total: 32.00\r\n" +
                "Promoted: 11.00",
                receipt);
        }

//        [Fact]
//        public async Task should_get_receipt_with_multi_promotions()
//        {
//            Fixtures.Products.Create(
//                new Product { Barcode = "barcode_coca", Id = Guid.NewGuid(), Name = "Coca Cola", Price = 1M },
//                new Product { Barcode = "barcode_poky", Id = Guid.NewGuid(), Name = "Poky", Price = 10M },
//                new Product {Barcode = "barcode", Id = Guid.NewGuid(), Name ="I do not care", Price = 100M});
//            Fixtures.Promotions.Create(
//                new Promotion { Type = "BUY_TWO_GET_ONE", Barcode = "barcode_coca" },
//                new Promotion { Type = "BUY_TWO_GET_ONE", Barcode = "barcode_poky" },
//                new Promotion {Type = "BUY_HUNDRED_CUT_FIFTY", Barcode = "barcode"});
//            HttpClient httpClient = CreateHttpClient();
//            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
//                "receipt",
//                new[] { "barcode_coca-2", "barcode_poky-3", "barcode_coca", "barcode_poky", "barcode" });
//
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//
//            string receipt = await response.Content.ReadAsStringAsync();
//            Assert.Equal(
//                "Receipt:\r\n" +
//                "--------------------------------------------------\r\n" +
//                "Product: Coca Cola, Amount: 3, Price: 3.00, Promoted: 1.00\r\n" +
//                "Product: Poky, Amount: 4, Price: 40.00, Promoted: 10.00\r\n" +
//                "Product: I do not care, Amount: 1, Price: 100, Promoted: 0\r\n" +
//                "--------------------------------------------------\r\n" +
//                "Total: 82.00\r\n" +
//                "Promoted: 61.00",
//                receipt);
//        }
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PosApp.Domain;
using PosApp.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace PosApp.Test.Apis
{
    public class PromotionsControllerFacts : ApiFactBase
    {
        public PromotionsControllerFacts(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public async Task should_return_bad_request_when_request_json_is_null()
        {
            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "promotions/BUY_TWO_GET_ONE", new {});
            Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
        }
        [Fact]
        public async Task should_return_400_when_add_one_barcode_not_in_products()
        {
            Fixtures.Products.Create(
                new Product { Barcode = "barcode_coca", Id = Guid.NewGuid(), Name = "I do not care", Price = 1M });
            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "promotions/BUY_TWO_GET_ONE", new[] { "barcode-not-exist" });
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task should_return_201_when_the_barcode_exist_but_not_add()
        {
            Fixtures.Products.Create(
                new Product { Barcode = "barcode_exist", Id = Guid.NewGuid(), Name = "I do not care", Price = 1M });
            Fixtures.Promotions.Create(
                new Promotion {Type = "BUY_TWO_GET_ONE", Barcode = "barcode_exist"});

            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "promotions/BUY_TWO_GET_ONE", new[] { "barcode_exist" });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
        [Fact]
        public async Task should_return_201_and_add_barcode()
        {
            Fixtures.Products.Create(
                new Product { Barcode = "barcode_exist", Id = Guid.NewGuid(), Name = "I do not care", Price = 1M });
            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "promotions/BUY_TWO_GET_ONE", new[] { "barcode_exist" });
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
        [Fact]
        public async Task should_get_barcodes_by_type()
        {
            Fixtures.Promotions.Create(
                new Promotion { Type = "BUY_TWO_GET_ONE", Barcode = "barcode_exist" });

            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync("promotions/BUY_TWO_GET_ONE");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            IList<string> barcodes = await response.Content.ReadAsAsync<IList<string>>();
            Assert.Equal("barcode_exist",barcodes[0]);

        }

        [Fact]
        public async Task should_delete_barcode_when_it_exist()
        {
            Fixtures.Promotions.Create(
                new Promotion { Type = "BUY_TWO_GET_ONE", Barcode = "barcode_exist" });

            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync(
                "promotions/BUY_TWO_GET_ONE", new[] { "barcode_exist" });

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }
        [Fact]
        public async Task should_delete_barcode_when_it_not_exist()
        {
            Fixtures.Promotions.Create(
                new Promotion { Type = "BUY_TWO_GET_ONE", Barcode = "barcode_exist" });
            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync("promotions/BUY_TWO_GET_ONE"
                , new[] { "barcode_not_exist" });
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PosApp.Services;

namespace PosApp.Controllers
{
    public class PromotionsController : ApiController
    {
        readonly PromotionService m_promotionService;

        public PromotionsController(PromotionService promotionService)
        {
            m_promotionService = promotionService;
        }

        [HttpPost]
        public HttpResponseMessage InsertPromotions(string type, string[] tags)
        {
            if (tags == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                m_promotionService.AddBarcode(type, tags);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (ArgumentException error)
            {
                throw new HttpException(400, error.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPromotions(string type)
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                m_promotionService.GetBarcodes(type));
        }

        [HttpDelete]
        public HttpResponseMessage DeletePromotions(string type, string[] tags)
        {
            m_promotionService.DeleteBarcode(type, tags);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
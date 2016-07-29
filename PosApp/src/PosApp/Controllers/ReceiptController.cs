using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PosApp.Domain;
using PosApp.Dtos.Requests;
using PosApp.Dtos.Responses;
using PosApp.Services;
using WebApiContrib.Formatting;

namespace PosApp.Controllers
{
    public class ReceiptController : ApiController
    {
        readonly PosService m_posService;

        public ReceiptController(PosService posService)
        {
            m_posService = posService;
        }

        [HttpPost]
        public HttpResponseMessage BuildReceipt(string[] tags)
        {
            BoughtProduct[] boughtProducts = tags.ToBoughtProducts();
            if (boughtProducts == null)
            {
                throw new HttpException(400, "Invalid tags format.");
            }

            try
            {
                Receipt receipt = m_posService.GetReceipt(boughtProducts);
                return Request.CreateResponse(HttpStatusCode.OK, receipt.ToReceiptDto(), new PlainTextFormatter());
            }
            catch (ArgumentException error)
            {
                throw new HttpException(400, error.Message);
            }
        }
    }
}
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using PosApp.Dtos;
using PosApp.Dtos.Responses;

namespace PosApp
{
    class PosAppExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(
            HttpActionExecutedContext actionExecutedContext)
        {
            Exception exception = actionExecutedContext?.Exception;
            if (exception == null) return;

            Console.WriteLine(exception.ToString());

            var httpException = exception as HttpException;
            if (httpException != null)
            {
                actionExecutedContext.Response = CreateErrorResponse(
                    actionExecutedContext.Request,
                    (HttpStatusCode) httpException.GetHttpCode(),
                    httpException.Message);
                return;
            }

            var httpResponseException = exception as HttpResponseException;
            if (httpResponseException != null)
            {
                actionExecutedContext.Response = CreateErrorResponse(
                    actionExecutedContext.Request,
                    httpResponseException.Response.StatusCode,
                    httpResponseException.Message);
                return;
            }

            actionExecutedContext.Response = CreateErrorResponse(
                actionExecutedContext.Request,
                HttpStatusCode.InternalServerError,
                exception.Message);
        }

        static HttpResponseMessage CreateErrorResponse(
            HttpRequestMessage request,
            HttpStatusCode httpStatusCode,
            string message)
        {
            return request.CreateResponse(
                httpStatusCode,
                new ErrorDto
                {
                    Message = message
                });
        }
    }
}
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace PosApp.Test.Common
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> DeleteAsync<T>(
            this HttpClient httpClient,
            string uri,
            T value)
        {
            return httpClient.SendAsync(
                new HttpRequestMessage(HttpMethod.Delete, uri)
                {
                    Content = new ObjectContent<T>(value,new JsonMediaTypeFormatter())
                });
        } 
    }
}
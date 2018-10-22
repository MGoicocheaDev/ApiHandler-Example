using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;

namespace Rest_Example.Handler
{
    public class ApiLogHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.LocalPath.Contains("swagger"))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var requestMetadata = BuildRequestMetadata(request);
            var response = await base.SendAsync(request, cancellationToken);
            var responseMetada = BuildResponseMetadata(response);

            LogApi logApi = new LogApi
            {
                Request = requestMetadata,
                Response = responseMetada
            };

            await SendToLog(logApi);
            return response;
        }

        /// <summary>
        /// Captura el request del Rest-API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private RequestApi BuildRequestMetadata(HttpRequestMessage request)
        {
            var requestMessage = request.Content.ReadAsByteArrayAsync();
            RequestApi requestApi = new RequestApi
            {
               // ContentType = request.Content.Headers.ContentType.ToString(),
                Url = request.RequestUri.ToString(),
                RequestDateTime = DateTime.Now.ToString("s"),
                Headers = request.Headers,
                IpAddress = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "0.0.0.0",
                Method = request.Method.ToString(),
               // Body = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(requestMessage.Result))
            };

            if (request.Content.Headers.ContentType != null)
            {
                requestApi.ContentType = request.Content.Headers.ContentType.ToString();
            }

            if (requestMessage.Result != null)
            {
                requestApi.Body = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(requestMessage.Result));
            }

            return requestApi;
        }

        /// <summary>
        /// Captura el response del Rest-Api
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private ResponseApi BuildResponseMetadata(HttpResponseMessage response)
        {
            var responseMessate = response.Content.ReadAsByteArrayAsync();

            ResponseApi responseApi = new ResponseApi
            {
                ContentType = response.Content.Headers.ContentType.ToString(),
                StatusCode = response.StatusCode,
                ResponseDateTime = DateTime.Now.ToString("s"),
                Body = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(responseMessate.Result))
            };

            return responseApi;
        }

        /// <summary>
        /// Persiste los datos trazados en una bd, archivo, etc
        /// </summary>
        /// <param name="logMetadata"></param>
        /// <returns></returns>
        private async Task<bool> SendToLog(LogApi logMetadata)
        {
            LogError(JsonConvert.SerializeObject(logMetadata, Formatting.Indented));
            return true;
        }


        public static void LogError(string message)
        {
            string currentPath = System.AppDomain.CurrentDomain.BaseDirectory; //System.IO.Directory.GetCurrentDirectory();


            using (StreamWriter w = File.AppendText(currentPath + "\\log.txt"))
            {
                Log(message, w);
            }
        }


        private static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }

    }
}
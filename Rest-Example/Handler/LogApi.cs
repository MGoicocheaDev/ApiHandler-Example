using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;

namespace Rest_Example.Handler
{
    public class LogApi
    {
        public RequestApi Request { get; set; }
        public ResponseApi Response { get; set; }

    }

    public class RequestApi {
        public string ContentType { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string IpAddress { get; set; }
        public HttpHeaders Headers { get; set; }
        public string RequestDateTime { get; set; }
        public object Body { get; set; }
    }

    public class ResponseApi {
        public string ContentType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ResponseDateTime { get; set; }
        public object Body { get; set; }

    }
}
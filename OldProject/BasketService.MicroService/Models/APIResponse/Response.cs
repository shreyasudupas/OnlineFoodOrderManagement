using System;
using System.Net;

namespace Identity.MicroService.Models.APIResponse
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public Object Content { get; set; }
        public Object Exception { get; set; }


        public Response(HttpStatusCode _httpStatusCode,Object _Content,Object _Exception)
        {
            StatusCode = _httpStatusCode;
            Content = _Content;
            Exception = _Exception;
        }
    }
}

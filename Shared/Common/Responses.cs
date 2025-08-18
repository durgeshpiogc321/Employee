using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common
{
    public class Responses<T>
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        public Responses()
        {
            
        }

        public Responses(T data, HttpStatusCode statusCode= HttpStatusCode.InternalServerError, string message="")
        {
            Data = data;
            StatusCode = statusCode;
            Message = message;
        }
    }
}

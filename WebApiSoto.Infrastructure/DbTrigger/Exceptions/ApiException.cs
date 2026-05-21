using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebApiSoto.API.Middleware.Exceptions
{
    public class ApiException  : ApplicationException
    {
       public int StatusCode  { get; set; }

        public ApiException(int statusCode, string message, Exception? innerEx = null): base(message, innerEx)
        {
            this.StatusCode = statusCode;
        }
    }
}

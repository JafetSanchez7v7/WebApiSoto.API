using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.API.Middleware.Exceptions;

namespace WebApiSoto.Infrastructure.DbTrigger.Exceptions
{
    public sealed class InputInvalidoException : ApiException
    {
        public InputInvalidoException(string message, int statusCode) : base(statusCode, message)
        {
            
        }
    }
}

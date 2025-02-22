using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Utilities
{
    public class OperationHandler
    {
        public string Message { get; set; }
        public Status Status { get; set; }

        public OperationHandler Success(string message)
        {
            return new OperationHandler()
            {
                Message = message
            };
        }
        public OperationHandler Error(string message)
        {
            return new OperationHandler()
            {
                Message = message
            };
        }
        public OperationHandler NotFound(string message)
        {
            return new OperationHandler()
            {
                Message = message
            };
        }
    }

    public enum Status
    {
        Success = 200  , 
        NotFound = 404 , 
        Error = 10
    }
}

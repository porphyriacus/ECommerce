using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Common.Behaviors.Errors
{
    public enum ErrorType
    {
        None,
        Failure,       
        NotFound,      
        Validation,    
        Conflict,       
        Unauthorized,

    }
}

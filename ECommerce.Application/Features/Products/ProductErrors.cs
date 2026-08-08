using ECommerce.Application.Common.Behaviors.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Products
{
    public static class ProductErrors
    {
        public static Error Validation(string message) 
            => Error.Validation("ProductError.Validation", message);
    }
}

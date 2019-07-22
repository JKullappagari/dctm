using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCTMRestAPI.Extensions
{
    public class CustomAuthOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Security == null)
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
            operation.Security.Add(new Dictionary<string, IEnumerable<string>>
        {
            { "Custom", new string []{} } // this should match the security definition name
        });
        }

        //[SwaggerOperationFilter(typeof(CustomAuthOperationFilter))]
        //public Task<IActionResult> Operation()
        //{
        //    return OkResult;
        //    // ...
        //}
    }
}

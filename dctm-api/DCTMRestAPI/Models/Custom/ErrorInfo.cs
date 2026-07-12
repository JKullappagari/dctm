using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DCTMRestAPI.Models.Custom
{
    public class ErrorInfo
    {
        public string Entity { get; set; }

        public string Id { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    //public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    //{
    //    public override void OnException(ExceptionContext context)
    //    {
    //        var exception = context.Exception;
    //        context.Result = new ContentResult
    //        {
    //            Content = context.,
    //            ContentType = "application/plain",
    //            // change to whatever status code you want to send out
    //            StatusCode = (int?)HttpStatusCode.BadRequest
    //        };
    //    }
    //}

}

using DCTMRestAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DCTMRestAPI.Extensions
{
    /// <summary>
    /// Filter to enable handling file upload in swagger
    /// </summary>
    public class FormFileSwaggerFilter : IOperationFilter
    {
        private readonly IEnumerable<string> _actionsWithUpload = new[]
        {
            //add your upload actions here!
            NamingHelpers.GetOperationId<LogUploadController>(nameof(LogUploadController.UploadLog))
        };

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (_actionsWithUpload.Contains(operation.OperationId))
            {
                //operation.Parameters.Clear();
                var fileParameter = operation.Parameters.FirstOrDefault(x => x.Name == "File" && x.In == "query");
                if (fileParameter != null)
                {
                    operation.Parameters.Remove(fileParameter);
                }

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "File",
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }

    /// <summary>
    /// Refatoring friendly helper to get names of controllers and operation ids
    /// </summary>
    public class NamingHelpers
    {
        public static string GetOperationId<T>(string actionName) where T : ControllerBase => $"Api{GetControllerName<T>()}Post";

        public static string GetControllerName<T>() where T : ControllerBase => typeof(T).Name.Replace(nameof(Controller), string.Empty);
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DCTMRestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace DCTMRestAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LogUploadController : ControllerBase
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly DCTrackContext _context;
        private readonly ILogger _logger;

        public LogUploadController(IHostingEnvironment hostingEnvironment, DCTrackContext context, ILogger<AssetsController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// To upload logs from device
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        [HttpPost]
        //[DisableFormValueModelBinding]
        [Authorize(Roles = "Mobile")]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadLog([FromForm]UploadFile uploadFile)
        {
            //FormValueProvider formModel;
            var viewModel = new DeviceDetails();
            string FileName = string.Empty;
            try
            {

                if (uploadFile != null && !string.IsNullOrEmpty(uploadFile.FileName) && !string.IsNullOrEmpty(uploadFile.DeviceId)
                    && uploadFile.LogFile != null)
                {
                    FileName = uploadFile.FileName;
                    viewModel.FileName = FileName;
                    List<TblMobileDevice> device = (from d in _context.TblMobileDevice
                                                    where d.DeviceId.ToLower().CompareTo(uploadFile.DeviceId.ToLower()) == 0
                                                    select d).ToList();

                    //Current root directory will be inetpub\wwwroot\dctmrest folder
                    // so go one level up and search for Logs folder, if not exists than create it and create one more folder based on device name specfied during 
                    // register device.
                    string path = _hostingEnvironment.ContentRootPath;
                    string rootPath = System.IO.Directory.GetParent(path).ToString();
                    string logsPath = rootPath + "\\Logs";
                    string deviceLogsPath = logsPath + "\\" + device[0].DeviceName;
                    if (!Directory.Exists(logsPath))
                    {
                        Directory.CreateDirectory(logsPath);
                    }

                    if (!Directory.Exists(deviceLogsPath))
                    {
                        Directory.CreateDirectory(deviceLogsPath);
                    }


                    if (System.IO.File.Exists(deviceLogsPath + "\\" + FileName + ".temp"))
                        System.IO.File.Delete(deviceLogsPath + "\\" + FileName + ".temp");


                    using (var stream = new FileStream(deviceLogsPath + "\\" + FileName + ".temp", FileMode.Create))
                    {
                        await uploadFile.LogFile.CopyToAsync(stream);
                    }

                    //using (var stream1 = System.IO.File.Create(deviceLogsPath + "\\" + FileName + ".temp"))
                    //{
                    //    formModel = await Request.StreamFile(stream1);
                    //}

                    //delete old file with same name
                    if (System.IO.File.Exists(deviceLogsPath + "\\" + FileName))
                        System.IO.File.Delete(deviceLogsPath + "\\" + FileName);

                    System.IO.File.Copy(deviceLogsPath + "\\" + FileName + ".temp", deviceLogsPath + "\\" + FileName, true);
                    //delete temp file
                    if (System.IO.File.Exists(deviceLogsPath + "\\" + FileName + ".temp"))
                        System.IO.File.Delete(deviceLogsPath + "\\" + FileName + ".temp");
                    //since we are always sending File contenet disposition, form collection is always null

                    //var bindingSuccessful = await TryUpdateModelAsync(viewModel, prefix: "",
                    //   valueProvider: formModel);
                    //if (!bindingSuccessful)
                    //{
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    //}

                    viewModel.UploadStatus = "Success";
                }
                else
                {
                    return BadRequest("Invalid/missing parameters.Log upload failed.");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Log upload failed.");
                return BadRequest("Log upload failed.");
            }

            return Ok(viewModel);

        }
    }

    public class UploadFile
    {
        public IFormFile LogFile { get; set; }
        public string FileName { get; set; }
        public string DeviceId { get; set; }
       

    }
    public class DeviceDetails
    {
        //public string DeviceId { get; set; }
        public string FileName { get; set; }
        public string UploadStatus { get; set; }
    }

    /*
    public static class MultipartRequestHelper
    {
        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec says 70 characters is a reasonable limit.
        public static string GetBoundary(Microsoft.Net.Http.Headers.MediaTypeHeaderValue contentType, int lengthLimit)
        {
            //var boundary = Microsoft.Net.Http.Headers.HeaderUtilities.RemoveQuotes(contentType.Boundary);// .NET Core <2.0
            var boundary = Microsoft.Net.Http.Headers.HeaderUtilities.RemoveQuotes(contentType.Boundary).Value; //.NET Core 2.0
            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }

        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                    && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool HasFormDataContentDisposition(Microsoft.Net.Http.Headers.ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                    && contentDisposition.DispositionType.Equals("form-data")
                    && string.IsNullOrEmpty(contentDisposition.FileName.Value) // For .NET Core <2.0 remove ".Value"
                    && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value); // For .NET Core <2.0 remove ".Value"
        }

        public static bool HasFileContentDisposition(Microsoft.Net.Http.Headers.ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                    && contentDisposition.DispositionType.Equals("form-data")
                    && (!string.IsNullOrEmpty(contentDisposition.FileName.Value) // For .NET Core <2.0 remove ".Value"
                        || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value)); // For .NET Core <2.0 remove ".Value"
        }
    }

    public static class FileStreamingHelper
    {
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public static async Task<FormValueProvider> StreamFile(this HttpRequest request, Stream targetStream)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(request.ContentType))
            {
                throw new Exception($"Expected a multipart request, but got {request.ContentType}");
            }

            // Used to accumulate all the form url encoded key value pairs in the 
            // request.
            var formAccumulator = new KeyValueAccumulator();
            string targetFilePath = null;

            var boundary = MultipartRequestHelper.GetBoundary(
                Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse(request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, request.Body);

            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                Microsoft.Net.Http.Headers.ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        await section.Body.CopyToAsync(targetStream);
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        // Content-Disposition: form-data; name="key"
                        //
                        // value

                        // Do not limit the key name length here because the 
                        // multipart headers length limit is already in effect.
                        var key = Microsoft.Net.Http.Headers.HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                        var encoding = GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            // The value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync();
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }
                            formAccumulator.Append(key.Value, value); // For .NET Core <2.0 remove ".Value" from key

                            if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            // Bind form data to a model
            var formValueProvider = new FormValueProvider(
                BindingSource.Form,
                new FormCollection(formAccumulator.GetResults()),
                CultureInfo.CurrentCulture);

            return formValueProvider;
        }

        private static System.Text.Encoding GetEncoding(MultipartSection section)
        {
            Microsoft.Net.Http.Headers.MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = Microsoft.Net.Http.Headers.MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || System.Text.Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var formValueProviderFactory = context.ValueProviderFactories
                .OfType<FormValueProviderFactory>()
                .FirstOrDefault();
            if (formValueProviderFactory != null)
            {
                context.ValueProviderFactories.Remove(formValueProviderFactory);
            }

            var jqueryFormValueProviderFactory = context.ValueProviderFactories
                .OfType<JQueryFormValueProviderFactory>()
                .FirstOrDefault();
            if (jqueryFormValueProviderFactory != null)
            {
                context.ValueProviderFactories.Remove(jqueryFormValueProviderFactory);
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
    */


}
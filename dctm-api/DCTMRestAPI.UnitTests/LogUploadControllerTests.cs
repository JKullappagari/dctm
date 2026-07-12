using System.IO;
using System.Text;
using System.Threading.Tasks;
using DCTMRestAPI.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DCTMRestAPI.UnitTests
{
    public class LogUploadControllerTests
    {
        private static LogUploadController Controller()
        {
            var env = new Mock<IWebHostEnvironment>();
            env.SetupGet(e => e.ContentRootPath).Returns(Path.GetTempPath());
            var logger = new Mock<ILogger<AssetsController>>();
            return new LogUploadController(env.Object, TestDb.NewContext(), logger.Object);
        }

        [Fact]
        public async Task UploadLog_null_input_returns_BadRequest()
        {
            var result = await Controller().UploadLog(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UploadLog_missing_deviceId_returns_BadRequest()
        {
            var bytes = Encoding.UTF8.GetBytes("log contents");
            var file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "LogFile", "app.log");
            var upload = new UploadFile { LogFile = file, FileName = "app.log", DeviceId = "" };

            var result = await Controller().UploadLog(upload);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}

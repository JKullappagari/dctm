using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DCTMRestAPI.Controllers;
using DCTMRestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DCTMRestAPI.UnitTests
{
    public class CheckOutPurposesControllerTests
    {
        private readonly Mock<ILogger<CheckOutPurposesController>> _logger = new();

        private CheckOutPurposesController Controller(out DCTrackContext ctx)
        {
            ctx = TestDb.NewContext();
            return new CheckOutPurposesController(ctx, _logger.Object);
        }

        [Fact]
        public async Task Post_with_invalid_model_state_returns_BadRequest()
        {
            var controller = Controller(out _);
            controller.ModelState.AddModelError("Id", "required");

            var result = await controller.Post(new List<TblCheckoutPurpose>());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_valid_record_persists_and_returns_Ok()
        {
            var controller = Controller(out var ctx);
            var rows = new List<TblCheckoutPurpose>
            {
                new() { Id = Guid.NewGuid(), CheckoutPurposeId = 5, CheckoutSessionId = Guid.NewGuid() },
            };

            var result = await controller.Post(rows);

            Assert.IsType<OkResult>(result);          // no per-record errors
            Assert.Equal(1, ctx.TblCheckoutPurpose.Count());
        }

        [Fact]
        public async Task Get_returns_seeded_rows()
        {
            var controller = Controller(out var ctx);
            ctx.TblCheckoutPurpose.Add(new TblCheckoutPurpose { Id = Guid.NewGuid(), CheckoutPurposeId = 7 });
            ctx.SaveChanges();

            var result = await controller.Get();

            Assert.Single(result);
        }
    }
}

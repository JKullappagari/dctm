using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DCTMRestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DCTMRestAPI.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]")]
    public class CheckOutSessionsController : Controller
    {

        private readonly DCTrackContext _context;
        private readonly ILogger _logger;

        public CheckOutSessionsController(DCTrackContext context,ILogger<CheckOutSessionsController> logger)
        {
            _context = context;
            _logger = logger;

        }

        /// <summary>
        /// Gets all checkout sessions
        /// </summary>
        /// <returns></returns>
        // GET: api/checkoutsessions
        [HttpGet]
        [ProducesResponseType(typeof(TblCheckOutSession), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblCheckOutSession>> Get()
        {
            List<TblCheckOutSession> checkoutSessions = await (from g in _context.TblCheckOutSession
                                            select g).AsNoTracking().ToListAsync();
            return checkoutSessions;
        }

        /// <summary>
        /// Gets checkout session based on identifier
        /// </summary>
        /// <param name="CheckOutSessionId"></param>
        /// <returns></returns>
        // GET api/checkoutsessions/5
        [HttpGet("{CheckOutSessionId}")]
        [ProducesResponseType(typeof(TblCheckOutSession), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblCheckOutSession>> Get(int CheckOutSessionId)
        {
            List<TblCheckOutSession> checkoutSessions = await (from g in _context.TblCheckOutSession
                                                  where g.CheckOutSessionId == CheckOutSessionId
                                                         select g).AsNoTracking().ToListAsync();
            return checkoutSessions;
        }


        /// <summary>
        /// Gets checkout sessions which are modified after Last Updated datetime
        /// </summary>
        // GET api/checkoutsessions/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblCheckOutSession), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblCheckOutSession>> Get(long LastUpdatedTime)
        {
            List<TblCheckOutSession> purposes = await (from g in _context.TblCheckOutSession
                                                 where g.LastUpdatedTime > LastUpdatedTime
                                                 select g).AsNoTracking().ToListAsync();
            return purposes;
        }

        /// <summary>
        /// Creates new checkout session record
        /// </summary>
        /// <response code="200" >No reponse was specified</response>
        /// <response code="204" >No content</response>
        /// <param name="value">Checkout session list</param>
        //POST api/checkoutsessions
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = "Mobile")]
        public async Task<IActionResult> Post([FromBody]List<TblCheckOutSession> value)
        {
            List<CheckoutSessionFailed> errors = new List<CheckoutSessionFailed>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (TblCheckOutSession session in value)
            {

                try
                {
                    _context.Entry(session).State = EntityState.Added;

                    _context.Database.BeginTransaction();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.tblCheckOutSession ON;");
                    await _context.SaveChangesAsync();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.tblCheckOutSession OFF;");
                    _context.Database.CommitTransaction();

                }
                catch (DbUpdateConcurrencyException ex1)
                {
                    _logger.LogCritical(ex1, "Post Request");

                    CheckoutSessionFailed failed = new CheckoutSessionFailed();
                    failed.Id = session.Id.ToString();
                    failed.CheckoutSessionId = session.CheckOutSessionId;
                    failed.ErrorMessage = ex1.Message;
                    errors.Add(failed);

                }
                catch (Exception ex2)
                {
                    _logger.LogCritical(ex2, "Post Request");
                    CheckoutSessionFailed failed = new CheckoutSessionFailed();
                    failed.Id = session.Id.ToString();
                    failed.CheckoutSessionId = session.CheckOutSessionId;

                    if (ex2.InnerException != null)
                        failed.ErrorMessage = ex2.InnerException.Message;
                    else
                        failed.ErrorMessage = ex2.Message;
                    errors.Add(failed);
                }
            }
            if (errors.Count > 0)
                return Ok(errors);
            else
                return Ok();


        }

    }

    public class CheckoutSessionFailed
    {

        public string Id { get; set; }
        public long CheckoutSessionId { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

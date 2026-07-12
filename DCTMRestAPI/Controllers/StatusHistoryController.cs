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
    public class StatusHistoryController : Controller
    {

        private readonly DCTrackContext _context;
        private readonly ILogger _logger;

        public StatusHistoryController(DCTrackContext context, ILogger<StatusHistoryController> logger)
        {
            _context = context;
            _logger = logger;

        }

        /// <summary>
        /// Gets all asset status history records
        /// </summary>
        /// <returns></returns>
        // GET: api/statushistory
        [HttpGet]
        [ProducesResponseType(typeof(TblStatusHistory), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblStatusHistory>> Get()
        {
            List<TblStatusHistory> history = await (from g in _context.TblStatusHistory
                                            select g).AsNoTracking().ToListAsync();
            return history;
        }

        /// <summary>
        /// Gets asset status history records based on identifier
        /// </summary>
        /// <param name="StatusHistoryId"></param>
        /// <returns></returns>
        // GET api/statushistory/5
        [HttpGet("{StatusHistoryId}")]
        [ProducesResponseType(typeof(TblStatusHistory), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblStatusHistory>> Get(int StatusHistoryId)
        {
            List<TblStatusHistory> history = await (from g in _context.TblStatusHistory
                                                where g.StatusHistoryId == StatusHistoryId
                                                select g).AsNoTracking().ToListAsync();
            return history;
        }

        /// <summary>
        /// Gets status history records which are modified after Last Updated datetime
        /// </summary>
        // GET api/statushistory/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblStatusHistory), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblStatusHistory>> Get(long LastUpdatedTime)
        {
            List<TblStatusHistory> history = await (from g in _context.TblStatusHistory
                                                 where g.LastUpdatedTime > LastUpdatedTime
                                                 select g).AsNoTracking().ToListAsync();
            return history;
        }

        /// <summary>
        /// Creates new asset status history record
        /// </summary>
        /// <response code="200" >No reponse was specified</response>
        /// <response code="204" >No content</response>
        /// <param name="value"> Status history list</param>
        //POST api/checkoutpurposes
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = "Mobile")]
        public async Task<IActionResult> Post([FromBody]List<TblStatusHistory> value)
        {
            List<StatusHistoryFailed> errors = new List<StatusHistoryFailed>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (TblStatusHistory sh in value)
            {
                try
                {
                    _context.Entry(sh).State = EntityState.Added;

                    _context.Database.BeginTransaction();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.tblStatusHistory ON;");
                    await _context.SaveChangesAsync();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.tblStatusHistory OFF;");
                    _context.Database.CommitTransaction();


                }
                catch (DbUpdateConcurrencyException ex1)
                {
                    _logger.LogCritical(ex1, "Post Request");

                    StatusHistoryFailed failed = new StatusHistoryFailed();
                    failed.Id = sh.Id.ToString();
                    failed.StatusHistoryID = sh.StatusHistoryId;
                    failed.ErrorMessage = ex1.Message;
                    errors.Add(failed);

                }
                catch (Exception ex2)
                {
                    _logger.LogCritical(ex2, "Post Request");
                    StatusHistoryFailed failed = new StatusHistoryFailed();
                    failed.Id = sh.Id.ToString();
                    failed.StatusHistoryID = sh.StatusHistoryId;

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

    public class StatusHistoryFailed
    {

        public string Id { get; set; }
        public long StatusHistoryID { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}

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
    public class AssetTransLogsController : Controller
    {

        private readonly DCTrackContext _context;
        private readonly ILogger _logger;

        public AssetTransLogsController(DCTrackContext context,ILogger<AssetTransLogsController> logger)
        {
            _context = context;
            _logger = logger;

        }

        /// <summary>
        /// Gets all asset transaction logs 
        /// </summary>
        /// <returns></returns>
        // GET: api/assettranslogs
        [HttpGet]
        [ProducesResponseType(typeof(TblAssetTransactionLog), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblAssetTransactionLog>> Get()
        {
            List<TblAssetTransactionLog> logs = await (from g in _context.TblAssetTransactionLog
                                            select g).AsNoTracking().ToListAsync();
            return logs;
        }

        /// <summary>
        /// Gets asset transaction logs based on identifier
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        // GET api/assettranslogs/5
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(TblAssetTransactionLog), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblAssetTransactionLog>> Get(int Id)
        {
            List<TblAssetTransactionLog> logs = await (from g in _context.TblAssetTransactionLog
                                            where g.TransactionId == Id
                                            select g).AsNoTracking().ToListAsync();
            return logs;
        }

        /// <summary>
        /// Gets asset transaction logs which are modified after Last Updated datetime
        /// </summary>
        // GET api/assettranslogs/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblAssetTransactionLog), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblAssetTransactionLog>> Get(long LastUpdatedTime)
        {
            List<TblAssetTransactionLog> logs = await (from g in _context.TblAssetTransactionLog
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                 select g).AsNoTracking().ToListAsync();
            return logs;
        }

        /// <summary>
        /// Creates new asset transaction log record
        /// </summary>
        /// <response code="200" >No reponse was specified</response>
        /// <response code="204" >No content</response>
        /// <param name="value">Asset transaction logs</param>
        //POST api/assettranslogs
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = "Mobile")]
        public async Task<IActionResult> Post([FromBody]List<TblAssetTransactionLog> value)
        {
            List<AssetTransactionLogFailed> errors = new List<AssetTransactionLogFailed>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            foreach (TblAssetTransactionLog at in value)
            {
                try
                {
                    _context.Entry(at).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex1)
                {
                    _logger.LogCritical(ex1, "Post Request");

                    AssetTransactionLogFailed failed = new AssetTransactionLogFailed();
                    failed.Id = at.Id.ToString();
                    failed.ErrorMessage = ex1.Message;
                    errors.Add(failed);

                }
                catch (Exception ex2)
                {
                    _logger.LogCritical(ex2, "Post Request");
                    AssetTransactionLogFailed failed = new AssetTransactionLogFailed();
                    failed.Id = at.Id.ToString();

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

    public class AssetTransactionLogFailed
    {

        public string Id { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

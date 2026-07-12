using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DCTMRestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DCTMRestAPI.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]")]
    public class EnclPositionsController : Controller
    {

        private readonly DCTrackContext _context;
        private readonly ILogger _logger;

        public EnclPositionsController(DCTrackContext context,ILogger<EnclPositionsController> logger)
        {
            _context = context;
            _logger = logger;

        }

        /// <summary>
        /// Gets all enclosures position details
        /// </summary>
        /// <returns></returns>
        // GET: api/enclpositions
        [HttpGet]
        [ProducesResponseType(typeof(TblEnclPositions), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblEnclPositions>> Get()
        {
            List<TblEnclPositions> encls = await (from g in _context.TblEnclPositions
                                            select g).AsNoTracking().ToListAsync();
            return encls;
        }

        /// <summary>
        /// Gets enclosure position details based on identifier
        /// </summary>
        /// <param name="EnclId"></param>
        /// <returns></returns>
        // GET api/enclpositions/5
        [HttpGet("{EnclId}")]
        [ProducesResponseType(typeof(TblEnclPositions), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblEnclPositions>> Get(int EnclId)
        {
            List<TblEnclPositions> encls = await (from g in _context.TblEnclPositions
                                                where g.EnclId == EnclId
                                                select g).AsNoTracking().ToListAsync();
            return encls;
        }

        /// <summary>
        /// Gets enclosures whose position details modifeied after Last Updated datetime
        /// </summary>
        // GET api/enclpositions/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblEnclPositions), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblEnclPositions>> Get(long LastUpdatedTime)
        {
            List<TblEnclPositions> encls = await (from g in _context.TblEnclPositions
                                           where g.LastUpdatedTime > LastUpdatedTime
                                           select g).AsNoTracking().ToListAsync();
            return encls;
        }

        /// <summary>
        /// Set enclosure position details based on enclosure identifier
        /// </summary>
        /// <response code="200" >No reponse was specified</response>
        /// <response code="204" >No content</response>
        /// <param name="value">Enclosure positions list</param>
        //PUT api/locations/5
        [HttpPut()]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Authorize(Roles = "Mobile")]
        public async Task<IActionResult> Put([FromBody]List<TblEnclPositions> value)
        {
            List<EnclPositionsFailed> errors = new List<EnclPositionsFailed>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            foreach (TblEnclPositions encl in value)
            {
                try
                {
                    // check last modified date and see whether Server or client has latest record.
                    // latest record will previal
                    List<TblEnclPositions> selectedEncl = await (from g in _context.TblEnclPositions.AsNoTracking()
                                              where g.EnclId == encl.EnclId
                                              select g).AsNoTracking().ToListAsync();
                    if (selectedEncl != null)
                    {
                        if (selectedEncl.Count <= 0)
                        {
                            EnclPositionsFailed failed = new EnclPositionsFailed();
                            failed.Id = encl.Id.ToString();
                            failed.EnclId = encl.EnclId;
                            failed.ErrorMessage = "Enclosure positions record does not exists.";
                            errors.Add(failed);
                        }
                        else
                        {

                            if (selectedEncl[0].LastModifiedDate == null || selectedEncl[0].LastModifiedDate <= encl.LastModifiedDate)
                            {
                                _context.Entry(encl).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                EnclPositionsFailed failed = new EnclPositionsFailed();
                                failed.Id = encl.Id.ToString();
                                failed.EnclId = encl.EnclId;
                                failed.ErrorMessage = "Server Record is latest compared to client record.";
                                errors.Add(failed);
                                
                                //write to sync conflict log
                                StringBuilder sb = new StringBuilder();
                                sb.Append(Environment.NewLine);
                                sb.Append("****************** Enclosure Positions Data Conflict******************");
                                sb.Append(Environment.NewLine);
                                sb.Append("Server Record is latest compared to client record.");
                                sb.Append(Environment.NewLine);
                                sb.Append("Enclosure ID:" + selectedEncl[0].EnclId.ToString());
                                sb.Append(Environment.NewLine);
                                sb.Append("Server Modified Date:" + selectedEncl[0].LastModifiedDate.ToString());
                                sb.Append(Environment.NewLine);
                                sb.Append("Client Modified Date:" + encl.LastModifiedDate.ToString());
                                sb.Append(Environment.NewLine);
                                sb.Append("******************************************************");
                                sb.Append(Environment.NewLine);
                                _logger.LogInformation(sb.ToString());
                                //Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(sb.ToString(), "ConflictLog");
                            }
                        }
                    }
                    else
                    {
                        EnclPositionsFailed failed = new EnclPositionsFailed();
                        failed.Id = encl.Id.ToString();
                        failed.EnclId = encl.EnclId;
                        failed.ErrorMessage = "Failed to get data from database";
                        errors.Add(failed);
                    }
                }
                catch (DbUpdateConcurrencyException ex1)
                {
                    _logger.LogCritical(ex1, "Put Request");

                    EnclPositionsFailed failed = new EnclPositionsFailed();
                    failed.Id = encl.Id.ToString();
                    failed.EnclId = encl.EnclId;
                    failed.ErrorMessage = ex1.Message;
                    errors.Add(failed);

                }
                catch (Exception ex2)
                {
                    _logger.LogCritical(ex2, "Put Request");
                    EnclPositionsFailed failed = new EnclPositionsFailed();
                    failed.Id = encl.Id.ToString();
                    failed.EnclId = encl.EnclId;

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

    public class EnclPositionsFailed
    {

        public string Id { get; set; }

        public int EnclId { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

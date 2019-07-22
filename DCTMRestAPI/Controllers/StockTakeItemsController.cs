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
    public class StockTakeItemsController : Controller
    {

        private readonly DCTrackContext _context;
        private readonly ILogger _logger;

        public StockTakeItemsController(DCTrackContext context,ILogger<StockTakeItemsController> logger)
        {
            _context = context;
            _logger = logger;

        }

        /// <summary>
        /// Gets all stock take items records
        /// </summary>
        /// <returns></returns>
        // GET: api/stocktakeitems
        [HttpGet]
        [ProducesResponseType(typeof(TblStockTakeItems), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblStockTakeItems> Get()
        {
            List<TblStockTakeItems> stocktakeitems = (from g in _context.TblStockTakeItems
                                                 select g).ToList();
            return stocktakeitems;
        }

        /// <summary>
        /// Gets stock take items records based on identifier
        /// </summary>
        /// <param name="StockTakeSessionId"></param>
        /// <returns></returns>
        // GET api/stocktakeitems/5
        [HttpGet("{StockTakeSessionId}")]
        [ProducesResponseType(typeof(TblStockTakeItems), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblStockTakeItems> Get(int StockTakeSessionId)
        {
            List<TblStockTakeItems> stocktakeitems = (from g in _context.TblStockTakeItems
                                                 where g.StockTakeSessionId == StockTakeSessionId
                                                      select g).ToList();
            return stocktakeitems;
        }

        /// <summary>
        /// Gets stock take items which are modified after Last Updated datetime
        /// </summary>
        // GET api/stocktakeitems/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblStockTakeItems), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblStockTakeItems> Get(long LastUpdatedTime)
        {
            List<TblStockTakeItems> purposes = (from g in _context.TblStockTakeItems
                                                 where g.LastUpdatedTime > LastUpdatedTime
                                                 select g).ToList();
            return purposes;
        }

        /// <summary>
        /// Creates new stock take item
        /// </summary>
        /// <response code="200" >No reponse was specified</response>
        /// <response code="204" >No content</response>
        /// <param name="value">Stocktake items list</param>
        //POST api/stocktakeitems
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(200)]
        [Authorize(Roles = "Mobile")]
        public async Task<IActionResult> Post([FromBody]List<TblStockTakeItems> value)
        {
            List<StockTakeItemsFailed> errors = new List<StockTakeItemsFailed>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (TblStockTakeItems stItems in value)
            {
                try
                {
                    _context.Entry(stItems).State = EntityState.Added;

                    //_context.Database.BeginTransaction();
                    //_context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.tblStockTakeItems ON;");
                    await _context.SaveChangesAsync();
                    //_context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.tblStockTakeItems OFF;");
                    //_context.Database.CommitTransaction();


                }
                catch (DbUpdateConcurrencyException ex1)
                {
                    _logger.LogCritical(ex1, "Post Request");

                    StockTakeItemsFailed failed = new StockTakeItemsFailed();
                    failed.Id = stItems.Id.ToString();
                    failed.SessionID = stItems.StockTakeSessionId;
                    failed.AssetId = stItems.AssetId;
                    failed.ErrorMessage = ex1.Message;
                    errors.Add(failed);

                }
                catch (Exception ex2)
                {
                    _logger.LogCritical(ex2, "Post Request");
                    StockTakeItemsFailed failed = new StockTakeItemsFailed();
                    failed.Id = stItems.Id.ToString();
                    failed.SessionID = stItems.StockTakeSessionId;
                    failed.AssetId = stItems.AssetId;
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


    public class StockTakeItemsFailed
    {

        public string Id { get; set; }
        public long SessionID { get; set; }

        public long AssetId { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}

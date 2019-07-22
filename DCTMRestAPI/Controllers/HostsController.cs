using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DCTMRestAPI.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DCTMRestAPI.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]")]
    public class HostsController : Controller
    {

        private readonly DCTrackContext _context;

        public HostsController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all host names
        /// </summary>
        /// <returns></returns>
        // GET: api/hosts
        [HttpGet]
        [ProducesResponseType(typeof(TblHost), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblHost> Get()
        {
            List<TblHost> hosts = (from g in _context.TblHost
                                            select g).ToList();
            return hosts;
        }

        /// <summary>
        /// Gets host name based on identifier
        /// </summary>
        /// <param name="HostId"></param>
        /// <returns></returns>
        // GET api/hosts/5
        [HttpGet("{HostId}")]
        [ProducesResponseType(typeof(TblHost), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblHost> Get(int HostId)
        {
            List<TblHost> hosts = (from g in _context.TblHost
                                       where g.HostId.ToString().ToLower().CompareTo(HostId.ToString().ToLower()) == 0
                                   select g).ToList();
            return hosts;
        }

        /// <summary>
        /// Gets host names which are modified after Last Updated datetime
        /// </summary>
        // GET api/hosts/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblHost), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblHost> Get(long LastUpdatedTime)
        {
            List<TblHost> hosts = (from g in _context.TblHost
                                         where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return hosts;
        }


        //// POST api/hosts
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// DELETE api/hosts/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

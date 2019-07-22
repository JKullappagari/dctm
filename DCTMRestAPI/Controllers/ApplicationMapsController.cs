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
    public class ApplicationMapsController : Controller
    {
        private readonly DCTrackContext _context;

        public ApplicationMapsController(DCTrackContext context)
        {
            _context = context;

        }


        /// <summary>
        /// Gets all air flow directions details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TblApplicationMap), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblApplicationMap> Get()
        {
            List<TblApplicationMap> appMaps = (from g in _context.TblApplicationMap
                                                      select g).ToList();
            return appMaps;
        }

        /// <summary>
        /// Gets air flow directions details based on identifier
        /// </summary>
        // GET api/applicationmaps/5
        [HttpGet("{ApplMapId}")]
        [ProducesResponseType(typeof(TblApplicationMap), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblApplicationMap> Get(string ApplMapId)
        {
            List<TblApplicationMap> appMap = (from g in _context.TblApplicationMap
                                                     where g.ApplMapId.ToString().ToLower().CompareTo(ApplMapId.ToLower()) == 0
                                                   select g).ToList();
            return appMap;
        }

        /// <summary>
        /// Gets air flow directions details which are modified after Last Updated datetime
        /// </summary>
        // GET api/applicationmaps/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblApplicationMap), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblApplicationMap> Get(long LastUpdatedTime)
        {
            List<TblApplicationMap> appMaps = (from g in _context.TblApplicationMap
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return appMaps;
        }

       
    }
}

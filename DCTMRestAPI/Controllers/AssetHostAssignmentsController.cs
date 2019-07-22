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
    public class AssetHostAssignmentsController : Controller
    {
        private readonly DCTrackContext _context;

        public AssetHostAssignmentsController(DCTrackContext context)
        {
            _context = context;

        }


        /// <summary>
        /// Gets all air flow directions details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TblAssetHostAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblAssetHostAssignment> Get()
        {
            List<TblAssetHostAssignment> ahAssignments = (from g in _context.TblAssetHostAssignment
                                                      select g).ToList();
            return ahAssignments;
        }

        /// <summary>
        /// Gets air flow directions details based on identifier
        /// </summary>
        // GET api/AssetHostAssignments/5
        [HttpGet("{ID}")]
        [ProducesResponseType(typeof(TblAssetHostAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblAssetHostAssignment> Get(string ID)
        {
            List<TblAssetHostAssignment> ahAssignment = (from g in _context.TblAssetHostAssignment
                                                     where g.Id.ToString().ToLower().CompareTo(ID.ToLower()) == 0
                                                     select g).ToList();
            return ahAssignment;
        }

        /// <summary>
        /// Gets air flow directions details which are modified after Last Updated datetime
        /// </summary>
        // GET api/AssetHostAssignments/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblAssetHostAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblAssetHostAssignment> Get(long LastUpdatedTime)
        {
            List<TblAssetHostAssignment> ahAssignments = (from g in _context.TblAssetHostAssignment
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return ahAssignments;
        }

    }
}

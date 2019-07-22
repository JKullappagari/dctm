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
    public class TenantHostAssignmentsController : Controller
    {
        private readonly DCTrackContext _context;

        public TenantHostAssignmentsController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all tenant-host assignment details
        /// </summary>
        /// <returns></returns>
        // GET: api/Tenanthostassignments
        [HttpGet]
        [ProducesResponseType(typeof(TblTenantHostAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblTenantHostAssignment> Get()
        {
            List<TblTenantHostAssignment> tenants = (from g in _context.TblTenantHostAssignment
                                            select g).ToList();
            return tenants;
        }

        /// <summary>
        /// Gets tenant-host assignment details based on identifier
        /// </summary>
        /// <param name="TenantHostId"></param>
        /// <returns></returns>
        // GET api/tenanthostassignments/5
        [HttpGet("{TenantHostId}")]
        [ProducesResponseType(typeof(TblTenantHostAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblTenantHostAssignment> Get(int TenantHostId)
        {
            List<TblTenantHostAssignment> buSite = (from g in _context.TblTenantHostAssignment
                                                   where g.TenantHostId == TenantHostId
                                                           select g).ToList();
            return buSite;
        }

        /// <summary>
        /// Gets tenant-host assignment details which are modified after Last Updated datetime
        /// </summary>
        // GET api/tenanthostassignments/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblTenantHostAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblTenantHostAssignment> Get(long LastUpdatedTime)
        {
            List<TblTenantHostAssignment> busite = (from g in _context.TblTenantHostAssignment
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return busite;
        }

       
    }
}

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
    public class TenantsController : Controller
    {
        private readonly DCTrackContext _context;

        public TenantsController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all Tenant details
        /// </summary>
        /// <returns></returns>
        // GET: api/Tenants
        [HttpGet]
        [ProducesResponseType(typeof(TblTenant), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblTenant> Get()
        {
            List<TblTenant> tenants = (from g in _context.TblTenant
                                            select g).ToList();
            return tenants;
        }

        /// <summary>
        /// Gets tenant details based on identifier
        /// </summary>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        // GET api/tenans/5
        [HttpGet("{TenantId}")]
        [ProducesResponseType(typeof(TblTenant), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblTenant> Get(int TenantId)
        {
            List<TblTenant> buSite = (from g in _context.TblTenant
                                                   where g.TenantId == TenantId
                                      select g).ToList();
            return buSite;
        }

        /// <summary>
        /// Gets tenant details which are modified after Last Updated datetime
        /// </summary>
        // GET api/tenants/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblTenant), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblTenant> Get(long LastUpdatedTime)
        {
            List<TblTenant> busite = (from g in _context.TblTenant
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return busite;
        }

       
    }
}

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
    public class AssetGroupsController : Controller
    {
        private readonly DCTrackContext _context;

        public AssetGroupsController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all asset group definitions
        /// </summary>
        /// <returns></returns>
        // GET: api/assetgroups
        [HttpGet]
        [ProducesResponseType(typeof(TblAssetGroup), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblAssetGroup> Get()
        {
            List<TblAssetGroup> assetGroups = (from g in _context.TblAssetGroup
                                              select g).ToList();
            return assetGroups;
        }

        /// <summary>
        /// Gets asset group definition based on identifier
        /// </summary>
        /// <param name="AssetGroupId"></param>
        /// <returns></returns>
        // GET api/assetgroups/5
        [HttpGet("{AssetGroupId}")]
        [ProducesResponseType(typeof(TblAssetGroup), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblAssetGroup> Get(int AssetGroupId)
        {
            List<TblAssetGroup> assetGroups = (from g in _context.TblAssetGroup
                                               where g.AssetGroupId == AssetGroupId
                                               select g).ToList();
            return assetGroups;
        }

        /// <summary>
        /// Gets asset group definitions which are modified after Last Updated datetime
        /// </summary>
        // GET api/assetgroups/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblAssetGroup), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblAssetGroup> Get(long LastUpdatedTime)
        {
            List<TblAssetGroup> assetgroups = (from g in _context.TblAssetGroup
                                               where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return assetgroups;
        }

        //// POST api/assetgroups
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// DELETE api/assetgroups/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

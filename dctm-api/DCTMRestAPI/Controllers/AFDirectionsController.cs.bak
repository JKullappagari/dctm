using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DCTMRestAPI.Models;

namespace DCTMRestAPI.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]")]
    public class AFDirectionsController : Controller
    {
        private readonly DCTrackContext _context;

        public AFDirectionsController(DCTrackContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all air flow directions details
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TblAirFlowDirection>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblAirFlowDirection>> Get() =>
            await _context.TblAirFlowDirection.AsNoTracking().ToListAsync();

        /// <summary>
        /// Gets air flow directions details based on identifier
        /// </summary>
        // GET api/AFDirections/5
        [HttpGet("{ID}")]
        [ProducesResponseType(typeof(IEnumerable<TblAirFlowDirection>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblAirFlowDirection>> Get(int ID) =>
            await _context.TblAirFlowDirection.AsNoTracking()
                .Where(g => g.Id == ID)
                .ToListAsync();

        /// <summary>
        /// Gets air flow directions details which are modified after Last Updated datetime
        /// </summary>
        // GET api/AFDirections/updated/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(IEnumerable<TblAirFlowDirection>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblAirFlowDirection>> Get(long LastUpdatedTime) =>
            await _context.TblAirFlowDirection.AsNoTracking()
                .Where(g => g.LastUpdatedTime > LastUpdatedTime)
                .ToListAsync();
    }
}

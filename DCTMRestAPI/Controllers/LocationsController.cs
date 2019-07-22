using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DCTMRestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using DCTMRestAPI.Models.Custom;
using Microsoft.AspNetCore.JsonPatch.Operations;
using DCTMRestAPI.Extensions;
using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DCTMRestAPI.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]")]
    public class LocationsController : Controller
    {

        private readonly DCTrackContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public LocationsController(DCTrackContext context, IMapper mapper, ILogger<LocationsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            //_userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        }

        // GET: api/locations
        /// <summary>
        /// Gets all locations details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TblLocation), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblLocation> Get()
        {
            List<TblLocation> locations = (from g in _context.TblLocation
                                           select g).ToList();
            return locations;
        }

        /// <summary>
        /// Gets location details based on location identifier
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        // GET api/locations/5
        [HttpGet("{LocationId}")]
        [ProducesResponseType(typeof(TblLocation), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblLocation> Get(int LocationId)
        {
            List<TblLocation> locations = (from g in _context.TblLocation
                                           where g.LocationId == LocationId
                                           select g).ToList();
            return locations;
        }

        /// <summary>
        /// Gets locations details which are modified after Last Updated datetime
        /// </summary>
        // GET api/AFDirections/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblLocation), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblLocation> Get(long LastUpdatedTime)
        {
            List<TblLocation> locations = (from g in _context.TblLocation
                                           where g.LastUpdatedTime > LastUpdatedTime
                                           select g).ToList();
            return locations;
        }

        /// <summary>
        /// Set location information based on location identifier
        /// </summary>
        /// <response code="200" >No reponse was specified</response>
        /// <response code="204" >No content</response>
        /// <param name="value">Location list</param>
        //PUT api/locations/5
        [HttpPut()]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Authorize(Roles = "Mobile")]
        public async Task<IActionResult> Put([FromBody]List<TblLocation> value)
        {
            List<LocationsFailed> errors = new List<LocationsFailed>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (TblLocation loc in value)
            {
                try
                {
                    // check last modified date and see whether Server or client has latest record.
                    // latest record will previal
                    TblLocation selectedLoc = (from g in _context.TblLocation.AsNoTracking()
                                               where g.LocationId == loc.LocationId
                                               select g).First();
                    if (selectedLoc.LastModifiedDate == null || selectedLoc.LastModifiedDate <= loc.LastModifiedDate)
                    {
                        _context.Entry(loc).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        LocationsFailed failed = new LocationsFailed();
                        failed.LocationId = loc.LocationId.ToString();
                        failed.ErrorMessage = "Server Record is latest compared to client record.";
                        errors.Add(failed);

                        //write to sync conflict log
                        StringBuilder sb = new StringBuilder();
                        sb.Append(Environment.NewLine);
                        sb.Append("******************Location Data Conflict******************");
                        sb.Append(Environment.NewLine);
                        sb.Append("Server Record is latest compared to client record.");
                        sb.Append(Environment.NewLine);
                        sb.Append("Location ID:" + selectedLoc.LocationId.ToString());
                        sb.Append(Environment.NewLine);
                        sb.Append("Server Modified Date:" + selectedLoc.LastModifiedDate.ToString());
                        sb.Append(Environment.NewLine);
                        sb.Append("Client Modified Date:" + loc.LastModifiedDate.ToString());
                        sb.Append(Environment.NewLine);
                        sb.Append("******************************************************");
                        sb.Append(Environment.NewLine);
                        _logger.LogInformation(sb.ToString());
                        //Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(sb.ToString(), "ConflictLog");
                    }
                }
                catch (DbUpdateConcurrencyException ex1)
                {
                    _logger.LogCritical(ex1, "Put Request");

                    LocationsFailed failed = new LocationsFailed();
                    failed.LocationId = loc.LocationId.ToString();
                    failed.ErrorMessage = ex1.Message;
                    errors.Add(failed);

                }
                catch (Exception ex2)
                {
                    _logger.LogCritical(ex2, "Put Request");
                    LocationsFailed failed = new LocationsFailed();
                    failed.LocationId = loc.LocationId.ToString();

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



        /// <summary>
        /// Partial update of a location 
        /// </summary>
        /// <remarks>
        /// This operation is based on the JSON Patch specification (RFC 6902) http://jsonpatch.com
        /// 
        /// This will allow Replace operation and applicable for below mentioned properties only
        /// 
        /// Location, ParentLocationID, TagID, SerialNumber and ExternalID 
        /// 
        /// Example:
        /// 
        ///     Patch /api/Locations/{LocationId}
        ///     
        ///         {
        ///         
        ///             "value": "TestSerialNo",
        ///             
        ///             "path": "/serialnumber",
        ///             
        ///             "op": "replace"
        ///             
        ///         },
        ///         
        ///         {
        ///         
        ///             ...
        ///             
        ///         }
        ///         
        /// 
        /// </remarks>
        /// <param name="LocationId"></param>
        /// <param name="patchLocation"></param>
        /// <returns></returns>
        [HttpPatch("{LocationId:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> LocationPatch(int LocationId, [FromBody] JsonPatchDocument<LocationDTO> patchLocation)
        {
            //Only repalce operation is allowed
            foreach (Operation o in patchLocation.Operations)
            {
                if (o.OperationType.ToString().ToLower().CompareTo("replace") != 0)
                {
                    return BadRequest("Only Replace operation is allowed");
                }
            }

            //check if non editable fields has values, if so throw error message
            // non editable fields: Description,Status,IsExitDoor,LocationTypeId
            //FloorNo ,CreatedDate ,CreatedBy ,LastModifiedDate ,LastModifiedBy ,IpAddress ,ParentLocationId 
            //IsCheckOutLocation ,Manufacturer,Model,IsSpecialRoom,ModelId ,Uheight,LastUpdatedTime 
            string[] allowedPaths = { "location", "parentlocationid", "tagid", "serialnumber", "externalid" };

            foreach (Operation o in patchLocation.Operations)
            {
                if (!allowedPaths.Contains(o.path.Replace("/", "").ToLower()))
                {
                    return BadRequest("Only Location, ParentLocationID, TagID, SerialNumber and ExternalID update is allowed");
                }
            }


            //check if Location name is in use.
            var location = (from l in _context.TblLocation.AsEnumerable()
                            where l.LocationId == LocationId
                            select l).First();

            foreach (Operation o in patchLocation.Operations)
            {
                if (o.path.Replace("/", "").ToLower().CompareTo("location") == 0)
                {
                    string query = " Exec iAssetTrack_Sp_Location_DoesExist " + LocationId.ToString() + ","
                        + "'" + o.value.ToString() + "'," + location.ParentLocationId.ToString();
                    DataTable dt = DbExtensions.ExecDataSet(query, _context);

                    bool locExists = ((int.Parse(dt.Rows[0][0].ToString())) >= 0 ? true : false);

                    if (!locExists)
                        return BadRequest("Location name is already in use");

                    if (o.value.ToString().ToLower().Contains("dispose") || o.value.ToString().ToLower().Contains("decom"))
                    {
                        //location with key words like dispose or decom is not allowed
                        return BadRequest("Location name with dispose or decom keywords is not allowed");
                    }

                }
            }

            //check if location hierarchy is correct based on selected parentlocationid
            //example: a room's parent location can't be a rack
            int locTypeID = 0;
            int parentlocID = 0;

            foreach (Operation o in patchLocation.Operations)
            {
                if (o.path.Replace("/", "").ToLower().CompareTo("parentlocationid") == 0)
                {
                    if (location.LocationTypeId.HasValue)
                        locTypeID = location.LocationTypeId.Value;
                    else
                        locTypeID = 0;
                    if (location.ParentLocationId.HasValue)
                        parentlocID = location.ParentLocationId.Value;
                    else
                        parentlocID = 0;

                    List<TblLocationType> locTypes = (from lt in _context.TblLocationType
                                                      select lt).ToList();

                    string curlocType = (string)(from lt in locTypes.AsEnumerable()
                                                 where lt.LocationTypeId == location.LocationTypeId
                                                 select lt.LocationType.First());


                    switch (curlocType)
                    {
                        case "Room":
                            if (parentlocID > 0)
                            {
                                return BadRequest("Parent location not allowed for room");
                            }
                            break;
                        case "Row":
                            if (parentlocID == 0)
                            {
                                return BadRequest("Row must contain a parent location");
                            }
                            else
                            {
                                var parentLocation = (from l in _context.TblLocation
                                                      where l.LocationId == parentlocID
                                                      select l).First();
                                string parentlocType = (string)(from lt in locTypes
                                                                where lt.LocationTypeId == parentLocation.LocationTypeId
                                                                select lt.LocationType.First());

                                if (parentlocType.CompareTo("Room") != 0)
                                {
                                    return BadRequest("Parent location for a row must be a room");
                                }
                            }
                            break;
                        case "Rack":
                            if (parentlocID == 0)
                            {
                                return BadRequest("Rack must contain a parent location");
                            }
                            else
                            {
                                var parentLocation = (from l in _context.TblLocation
                                                      where l.LocationId == parentlocID
                                                      select l).First();
                                string parentlocType = (string)(from lt in locTypes
                                                                where lt.LocationTypeId == parentLocation.LocationTypeId
                                                                select lt.LocationType.First());
                                if (parentlocType.CompareTo("Rack") == 0)
                                {
                                    return BadRequest("Parent location for a rack must be a room or a row");
                                }
                            }
                            break;
                    }
                }
            }

            // check if tagid is unique or not
            foreach (Operation o in patchLocation.Operations)
            {
                bool tagExists = false;
                if (o.path.Replace("/", "").ToLower().CompareTo("tagid") == 0)
                {
                    tagExists = ((from l in _context.TblLocation.AsEnumerable()
                                  where l.LocationId != LocationId &&
                                  l.TagId != null &&
                                  l.TagId.ToLower().CompareTo(o.value.ToString().ToLower()) == 0
                                  select l).Count() > 0 ? true : false);
                }

                if (tagExists)
                {
                    return BadRequest("Rack tag already in use");
                }
            }
            // check if rack serial number is unique or not
            foreach (Operation o in patchLocation.Operations)
            {
                bool snoExists = false;
                if (o.path.Replace("/", "").ToLower().CompareTo("serialnumber") == 0)
                {
                    snoExists = ((from l in _context.TblLocation
                                  where l.LocationId != LocationId &&
                                  l.SerialNumber != null &&
                                  l.SerialNumber.ToLower().CompareTo(o.value.ToString().ToLower()) == 0
                                  select l).Count() > 0 ? true : false);
                }

                if (snoExists)
                {
                    return BadRequest("Rack serial number already in use");
                }
            }
            //do update

            var locDTO = _mapper.Map<LocationDTO>(location);
            patchLocation.ApplyTo(locDTO);
            //update LastMofidiedBy and LastModifiedDate fields.
            string userName = User.Identity.Name;
            TblUser user = (from u in _context.TblUser
                            where u.LoginName.ToLower().CompareTo(userName.ToLower()) == 0
                            select u).First();

            locDTO.LastModifiedDate = DateTime.Now;
            locDTO.LastModifiedBy = user.UserId;
            //
            _mapper.Map(locDTO, location);
            _context.Entry(location).State = EntityState.Modified;
            _context.TblLocation.Update(location);
            await _context.SaveChangesAsync();
            //string userID = _userId;

            return Ok(location);
        }
    }

    public class LocationsFailed
    {

        public string LocationId { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

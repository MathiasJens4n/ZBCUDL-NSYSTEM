using LoanSystemApiMini.Data.IRepositories;
using LoanSystemApiMini.Log;
using LoanSystemLibraryMini;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanSystemApiMini.Controllers
{
    [ApiController]
    [Route("api/types")]
    public class TypeController : ControllerBase
    {
        private readonly IRepo<Type> _typeRepo;
        private ILoggerManager _logger;

        public TypeController(IRepo<Type> typeRepo, ILoggerManager logger)
        {
            _typeRepo = typeRepo;
            _logger = logger;
        }

        // GET: api/types
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Type>>> GetAllTypes()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                List<Type> types = new List<Type>();
                types = (await _typeRepo.GetAll()).ToList();
                if (types == null)
                {
                    _logger.LogError($"GetAllTypes endpoint: Could not find. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetAllTypes endpoint: Successfuly Get. - IP: {remoteIpAddress}");
                return Ok(types);
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("GetAllTypes endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/types/{id}
        [HttpGet]
        [Route("{id}", Name = "GetType")]
        public async Task<ActionResult<Type>> GetType(int id)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                Type type = await _typeRepo.Get(id);
                if (type == null)
                {
                    _logger.LogError($"GetType endpoint: Could not find type ID: {id}. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetType endpoint: Successfuly Get type ID: {id}. - IP: {remoteIpAddress}");
                return Ok(type);
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("GetType endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/types
        [HttpPost]
        public async Task<ActionResult<Type>> PostType(Type type)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                int id = await _typeRepo.Create(type);
                type.Id = id;

                _logger.LogInfo($"PostType endpoint: Successfuly Post type ID: {id}. - IP: {remoteIpAddress}");
                return CreatedAtRoute("GetType", new { id = type.Id }, type);
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("PostType endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/types
        [HttpPut]
        public async Task<ActionResult> PutType(Type type)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                Type existType = await _typeRepo.Get(type.Id);
                if (existType == null)
                {
                    _logger.LogError($"PutType endpoint: Could not find type ID: {type.Id}. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                await _typeRepo.Update(type);
                _logger.LogInfo($"PutType endpoint: Successfuly Put type ID: {type.Id}. - IP: {remoteIpAddress}");
                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug("PutType endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
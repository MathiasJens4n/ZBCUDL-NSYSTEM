using LoanSystemApiMini.Data.IRepositories;
using LoanSystemApiMini.Log;
using LoanSystemLibraryMini;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanSystemApiMini.Controllers
{
    [ApiController]
    [Route("api/statuses")]
    public class StatusController : ControllerBase
    {
        private readonly IBaseRepo<Status> _statusRepo;
        private ILoggerManager _logger;

        public StatusController(IBaseRepo<Status> statusRepo, ILoggerManager logger)
        {
            _statusRepo = statusRepo;
            _logger = logger;
        }

        // GET: api/statuses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Status>>> GetAllStatuses()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                List<Status> statuses = new List<Status>();
                statuses = (await _statusRepo.GetAll()).ToList();
                if (statuses == null)
                {
                    _logger.LogError($"GetAllStatuses endpoint: Could not find. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetAllStatuses endpoint: Successfuly Get. - IP: {remoteIpAddress}");
                return Ok(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("GetAllStatuses endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
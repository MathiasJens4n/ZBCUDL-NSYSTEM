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
    [Route("api/departments")]
    public class DepartmentController : ControllerBase
    {
        private readonly IBaseRepo<Department> _departmentRepo;
        private ILoggerManager _logger;

        public DepartmentController(IBaseRepo<Department> departmentRepo, ILoggerManager logger)
        {
            _departmentRepo = departmentRepo;
            _logger = logger;
        }

        // GET: api/departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                List<Department> departments = new List<Department>();
                departments = (await _departmentRepo.GetAll()).ToList();
                if (departments == null)
                {
                    _logger.LogError($"GetAllDepartments endpoint: Could not find. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetAllDepartments endpoint: Successfuly Get. - IP: {remoteIpAddress}");
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("GetAllDepartments endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}

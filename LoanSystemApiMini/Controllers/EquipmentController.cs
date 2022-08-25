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
    [Route("api/equipments")]
    public class EquipmentController : ControllerBase
    {
        private readonly IRepo<Equipment> _equipmentRepo;
        private ILoggerManager _logger;

        public EquipmentController(IRepo<Equipment> equipmentRepo, ILoggerManager logger)
        {
            _equipmentRepo = equipmentRepo; 
            _logger = logger;
        }

        // GET: api/equipments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipment>>> GetAllEquipments()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                List<Equipment> equipments = new List<Equipment>();
                equipments = (await _equipmentRepo.GetAll()).ToList();
                if (equipments == null)
                {
                    _logger.LogError($"GetAllEquipments endpoint: Could not find. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetAllEquipments endpoint: Successfuly Get. - IP: {remoteIpAddress}");
                return Ok(equipments);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("GetAllEquipments endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/equipments/{id}
        [HttpGet]
        [Route("{id}", Name = "GetEquipment")]
        public async Task<ActionResult<Equipment>> GetEquipment(int id)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                Equipment equipment = await _equipmentRepo.Get(id);
                if (equipment == null)
                {
                    _logger.LogError($"GetEquipment endpoint: Could not find equipment ID: {id}. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetEquipment endpoint: Successfuly Get equipment ID: {id}. - IP: {remoteIpAddress}");
                return Ok(equipment);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("GetEquipment endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/equipments
        [HttpPost]
        public async Task<ActionResult<Equipment>> PostEquipment(Equipment equipment)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                int id = await _equipmentRepo.Create(equipment);
                equipment.Id = id;

                _logger.LogInfo($"PostEquipment endpoint: Successfuly Post equipment ID: {id}. - IP: {remoteIpAddress}");
                return CreatedAtRoute("GetEquipment", new { id = equipment.Identification }, equipment);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("PostEquipment endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/equipments
        [HttpPut]
        public async Task<ActionResult> PutEquipment(Equipment equipment)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                Equipment existEquipment = await _equipmentRepo.Get(equipment.Id);
                if (existEquipment == null)
                {
                    _logger.LogError($"PutEquipment endpoint: Could not find equipment ID: {equipment.Id}. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                await _equipmentRepo.Update(equipment);
                _logger.LogInfo($"PutEquipment endpoint: Successfuly Put equipment ID: {equipment.Id}. - IP: {remoteIpAddress}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogDebug("PutEquipment endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
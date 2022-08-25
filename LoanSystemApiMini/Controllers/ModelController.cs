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
    [Route("api/models")]
    public class ModelController : ControllerBase
    {
        private readonly IRepo<Model> _modelRepo;
        private ILoggerManager _logger;

        public ModelController(IRepo<Model> modelRepo, ILoggerManager logger)
        {
            _modelRepo = modelRepo;
            _logger = logger;
        }

        // GET: api/models
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Model>>> GetAllModels()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                List<Model> models = new List<Model>();
                models = (await _modelRepo.GetAll()).ToList();
                if (models == null)
                {
                    _logger.LogError($"GetAllmodels endpoint: Could not find. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetAllmodels endpoint: Successfuly Get. - IP: {remoteIpAddress}");
                return Ok(models);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("GetAllmodels endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/models/{id}
        [HttpGet]
        [Route("{id}", Name = "getmodel")]
        public async Task<ActionResult<Model>> GetModel(int id)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                Model model = await _modelRepo.Get(id);
                if (model == null)
                {
                    _logger.LogError($"GetModel endpoint: Could not find model ID: {id}. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetModel endpoint: Successfuly Get nodel ID: {id}. - IP: {remoteIpAddress}");
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("GetModel endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/models
        [HttpPost]
        public async Task<ActionResult<Model>> PostModel(Model model)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                int id = await _modelRepo.Create(model);
                model.Id = id;

                _logger.LogInfo($"PostModel endpoint: Successfuly Post model ID: {id}. - IP: {remoteIpAddress}");
                return CreatedAtRoute("getmodel", new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("PostModel endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/models
        [HttpPut]
        public async Task<ActionResult> PutModel(Model model)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                Model exsitModel = await _modelRepo.Get(model.Id);
                if (model == null)
                {
                    _logger.LogError($"PutModel endpoint: Could not find model ID: {model.Id}. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                await _modelRepo.Update(model);
                _logger.LogInfo($"PutModel endpoint: Successfuly Put nodel ID: {model.Id}. - IP: {remoteIpAddress}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogDebug("PutModel endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
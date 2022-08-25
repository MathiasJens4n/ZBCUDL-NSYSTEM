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
    [Route("api/loanrules")]
    public class LoanRuleController : ControllerBase
    {
        private readonly IRepo<LoanRule> _loanRuleRepo;
        private ILoggerManager _logger;

        public LoanRuleController(IRepo<LoanRule> loanRuleRepo, ILoggerManager logger)
        {
            _loanRuleRepo = loanRuleRepo;
            _logger = logger;
        }

        // GET: api/loanrules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanRule>>> GetAllLoanRules()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                List<LoanRule> loanRules = new List<LoanRule>();
                loanRules = (await _loanRuleRepo.GetAll()).ToList();
                if (loanRules == null)
                {
                    _logger.LogError($"GetAllLoanRules endpoint: Could not find. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetAllLoanRules endpoint: Successfuly Get. - IP: {remoteIpAddress}");
                return Ok(loanRules);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("GetAllLoanRules endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/loanrules/{id}
        [HttpGet]
        [Route("{id}", Name = "GetLoanRule")]
        public async Task<ActionResult<LoanRule>> GetLoanRule(int id)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                LoanRule loanRule = await _loanRuleRepo.Get(id);
                if (loanRule == null)
                {
                    _logger.LogError($"GetLoanRule endpoint: Could not find LoanRule ID: {id}. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetLoanRule endpoint: Successfuly Get LoanRule ID: {id}. - IP: {remoteIpAddress}");
                return Ok(loanRule);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("GetLoanRule endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/loanrules
        [HttpPost]
        public async Task<ActionResult<LoanRule>> PostLoanRule(LoanRule loanRule)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                int id = await _loanRuleRepo.Create(loanRule);
                loanRule.Id = id;

                _logger.LogInfo($"PutLoanRule endpoint: Successfuly Post loanRule ID: {id}. - IP: {remoteIpAddress}");
                return CreatedAtRoute("GetLoanRule", new { Id = loanRule.Id }, loanRule);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("PutEquipment endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/loanrules
        [HttpPut]
        public async Task<ActionResult> PutLoanRule(LoanRule loanRule)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                LoanRule existLoanRule = await _loanRuleRepo.Get(loanRule.Id);
                if (existLoanRule == null)
                {
                    _logger.LogError($"PutLoanRule endpoint: Could not find loanRule ID: {loanRule.Id}. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                await _loanRuleRepo.Update(loanRule);
                _logger.LogInfo($"PutLoanRule endpoint: Successfuly Put loanRule ID: {loanRule.Id}. - IP: {remoteIpAddress}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogDebug("PutLoanRule endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
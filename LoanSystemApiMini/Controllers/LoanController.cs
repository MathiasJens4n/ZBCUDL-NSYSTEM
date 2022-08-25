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
    [Route("api/loans")]
    public class LoanController : ControllerBase
    {
        private readonly IRepo<Loan> _loanRepo;
        private ILoggerManager _logger;

        public LoanController(IRepo<Loan> loanRepo, ILoggerManager logger)
        {
            _loanRepo = loanRepo;
            _logger = logger;
        }

        // GET: api/loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetAllLoans()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                List<Loan> loans = new List<Loan>();
                loans = (await _loanRepo.GetAll()).ToList();
                if (loans == null)
                {
                    _logger.LogError($"GetAllLoans endpoint: Could not find. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetAllLoans endpoint: Successfuly Get. - IP: {remoteIpAddress}");
                return Ok(loans);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("GetAllLoans endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/loans/{id}
        [HttpGet]
        [Route("{id}", Name = "GetLoan")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                Loan loan = await _loanRepo.Get(id);
                if (loan == null)
                {
                    _logger.LogError($"GetLoan endpoint: Could not return loan ID: {id}. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetLoan endpoint: Successfuly Get loan ID: {id}. - IP: {remoteIpAddress}");
                return Ok(loan);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("GetLoan endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/loans
        [HttpPost]
        public async Task<ActionResult<Loan>> PostLoan(Loan loan)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                int id = await _loanRepo.Create(loan);
                loan.Id = id;

                _logger.LogInfo($"PostLoan endpoint: Successfuly Post loan ID: {id}. - IP: {remoteIpAddress}");
                return CreatedAtRoute("GetLoan", new { id = loan.Id }, loan);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("PostLoan endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/loans
        [HttpPut]
        public async Task<ActionResult> PutLoan(Loan loan)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                Loan existLoan = await _loanRepo.Get(loan.Id);
                if (existLoan == null)
                {
                    _logger.LogError($"PutLoan endpoint: Could not find loan ID: {loan.Id}. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                await _loanRepo.Update(loan);
                _logger.LogInfo($"PutLoan endpoint: Successfuly put ID: {loan.Id}. - IP: {remoteIpAddress}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogDebug("PutLoan endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
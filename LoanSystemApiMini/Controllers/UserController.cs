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
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IRepo<User> _userRepo;
        private ILoggerManager _logger;

        public UserController(IRepo<User> userRepo, ILoggerManager logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                List<User> users = new List<User>();
                users = (await _userRepo.GetAll()).ToList();
                if (users == null)
                {
                    _logger.LogError($"GetAllUsers endpoint: Could not find. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetAllUsers endpoint: Successfuly Get. - IP: {remoteIpAddress}");
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("GetEquipment endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Users/{id}
        [HttpGet]
        [Route("{id}", Name = "GetUser")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                User user = await _userRepo.Get(id);
                if (user == null)
                {
                    _logger.LogError($"GetUser endpoint: Could not find user ID: {id}. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                _logger.LogInfo($"GetUser endpoint: Successfuly Get user ID: {id}. - IP: {remoteIpAddress}");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("GetUser endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                int id = await _userRepo.Create(user);
                user.Id = id;

                _logger.LogInfo($"PostUser endpoint: Successfuly Post user ID: {id}. - IP: {remoteIpAddress}");
                return CreatedAtRoute("GetUser", new { id = user.UniLogin }, user);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("PostUser endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Users
        [HttpPut]
        public async Task<ActionResult> PutUser(User user)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            try
            {
                User existUser = await _userRepo.Get(user.Id);
                if (existUser == null)
                {
                    _logger.LogError($"PutUser endpoint: Could not find user ID: {user.Id}. - IP: {remoteIpAddress}");
                    return NotFound();
                }

                await _userRepo.Update(user);
                _logger.LogInfo($"PutUser endpoint: Successfuly Put user ID: {user.Id}. - IP: {remoteIpAddress}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogDebug("PutUser endpoint: " + ex.Message + $"- IP: {remoteIpAddress}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
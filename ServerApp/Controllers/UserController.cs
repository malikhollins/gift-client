using Microsoft.AspNetCore.Mvc;
using ServerApp.Models;
using ServerApp.Services;

namespace ServerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController( UserService userService ) 
        {
            _userService = userService;
        }

        [HttpGet("create/{authId}/{email}")]
        public async Task<ActionResult<User>> CreateUser(string authId, string email)
        {
            var user = await _userService.CreateUser(authId, email);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("get/{authId}")]
        public async Task<ActionResult<User>> GetUser(string authId)
        {
            var user = await _userService.GetUser(authId);
            return user == null ? NotFound() : Ok(user);        }

        [HttpGet("get/bulk/{searchTerm}")]
        public async Task<ActionResult<IReadOnlyList<User?>>> BulkGetUser(string searchTerm )
        {
            var bulkList = await _userService.BulkGetUser(searchTerm, string.Empty);
            return Ok(bulkList);
        }

        [HttpGet("get/invites/{userId}")]
        public async Task<ActionResult<IReadOnlyList<Invite>>> GetInvites(int userId)
        {
            var invite = await _userService.GetUserInvites(userId);
            return Ok(invite);
        }
    }
}

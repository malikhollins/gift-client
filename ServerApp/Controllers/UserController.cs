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

        [HttpGet("get/{authId}/{email}")]
        public async Task<ActionResult<User>> RegisterUserAsync(string authId, string email)
        {
            var user = await _userService.GetUserAsync(authId);
            if (user == null)
            {
                user = await _userService.CreateUserAsync(authId, email);
            }
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("get/bulk/{searchTerm}")]
        public async Task<ActionResult<IReadOnlyList<User?>>> BulkGetUserAsync(string searchTerm )
        {
            var bulkList = await _userService.BulkGetUserAsync(searchTerm, string.Empty);
            return Ok(bulkList);
        }
    }
}

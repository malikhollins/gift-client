using Microsoft.AspNetCore.Mvc;
using Models;
using ServerApp.Models;
using ServerApp.Services;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private readonly HouseService _houseService;
        private readonly InviteService _inviteService;

        public HouseController(HouseService houseService, InviteService inviteService)
        {
            _houseService = houseService;
            _inviteService = inviteService;
        }

        [HttpGet("get/{userId}")]
        public async Task<ActionResult<IEnumerable<House>>> GetHousesAsync( int userId )
        {
            var result = await _houseService.GetHousesAsync(userId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<House>> CreateHouseAsync( CreateHouseRequest request )
        {
            var createdHouse = await _houseService.CreateHouseAsync(request.UserId, request.Name);

            if (request.InvitedUsers != null && request.InvitedUsers.Count != 0)
            {
                foreach (var userId in request.InvitedUsers)
                {
                    await _inviteService.CreateInvite(createdHouse.Id, userId);
                }
            }

            return Ok(createdHouse);
        }
    }
}

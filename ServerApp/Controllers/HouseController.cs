using Microsoft.AspNetCore.Mvc;
using ServerApp.Models;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private readonly HouseService _houseService;

        public HouseController(HouseService houseService )
        {
            _houseService = houseService;
        }

        [HttpGet("get/{userId}")]
        public async Task<ActionResult<IEnumerable<House>>> GetHousesAsync( int userId )
        {
            var result = await _houseService.GetHousesAsync(userId);
            return Ok(result);
        }

        [HttpGet("create/{userId}/{name}")]
        public async Task<ActionResult<House>> CreateHouseAsync(int userId, string name)
        {
            var createdHouse = await _houseService.CreateHouseAsync(userId, name);
            return Ok(createdHouse);
        }

        [HttpGet("create/invite/{houseId}/{userIdToInvite}")]
        public async Task<ActionResult> CreateHouseInviteAsync(int houseId, int userIdToInvite)
        {
            await _houseService.CreateHouseInviteAsync(houseId, userIdToInvite);
            return Ok();
        }


        [HttpGet("set/invite/{userId}/{houseId}/{status}")]
        public async Task<ActionResult> UpdateHouseInviteStatusAsync(int userId, int houseId, InviteStatus status)
        {
            await _houseService.UpdateHouseInviteStatusAsync(userId, houseId, status);
            return Ok();
        }
    }
}

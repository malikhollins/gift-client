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
    }
}

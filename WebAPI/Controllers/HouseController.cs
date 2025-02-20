using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{userId}")]
        public async Task<IEnumerable<House>> GetHouses( int userId )
        {
            return await _houseService.GetHousesAsync( userId );
        }
    }
}

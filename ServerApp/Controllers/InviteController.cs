using Microsoft.AspNetCore.Mvc;
using ServerApp.Services;

namespace ServerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InviteController : Controller
    {
        private readonly InviteService _inviteService;

        public InviteController(InviteService inviteService)
        {
            _inviteService = inviteService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateInvite(int houseId, int userId)
        {
            await _inviteService.CreateInvite(houseId, userId);
            return Ok();
        }
    }
}

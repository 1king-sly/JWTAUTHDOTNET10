using JWTAUTHDOTNET10.DTOs;
using JWTAUTHDOTNET10.Services;
using Microsoft.AspNetCore.Mvc;

namespace JWTAUTHDOTNET10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<UserOut>>> GetUsers()
        {
            var users = await service.GetUsers();

            return Ok(users);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserOut>> GetUser(string id) {
            var user = await service.GetUser(id);

            if (user == null) {
                return NotFound("User not found");
            }
            return Ok(user);
        }

    }
}

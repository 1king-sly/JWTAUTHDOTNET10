using JWTAUTHDOTNET10.DTOs;
using JWTAUTHDOTNET10.Models;
using JWTAUTHDOTNET10.Services;
using Microsoft.AspNetCore.Mvc;

namespace JWTAUTHDOTNET10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        public static User user = new();


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto userIn)
        {
            var user = await authService.RegisterUserAsync(userIn);
            if (user == null)
            {
                return BadRequest("Invalid credentials");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserOut>> Login(UserDto userIn)
        {
            var user = await authService.LoginUserAsync(userIn);
            if (user == null)
            {
                return BadRequest("Invalid email or password");
            }
            return Ok(user);
        }




       
    }
}

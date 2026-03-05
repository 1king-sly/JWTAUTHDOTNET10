using JWTAUTHDOTNET10.DTOs;
using JWTAUTHDOTNET10.Models;
using JWTAUTHDOTNET10.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAUTHDOTNET10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {


        [HttpPost("register")]
        public async Task<ActionResult<UserOutLogin>> Register(UserDto userIn)
        {
            var user = await authService.RegisterUserAsync(userIn);
            if (user == null)
            {
                return BadRequest("Account with email already exists");
            }


            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserOutLogin>> Login(UserDto userIn)
        {
            var user = await authService.LoginUserAsync(userIn);
            if (user == null)
            {
                return BadRequest("Invalid email or password");
            }
            return Ok(user);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ProtectedRoute()
        {
            return Ok("You are authenticated");
        }




       
    }
}

using JWTAUTHDOTNET10.DTOs;
using JWTAUTHDOTNET10.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JWTAUTHDOTNET10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new();

        [HttpPost("register")]
        public ActionResult<string> Register(UserDto user)
        {
            return Ok(HashPassword(user.password));
        }

        private string HashPassword(string password)
        {
            var hashedPassword = new PasswordHasher<User>().HashPassword(user,password);
            return hashedPassword;
        }
    }
}

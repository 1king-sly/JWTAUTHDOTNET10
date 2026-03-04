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
        public ActionResult<User> Register(UserDto userIn)
        {
            var hashedPassword = HashPassword(userIn.Password);

            user.HashedPassword = hashedPassword;
            user.Email = userIn.Email;

            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<string> Login(UserDto userIn)
        {
            if (user == null)
            {
                return BadRequest("Missing values");
            }
            else if (!VerifyHashedPassword(user.HashedPassword, userIn.Password))
            {
                return Unauthorized("Invalid email or password");
            }
            else
            {
                return Ok("Login successful");
            }
        }




        private static string HashPassword(string password)
        {
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, password);
            return hashedPassword;
        }

        private static bool VerifyHashedPassword(string hashPassword, string password)
        {
            if (new PasswordHasher<User>().VerifyHashedPassword(user, hashPassword, password) == PasswordVerificationResult.Failed)
            {
                return false;


            }
            else
            {
                return true;
            }
        }
    }
}

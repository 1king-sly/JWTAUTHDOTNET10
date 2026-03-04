using JWTAUTHDOTNET10.DTOs;
using JWTAUTHDOTNET10.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAUTHDOTNET10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ControllerBase
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
        public ActionResult<UserOut> Login(UserDto userIn)
        {
            if (userIn == null)
            {
                return BadRequest("Missing values");
            }
            else if (!VerifyHashedPassword(user.HashedPassword, userIn.Password))
            {
                return Unauthorized("Invalid email or password");
            }
            else
            {
                return Ok(new UserOut(user.Email, GenerateJwtToken(user)));
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

        private  string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Email,user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    configuration.GetValue<string>("AppSettings:Token")!
                    )
                );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor =new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}

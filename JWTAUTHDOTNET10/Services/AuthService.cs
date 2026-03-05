using JWTAUTHDOTNET10.Data;
using JWTAUTHDOTNET10.DTOs;
using JWTAUTHDOTNET10.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAUTHDOTNET10.Services
{
    public class AuthService(IConfiguration configuration,LocalDbContext context) : IAuthService
    {

        
        public async Task<UserOutLogin?> RegisterUserAsync(UserDto userIn)

        {
            if (context.users.FirstOrDefault(u => u.Email == userIn.Email) is not null) { return null; }
         User user = new();


            var hashedPassword = HashPassword(userIn.Password);
            user.HashedPassword = hashedPassword;
            user.Email = userIn.Email;

            context.users.Add(user);
            return new UserOutLogin(user.Id, user.Email, GenerateJwtToken(user));
        }

        public async Task<UserOutLogin?> LoginUserAsync(UserDto userIn)
        {
            var user = context.users.FirstOrDefault(u => u.Email == userIn.Email);


            if (user is null || !VerifyHashedPassword(user!.HashedPassword, userIn.Password))
            {
                return null;
            }

            return new UserOutLogin(user.Id,user.Email, GenerateJwtToken(user));


        }

       

        private static string HashPassword(string password)
        {
            var hashedPassword = new PasswordHasher<User>().HashPassword(new(), password);
            return hashedPassword;
        }

        private static bool VerifyHashedPassword(string hashPassword, string password)
        {

            if (new PasswordHasher<User>().VerifyHashedPassword(new(), hashPassword, password) == PasswordVerificationResult.Failed)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>{
                new(ClaimTypes.Email,user.Email),
                new(ClaimTypes.NameIdentifier,user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    configuration.GetValue<string>("AppSettings:Token")!
                    )
                );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
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

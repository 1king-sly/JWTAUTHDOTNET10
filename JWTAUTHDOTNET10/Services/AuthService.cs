using JWTAUTHDOTNET10.Data;
using JWTAUTHDOTNET10.DTOs;
using JWTAUTHDOTNET10.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWTAUTHDOTNET10.Services
{
    public class AuthService(IConfiguration configuration, LocalDbContext context) : IAuthService
    {


        public async Task<UserOutLogin?> RegisterUserAsync(UserDto userIn)

        {
            if (context.users.FirstOrDefault(u => u.Email == userIn.Email) is not null) { return null; }
            User user = new();


            var hashedPassword = HashPassword(userIn.Password);
            user.HashedPassword = hashedPassword;
            user.Email = userIn.Email;

            if (context.users.Count == 0)
            {
                user.Role = "Admin";
            }

            context.users.Add(user);
            return await CreateUserOut(user);
        }

        public async Task<UserOutLogin?> LoginUserAsync(UserDto userIn)
        {
            var user = context.users.FirstOrDefault(u => u.Email == userIn.Email);


            if (user is null || !VerifyHashedPassword(user!.HashedPassword, userIn.Password))
            {
                return null;
            }

            return await CreateUserOut(user);


        }


        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest)
        {
            var user = await ValidateRefreshToken(refreshTokenRequest);

            if (user is null) { return null; };

            var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);

            var accessToken = GenerateJwtToken(user);

            return new TokenResponseDto(refreshToken, accessToken);
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

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();

            var userRefreshToken = context.refreshTokens.FirstOrDefault(r => r.UserId == user.Id);

            if (userRefreshToken == null)
            {
                RefreshToken newRefreshToken = new RefreshToken { Token = refreshToken, UserId = user.Id, TokenExpiry = DateTime.UtcNow.AddDays(7) };
                context.refreshTokens.Add(newRefreshToken);

                return newRefreshToken.Token;
            }

            userRefreshToken.Token = refreshToken;
            userRefreshToken.TokenExpiry = DateTime.UtcNow.AddDays(7);

            return userRefreshToken.Token;
        }

        private async Task<User?> ValidateRefreshToken(RefreshTokenRequestDto request)
        {
            var refreshToken = context.refreshTokens.FirstOrDefault(r => r.Token == request.RefreshToken);

            if (refreshToken is null || refreshToken.TokenExpiry < DateTime.UtcNow )
                return null;

            var user = context.users.Find(u =>u.Id.ToString() == request.userId);

            return user;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>{
                new(ClaimTypes.Email,user.Email),
                new(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new(ClaimTypes.Role,user.Role),
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
                expires: DateTime.UtcNow.AddSeconds(30),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private async Task<UserOutLogin> CreateUserOut(User user)
        {
            return new UserOutLogin(user.Id, user.Email, user.Role, GenerateJwtToken(user), await GenerateAndSaveRefreshTokenAsync(user));
        }

        
    }
}

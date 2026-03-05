using JWTAUTHDOTNET10.DTOs;
using JWTAUTHDOTNET10.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAUTHDOTNET10.Services
{
    public interface IAuthService
    {
        public Task<User?> RegisterUserAsync(UserDto userIn);

        public Task<UserOut?> LoginUserAsync(UserDto userIn); 


        
        

    }
}

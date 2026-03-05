using JWTAUTHDOTNET10.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JWTAUTHDOTNET10.Services
{
    public interface IUserService
    {
        Task<List<UserOut>> GetUsers();

        Task<UserOut?> GetUser(string userId);

    }
}

using JWTAUTHDOTNET10.Data;
using JWTAUTHDOTNET10.DTOs;


namespace JWTAUTHDOTNET10.Services
{
    public class UserService(LocalDbContext context) : IUserService
    {
        public async Task<UserOut?> GetUser(string userId)
        {
            var user = context.users.FirstOrDefault(u => u.Id.ToString() == userId);

            if (user == null) return null;

            return new UserOut(user.Id.ToString(), user.Email, user.Role, user.Created_At);
        }

        public async Task<List<UserOut>> GetUsers()
        {
            var users = new List<UserOut>();
            foreach (var user in context.users)
            {
                users.Add(new UserOut(user.Id.ToString(), user.Email, user.Role, user.Created_At));
            }
            return users;
        }
    }
}

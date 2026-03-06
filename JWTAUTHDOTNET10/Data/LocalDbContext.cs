using JWTAUTHDOTNET10.Models;

namespace JWTAUTHDOTNET10.Data
{
    public class LocalDbContext
    {
     public List<User> users = new();
     public List<RefreshToken> refreshTokens = new();

    }
}

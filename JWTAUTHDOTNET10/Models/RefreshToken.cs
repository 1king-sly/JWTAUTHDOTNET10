namespace JWTAUTHDOTNET10.Models
{
    public class RefreshToken
    {
        public required Guid UserId { get; set; }

        public required string Token { get; set; }

        public required DateTime TokenExpiry { get; set; }
    }
}

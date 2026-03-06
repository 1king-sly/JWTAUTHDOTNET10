namespace JWTAUTHDOTNET10.DTOs
{
    public class RefreshTokenRequestDto
    {
        public required string userId { get; set; }
        public required string RefreshToken { get; set; }
    }
}

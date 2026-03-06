namespace JWTAUTHDOTNET10.DTOs
{
    public class TokenResponseDto(string refreshToken, string accessToken)
    {
        public  string RefreshToken {  get; set; } = refreshToken;

        public  string AccessToken { get; set; }  = accessToken;  
    }
}

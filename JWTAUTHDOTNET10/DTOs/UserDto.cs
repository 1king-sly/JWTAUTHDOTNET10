using System.ComponentModel.DataAnnotations;

namespace JWTAUTHDOTNET10.DTOs
{
    public class UserDto
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public required string Email { get; set; }
        [Required(ErrorMessage ="Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public required string Password { get; set; }
    }

    public class UserOutLogin(Guid id, string email,string role, string access_Token)
    {
        public string Id { get; set; } = id.ToString();
        public string Email { get; set; } = email;

        public string Role { get; set; } = role;
        public string Access_Token { get; set; } = access_Token;
    }

    public class UserOut(string id, string email,string role, DateTime createdAt)
    {
        public string Id { get; set; } = id;
        public string Email { get; set; } = email;
        public string Role { get; set; } = role;


        public DateTime Created_At { get; set; } = createdAt;
    }
}

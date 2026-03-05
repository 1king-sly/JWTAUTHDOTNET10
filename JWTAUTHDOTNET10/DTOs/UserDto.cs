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

    public class UserOut
    {   
        public string Id { get; set; }
        public  string Email { get; set; }
        public  string Access_Token { get; set; }

        public UserOut(Guid id,string email, string access_Token)
        {
            Id = id.ToString();
            Email = email;
            Access_Token = access_Token;
        }
    }
}

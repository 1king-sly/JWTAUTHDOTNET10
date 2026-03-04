using System.ComponentModel.DataAnnotations;

namespace JWTAUTHDOTNET10.DTOs
{
    public class UserDto
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public required string Email { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public required string Password { get; set; }
    }
}

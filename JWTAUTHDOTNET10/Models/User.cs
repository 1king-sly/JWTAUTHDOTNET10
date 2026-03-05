namespace JWTAUTHDOTNET10.Models
{
    public class User
    {   
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public DateTime Created_At { get; set; } = DateTime.UtcNow;


    }
}

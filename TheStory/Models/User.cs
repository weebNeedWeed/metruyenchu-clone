namespace TheStory.Models
{
    public enum RoleEnum
    {
        Administrator,
        Member
    }

    public class User
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public RoleEnum? Role { get; set; }
    }
}

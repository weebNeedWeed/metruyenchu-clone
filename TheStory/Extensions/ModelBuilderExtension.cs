using Microsoft.EntityFrameworkCore;
using TheStory.Models;
using TheStory.Utilities;

namespace TheStory.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            string salt = PasswordHasher.GetSalt();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Email = "admin@gmail.com",
                    Password = PasswordHasher.Hash(salt: salt, rawPassword: "admin"),
                    Salt = salt,
                    Role = RoleEnum.Administrator,
                    UserName = "admin"
                });
        }
    }
}

using Microsoft.EntityFrameworkCore;
using TheStory.Models;
using TheStory.Data.Configurations;

namespace TheStory.Data
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ChapterConfiguration());
            builder.ApplyConfiguration(new BookConfiguration());
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Book>? Books { get; set; }
        public DbSet<Chapter>? Chapters { get; set; }
    }
}

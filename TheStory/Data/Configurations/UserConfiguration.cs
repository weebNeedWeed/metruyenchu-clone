using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheStory.Models;

namespace TheStory.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(o => o.UserId);

            builder.Property(o => o.Email)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(120);

            builder.Property(o => o.Password)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(120);

            builder.Property(o => o.UserName)
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnType("varchar");

            builder.Property(x => x.Salt)
                .IsRequired();

            // Email column is unique
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}

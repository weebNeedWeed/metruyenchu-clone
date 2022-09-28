using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheStory.Models;

namespace TheStory.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(o => o.BookId);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnType("nvarchar");

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnType("nvarchar");

            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(120);

            builder.HasOne(x => x.Genre)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.GenreId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

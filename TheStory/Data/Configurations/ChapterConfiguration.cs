using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheStory.Models;

namespace TheStory.Data.Configurations
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.HasKey(x => x.ChapterId);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnType("nvarchar");

            builder.Property(x => x.ChapterNumber)
                .IsRequired();

            builder.HasOne(x => x.Book)
                .WithMany(x => x.Chapters)
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}

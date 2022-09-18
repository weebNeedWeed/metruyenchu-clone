namespace TheStory.Models
{
    public class Chapter
    {
        public int ChapterId { get; set; }
        public string? Name { get; set; }
        public int ChapterNumber { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}

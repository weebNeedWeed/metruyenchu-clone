﻿namespace TheStory.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        public ICollection<Chapter>? Chapters { get; set; }
    }
}

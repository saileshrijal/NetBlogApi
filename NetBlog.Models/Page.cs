namespace NetBlog.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? Slug { get; set; }
        public string? thumbnailUrl { get; set; }
    }
}

namespace NetBlog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? ThumbnailUrl { get; set; }
        public bool Status { get; set; }
        public bool IsBanner { get; set; }
        public string? Slug { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        //navigation properties
        public List<PostCategory>? PostCategories { get; set; }
    }
}

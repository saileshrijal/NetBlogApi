namespace NetBlog.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? UserId { get; set; }
        public string? Slug { get; set; }
        public ApplicationUser? User { get; set; }

        //navigation properties
        public List<PostCategory>? PostCategories { get; set; }
    }
}

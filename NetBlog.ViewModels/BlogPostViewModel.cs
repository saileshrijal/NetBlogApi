namespace NetBlog.ViewModels
{
    public class BlogPostViewModel
    {
        public PostViewModel? Post { get; set; }
        public List<PostViewModel>? RecentPosts { get; set; }
    }
}

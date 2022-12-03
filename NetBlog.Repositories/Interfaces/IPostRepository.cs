using NetBlog.Models;
using NetBlog.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Repositories.Interfaces
{
    public interface IPostRepository:IGenericRepository<Post>
    {
        Task<List<Post>> GetAllPostByUserId(string userId);
        Task<List<Post>> GetRecentPosts();
        Task<List<Post>> GetBannerPosts();
        Task<List<Post>> GetAllPublishedPosts();
        Task<Post> GetPublishedPost(string slug);
        Task<List<Post>> SearchPublishedPosts(string searchString);
    }
}

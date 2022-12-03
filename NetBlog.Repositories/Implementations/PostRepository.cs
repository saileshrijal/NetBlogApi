using Microsoft.EntityFrameworkCore;
using NetBlog.Data;
using NetBlog.Models;
using NetBlog.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Repositories.Implementations
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Post?> GetBy(Expression<Func<Post, bool>> predicate)
        {
            return await _context.Posts.Include(x => x.User).Include(x=>x.PostCategories).ThenInclude(x=>x.Category).FirstOrDefaultAsync(predicate);
        }

        public override async Task<List<Post>> GetAll()
        {
            return await _context.Posts.Include(x => x.User).Include(x=>x.PostCategories).ThenInclude(x=>x.Category).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<List<Post>> GetAllPostByUserId(string userId)
        {
            return await _context.Posts.Include(x => x.User).Include(x => x.PostCategories).ThenInclude(x => x.Category).Where(x=>x.UserId==userId).OrderByDescending(x=>x.CreatedDate).ToListAsync();
        }

        public async Task<List<Post>> GetRecentPosts()
        {
            return await _context.Posts.Include(x => x.User).Include(x => x.PostCategories).ThenInclude(x => x.Category).Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).Take(3).ToListAsync();
        }

        public async Task<List<Post>> GetBannerPosts()
        {
            return await _context.Posts.Include(x => x.User).Include(x => x.PostCategories).ThenInclude(x => x.Category).Where(x => x.Status == true && x.IsBanner==true).OrderByDescending(x => x.CreatedDate).Take(3).ToListAsync();
        }

        public async Task<List<Post>> GetAllPublishedPosts()
        {
            return await _context.Posts.Include(x => x.User).Include(x => x.PostCategories).ThenInclude(x => x.Category).Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<Post> GetPublishedPost(string slug)
        {
            return await _context.Posts.Include(x => x.User).Include(x => x.PostCategories).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Status == true && x.Slug == slug);
        }

        public async Task<List<Post>> SearchPublishedPosts(string searchString)
        {
            return await _context.Posts.Include(x => x.User).Include(x => x.PostCategories).ThenInclude(x => x.Category).Where(x => x.Status == true && x.Title!.StartsWith(searchString)).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }
    }
}

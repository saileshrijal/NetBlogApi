using Microsoft.EntityFrameworkCore;
using NetBlog.Data;
using NetBlog.Models;
using NetBlog.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Repositories.Implementations
{
    internal class PostCategoryRepository : GenericRepository<PostCategory>, IPostCategoryRepository
    {
        public PostCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<PostCategory>> SearchPublishedPostsByCategory(string categorySlug)
        {
            return await _context.PostCategories.Include(x=>x.Post).ThenInclude(x=>x.User).Include(x=>x.Post).ThenInclude(x=>x.PostCategories).ThenInclude(x=>x.Category).Where(x=>x.Category.Slug==categorySlug && x.Post.Status==true).OrderByDescending(x=>x.Post.CreatedDate).ToListAsync();
        }
    }
}

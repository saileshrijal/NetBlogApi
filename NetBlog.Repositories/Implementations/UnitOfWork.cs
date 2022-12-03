using NetBlog.Data;
using NetBlog.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository Category {get; private set;}
        public IPostRepository Post {get; private set;}
        public IPostCategoryRepository PostCategory {get; private set;}
        public IPageRepository Page {get; private set;}
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Post = new PostRepository(context);
            PostCategory = new PostCategoryRepository(context);
            Page = new PageRepository(context);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

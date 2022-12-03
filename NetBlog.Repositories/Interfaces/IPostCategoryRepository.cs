using NetBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Repositories.Interfaces
{
    public interface IPostCategoryRepository:IGenericRepository<PostCategory>
    {
        Task<List<PostCategory>> SearchPublishedPostsByCategory(string categorySlug);
    }
}

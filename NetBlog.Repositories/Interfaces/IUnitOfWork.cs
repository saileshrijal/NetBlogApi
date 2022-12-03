using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IPostRepository Post { get; }
        IPostCategoryRepository PostCategory { get; }
        IPageRepository Page { get; }
        Task SaveAsync();
    }
}

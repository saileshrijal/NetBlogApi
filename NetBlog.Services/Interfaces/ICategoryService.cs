using NetBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetCategories();
        Task<CategoryViewModel> GetCategory(int id);
        Task CreateCategory(CategoryViewModel vm);
        Task DeleteCategory(int id);
        Task UpdateCategory(CategoryViewModel vm);
    }
}

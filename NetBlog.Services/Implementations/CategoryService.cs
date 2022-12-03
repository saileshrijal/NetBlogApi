using Microsoft.Extensions.Hosting;
using NetBlog.Models;
using NetBlog.Repositories.Interfaces;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateCategory(CategoryViewModel vm)
        {
            var model = new CategoryViewModel().ConvertViewModel(vm);
            if (vm.Title != null)
            {
                model.Slug = vm.Title.Trim().ToLower().Replace(' ', '-');
            }
            await _unitOfWork.Category.Create(model);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCategory(int id)
        {
            try
            {
                await _unitOfWork.Category.Delete(id);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CategoryViewModel>> GetCategories()
        {
            var listOfCategories = await _unitOfWork.Category.GetAll();
            var listOfCategoriesVM = ConvertModelToViewModelList(listOfCategories);
            return listOfCategoriesVM;
        }

        public async Task<CategoryViewModel> GetCategory(int id)
        {
            var model = await _unitOfWork.Category.GetBy(x => x.Id == id);
            var vm = new CategoryViewModel();
            if(model != null)
            {
                vm = new CategoryViewModel(model);
            }
            return vm;
        }

        public async Task UpdateCategory(CategoryViewModel vm)
        {
            var model = await _unitOfWork.Category.GetBy(x => x.Id == vm.Id);
            model.Title = vm.Title;
            model.Description = vm.Description;
            await _unitOfWork.SaveAsync();
        }

        private List<CategoryViewModel> ConvertModelToViewModelList(List<Category> modelList)
        {
            return modelList.Select(x => new CategoryViewModel(x)).ToList();
        }
    }
}

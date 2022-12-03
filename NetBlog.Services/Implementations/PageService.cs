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
    public class PageService : IPageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreatePage(PageViewModel vm)
        {
            var pageVM = new PageViewModel().ConvertViewModel(vm);
            await _unitOfWork.Page.Create(pageVM);
            await _unitOfWork.SaveAsync();
        }

        public async Task<PageViewModel> GetPage(string slug)
        {
            var model = await _unitOfWork.Page.GetBy(x => x.Slug == slug);
            var vm = new PageViewModel();
            if(model != null)
            {
                vm = new PageViewModel(model);
            }
            return vm;
        }

        public async Task UpdatePage(PageViewModel vm)
        {
            var page = new PageViewModel().ConvertViewModel(vm);
            _unitOfWork.Page.Edit(page);
            await _unitOfWork.SaveAsync();
        }
    }
}

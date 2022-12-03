using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBlog.Data;
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

    public class PostService:IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreatePost(PostViewModel vm)
        {
            var selectedCateogries = vm.Categories?.Where(x => x.Selected).Select(x => x.Value).Select(int.Parse).ToList();
            var categories = selectedCateogries?.Select(x => new Category());
            var post = new Post
            {
                Title = vm.Title,
                Description = vm.Description,
                Slug = vm.Slug,
                ShortDescription = vm.ShortDescription,
                CreatedDate = vm.CreatedDate,
                IsBanner = vm.IsBanner,
                Status = vm.Status,
                UserId = vm.UserId,
                ThumbnailUrl = vm.ThumbnailUrl
            };
            
            if (vm.Title != null)
            {
                string slug = vm.Title.Trim();
                slug = slug.Replace(" ", "-");
                post.Slug = slug + "-" + Guid.NewGuid();
            }

            post.PostCategories = new List<PostCategory>();
            foreach (var categoryId in selectedCateogries!)
            {
                post.PostCategories?.Add(new PostCategory
                {
                    Post = post,
                    CategoryId = categoryId,
                });
            }

            await _unitOfWork.Post.Create(post);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeletePost(int id)
        {
            await _unitOfWork.Post.Delete(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<PostViewModel>> GetBannerPosts()
        {
            var listOfBannerPosts = await _unitOfWork.Post.GetBannerPosts();
            var listOfBannerPostsVM = ConvertModelToViewModelList(listOfBannerPosts);
            return listOfBannerPostsVM;
        }

        public async Task<PostViewModel> GetPost(int id)
        {
            try
            {
                var model = await _unitOfWork.Post.GetBy(x => x.Id == id);
                var vm = new PostViewModel();
                if(model == null)
                {
                    return vm;
                }
                vm = new PostViewModel(model);
                var CategoriesList = await _unitOfWork.Category.GetAll();
                var selectedCategoryList = await _unitOfWork.PostCategory.GetAllBy(pc => pc.PostId == model.Id);
                vm.Categories = new List<SelectListItem>();
                vm.Categories = CategoriesList.Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.Id.ToString()
                }).ToList();

                foreach (var item in vm.Categories)
                {
                    foreach (var selectedcategory in selectedCategoryList)
                    {
                        if (item.Value == selectedcategory.CategoryId.ToString())
                        {
                            item.Selected = true;
                        }
                    }
                }
                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PostViewModel>> GetPosts()
        {
            var listOfPosts = await _unitOfWork.Post.GetAll();
            var listOfPostsVM = ConvertModelToViewModelList(listOfPosts);
            return listOfPostsVM;
        }

        public async Task<List<PostViewModel>> GetPostsByUserId(string userId)
        {
            var listOfPosts = await _unitOfWork.Post.GetAllPostByUserId(userId);
            var listOfPostsVM = ConvertModelToViewModelList(listOfPosts);
            return listOfPostsVM;
        }

        public async Task<PostViewModel> GetPublishedPost(string slug)
        {
            var model = await _unitOfWork.Post.GetPublishedPost(slug);
            var vm = new PostViewModel();
            if (model != null)
            {
                vm = new PostViewModel(model);
            }
            return vm;
        }

        public async Task<List<PostViewModel>> GetPublishedPosts()
        {
            var listOfPublishedPosts = await _unitOfWork.Post.GetAllPublishedPosts();
            var listOfPublishedPostsVM = ConvertModelToViewModelList(listOfPublishedPosts);
            return listOfPublishedPostsVM;
        }

        public async Task<List<PostViewModel>> GetPublishedPostsByCategory(string categorySlug)
        {
            var listOfPosts = await _unitOfWork.PostCategory.SearchPublishedPostsByCategory(categorySlug);
            var post = new List<Post>();
            foreach(var item in listOfPosts)
            {
                post.Add(item.Post);
            }
            var listOfPublishedPostsVM = ConvertModelToViewModelList(post);
            return listOfPublishedPostsVM;
        }

        public async Task<List<PostViewModel>> GetPublishedSearchPosts(string searchString)
        {
            var listOfPublishedPosts = await _unitOfWork.Post.SearchPublishedPosts(searchString);
            var listOfPublishedPostsVM = ConvertModelToViewModelList(listOfPublishedPosts);
            return listOfPublishedPostsVM;
        }

        public async Task<List<PostViewModel>> GetRecentPosts()
        {
            var listOfRecentPosts =  await _unitOfWork.Post.GetRecentPosts();
            var listOfRecentPostsVM = ConvertModelToViewModelList(listOfRecentPosts);
            return listOfRecentPostsVM;
        }

        public async Task UpdatePost(PostViewModel vm)
        {
            var post = await _unitOfWork.Post.GetBy(x => x.Id == vm.Id);
            post.Title = vm.Title;
            post.Description = vm.Description;
            post.ShortDescription = vm.ShortDescription;
            post.ThumbnailUrl = vm.ThumbnailUrl;
            post.Status = vm.Status;
            post.IsBanner = vm.IsBanner;
            post.CreatedDate = vm.CreatedDate;

            var CategoriesList = await _unitOfWork.PostCategory.GetAllBy(pc => pc.PostId == post.Id);

            foreach (var item in CategoriesList)
            {
                await _unitOfWork.PostCategory.Delete(item.Id);
            }
            var selectedCateogries = vm.Categories?.Where(x => x.Selected).Select(x => x.Value).Select(int.Parse).ToList();

            post.PostCategories = new List<PostCategory>();
            foreach (var categoryId in selectedCateogries)
            {
                post.PostCategories?.Add(new PostCategory
                {
                    Post = post,
                    CategoryId = categoryId,
                });
            }

            _unitOfWork.Post.Edit(post);
            await _unitOfWork.SaveAsync();
        }

        private List<PostViewModel> ConvertModelToViewModelList(List<Post> listOfPosts)
        {
            return listOfPosts.Select(x => new PostViewModel(x)).ToList();
        }
    }
}

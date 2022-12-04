using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetBlog.Api.Config;
using NetBlog.Models;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;

namespace NetBlog.Api.Controllers
{
    [ApiController]
    public class BlogController : BaseController
    {
        public BlogController(UserManager<ApplicationUser> userManager, IUserService userService, ICategoryService categoryService, IPageService pageService, IWebHostEnvironment webHostEnvironment, IPostService postService, IOptionsMonitor<JwtConfig> optionsMonitor) : base(userManager, userService, categoryService, pageService, webHostEnvironment, postService, optionsMonitor)
        {
        }
        public async Task<IActionResult> Index(int? page, string? search, string? category)
        {
            var listOfPostsVM = new List<PostViewModel>();
            if (search != null)
            {
                listOfPostsVM = await _postService.GetPublishedSearchPosts(search);
            }
            else if (category != null)
            {
                listOfPostsVM = await _postService.GetPublishedPostsByCategory(category);
            }
            else
            {
                listOfPostsVM = await _postService.GetPublishedPosts();
            }
            return Ok(listOfPostsVM);
        }


        public async Task<IActionResult> Post(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(NotFound));
            }
            var vm = new BlogPostViewModel()
            {
                Post = await _postService.GetPublishedPost(id),
                RecentPosts = await _postService.GetRecentPosts()
            };
            if (vm.Post?.Id == 0)
            {
                return RedirectToAction(nameof(NotFound));
            }
            return Ok(vm);
        }
    }
}

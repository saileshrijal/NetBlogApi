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
    public class Home_Controller : BaseController
    {
        public Home_Controller(UserManager<ApplicationUser> userManager, IUserService userService, ICategoryService categoryService, IPageService pageService, IWebHostEnvironment webHostEnvironment, IPostService postService, IOptionsMonitor<JwtConfig> optionsMonitor) : base(userManager, userService, categoryService, pageService, webHostEnvironment, postService, optionsMonitor)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new HomeViewModel()
            {
                RecentPosts = await _postService.GetRecentPosts(),
                BannerPosts = await _postService.GetBannerPosts()
            };
            return Ok(vm);
        }
    }
}

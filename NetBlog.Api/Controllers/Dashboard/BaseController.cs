using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetBlog.Api.Config;
using NetBlog.Models;
using NetBlog.Services.Interfaces;
using System.Security.Claims;

namespace NetBlog.Api.Controllers.Dashboard
{
    [Route("api/Dashboard/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public UserManager<ApplicationUser> _userManager;
        public ICategoryService _categoryService;
        public IPageService _pageService;
        public IWebHostEnvironment _webHostEnvironment;
        public IPostService _postService;
        public IUserService _userService;
        public JwtConfig _jwtConfig;

        public BaseController(UserManager<ApplicationUser> userManager,
                                IUserService userService,
                                ICategoryService categoryService,
                                IPageService pageService,
                                IWebHostEnvironment webHostEnvironment,
                                IPostService postService,
                                IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _userService = userService;
            _categoryService = categoryService;
            _pageService = pageService;
            _webHostEnvironment = webHostEnvironment;
            _postService = postService;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        public async Task<ApplicationUser> GetLoggedInUser()
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _userManager.FindByEmailAsync(userEmail);
        }

        public async Task<string> GetRoleOfUser(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles[0];
        }
    }
}

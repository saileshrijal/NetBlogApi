using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetBlog.Api.Config;
using NetBlog.Models;
using NetBlog.Services.Interfaces;
using NetBlog.Utilities;
using NetBlog.ViewModels;

namespace NetBlog.Api.Controllers
{
    [ApiController]
    public class PostController : BaseController
    {
        public PostController(UserManager<ApplicationUser> userManager, IUserService userService, ICategoryService categoryService, IPageService pageService, IWebHostEnvironment webHostEnvironment, IPostService postService, IOptionsMonitor<JwtConfig> optionsMonitor) : base(userManager, userService, categoryService, pageService, webHostEnvironment, postService, optionsMonitor)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await GetLoggedInUser();
            var role = await GetRoleOfUser(loggedInUser);
            var vm = new List<PostViewModel>();
            if (role == "Admin")
            {
                vm = await _postService.GetPosts();
            }
            else
            {
                vm = await _postService.GetPostsByUserId(loggedInUser.Id);
            }
            return Ok(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PostViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            var loggedInUser = await GetLoggedInUser();
            vm.UserId = loggedInUser.Id;
            if (vm.Thumbnail != null)
            {
                vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "thumbnails");
            }
            await _postService.CreatePost(vm);
            return Ok("Post Created Successfully");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var loggedInUser = await GetLoggedInUser();
            var role = await GetRoleOfUser(loggedInUser);
            var postVM = await _postService.GetPost(id);
            if (postVM.Id == 0)
            {
                return BadRequest("Post not found");
            }
            if (loggedInUser.Id != postVM.UserId && role != "Admin")
            {
                return Ok("You are not authorized");
            }
            return Ok(postVM);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] PostViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            if (vm.Thumbnail != null)
            {
                vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "thumbnails");
            }
            await _postService.UpdatePost(vm);
            return Ok("Post updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var postVM = await _postService.GetPost(id);
            if (postVM.Id == 0)
            {
                return BadRequest("Post not found");
            }
            await _postService.DeletePost(id);
            return Ok("Post has been deleted successfully");
        }
    }
}

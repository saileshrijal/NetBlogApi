using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetBlog.Api.Config;
using NetBlog.Models;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;

namespace NetBlog.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class CategoryController : BaseController
    {
        public CategoryController(UserManager<ApplicationUser> userManager, IUserService userService, ICategoryService categoryService, IPageService pageService, IWebHostEnvironment webHostEnvironment, IPostService postService, IOptionsMonitor<JwtConfig> optionsMonitor) : base(userManager, userService, categoryService, pageService, webHostEnvironment, postService, optionsMonitor)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var listOfCategories = await _categoryService.GetCategories();
                return Ok(listOfCategories.Select(x => new
                {
                    x.Id,
                    x.UserId,
                    x.Title,
                    x.Description,
                    x.CreatedDate
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            var loggedInUser = await GetLoggedInUser();
            vm.UserId = loggedInUser.Id;
            await _categoryService.CreateCategory(vm);
            return Ok("Category Created Successfully");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return Ok("Category Deleted Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var categorVM = await _categoryService.GetCategory(id);
                if (categorVM.Id == 0)
                {
                    return BadRequest("Category Not Found");
                }
                return Ok(categorVM);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit(CategoryViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            try
            {
                await _categoryService.UpdateCategory(vm);
                return Ok("Category Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

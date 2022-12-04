using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetBlog.Api.Config;
using NetBlog.Models;
using NetBlog.Services.Interfaces;
using NetBlog.Utilities;
using NetBlog.ViewModels;
using System.Data;

namespace NetBlog.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PageController : BaseController
    {
        public PageController(UserManager<ApplicationUser> userManager, IUserService userService, ICategoryService categoryService, IPageService pageService, IWebHostEnvironment webHostEnvironment, IPostService postService, IOptionsMonitor<JwtConfig> optionsMonitor) : base(userManager, userService, categoryService, pageService, webHostEnvironment, postService, optionsMonitor)
        {
        }

        [HttpGet]
        public async Task<IActionResult> About()
        {
            try
            {
                var vm = await _pageService.GetPage("About");
                return Ok(vm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAbout([FromForm] PageViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            try
            {
                if (vm.Thumbnail != null)
                {
                    vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "page-img");
                }
                vm.Slug = "About";
                await _pageService.CreatePage(vm);
                return Ok("About Page Created Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditAbout([FromForm] PageViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            try
            {
                if (vm.Thumbnail != null)
                {
                    vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "page-img");
                }

                await _pageService.UpdatePage(vm);
                return Ok("About Page Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            try
            {
                var vm = await _pageService.GetPage("Contact");
                return Ok(vm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromForm] PageViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            try
            {
                if (vm.Thumbnail != null)
                {
                    vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "page-img");
                }

                vm.Slug = "Contact";
                await _pageService.CreatePage(vm);
                return Ok("Contact Page Created Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditContact([FromForm] PageViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            try
            {
                if (vm.Thumbnail != null)
                {
                    vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "page-img");
                }
                await _pageService.UpdatePage(vm);
                return Ok("Contact Page Created Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> PrivacyPolicy()
        {
            try
            {
                var vm = await _pageService.GetPage("PrivacyPolicy");
                return Ok(vm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrivacyPolicy(PageViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            if (vm.Thumbnail != null)
            {
                vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "page-img");
            }
            vm.Slug = "PrivacyPolicy";
            await _pageService.CreatePage(vm);
            return Ok("Privacy Policy page created successfully");
        }

        [HttpPut]
        public async Task<IActionResult> EditPrivacyPolicy(PageViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            if (vm.Thumbnail != null)
            {
                vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "page-img");
            }
            await _pageService.UpdatePage(vm);
            return Ok("Privacy Policy page updated successfully");
        }
    }
}

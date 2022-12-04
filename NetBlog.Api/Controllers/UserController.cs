using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetBlog.Api.Config;
using NetBlog.Models;
using NetBlog.Services.Interfaces;
using NetBlog.Utilities;
using NetBlog.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetBlog.Api.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        public UserController(UserManager<ApplicationUser> userManager, IUserService userService, ICategoryService categoryService, IPageService pageService, IWebHostEnvironment webHostEnvironment, IPostService postService, IOptionsMonitor<JwtConfig> optionsMonitor) : base(userManager, userService, categoryService, pageService, webHostEnvironment, postService, optionsMonitor)
        {
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var ListOfUsersVm = await _userService.GetUsers();
                return Ok(ListOfUsersVm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            try
            {
                var existingUser = await _userManager.FindByNameAsync(vm.UserName);
                if (existingUser == null) { return BadRequest("Invalid Username"); }
                var checkPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);
                if (!checkPassword) { return BadRequest("Invalid Username or Password"); }
                if (!existingUser.Status) { return BadRequest("This Username is Currently Inactive"); }
                var jwtToken = await GenerateToken(existingUser);
                return Ok(new AuthResult
                {
                    Token = jwtToken,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            try
            {
                var existingUserByUserName = await _userManager.FindByNameAsync(vm.UserName);
                if (existingUserByUserName != null) { return BadRequest("Username already exist"); }
                var existingUserByEmail = await _userManager.FindByEmailAsync(vm.Email);
                if (existingUserByEmail != null) { return BadRequest("Email already exist"); }
                await _userService.Register(vm);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(string id)
        {
            try
            {
                await _userService.ChangeStatus(id);
                return Ok("Status Changed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Profile([FromForm] ProfileViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var loggedInUser = await GetLoggedInUser();
                var profileVM = await _userService.GetUserProfileById(loggedInUser.Id);
                profileVM.About = vm.About;
                if (vm.ProfilePicture != null)
                {
                    profileVM.ProfilePictureUrl = FileHelper.UploadImage(vm.ProfilePicture, _webHostEnvironment, "user-img");
                }
                else
                {
                    vm.ProfilePictureUrl = profileVM.ProfilePictureUrl;
                }
                await _userService.UpdateProfile(profileVM);
                return Ok("Profile Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            try
            {
                await _userService.ResetPassword(vm);
                return Ok("Password Reset Successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }



        //method to generate token
        private async Task<string> GenerateToken(ApplicationUser newUser)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret!);
            var roles = await _userManager.GetRolesAsync(newUser);
            var jwtDescreptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", newUser.Id),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                }),

                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };
            jwtDescreptor.Subject.AddClaims(roles.Select(x => new Claim(ClaimTypes.Role, x)));
            var token = jwtHandler.CreateToken(jwtDescreptor);
            var jwtToken = jwtHandler.WriteToken(token);
            return jwtToken;
        }

    }
}

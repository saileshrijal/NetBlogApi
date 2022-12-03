using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBlog.Models;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;
using System.Transactions;

namespace NetBlog.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserViewModel>> GetUsers()
        {
            var listOfUsers = await _userManager.Users.ToListAsync();
            var ListOfUsersVm = ConvertModelToViewModelList(listOfUsers);
            foreach (var user in ListOfUsersVm)
            {
                var u = new UserViewModel().ConvertViewModel(user);
                user.Role = String.Join(',', await _userManager.GetRolesAsync(u));
            }
            return ListOfUsersVm;
        }

        private List<UserViewModel> ConvertModelToViewModelList(List<ApplicationUser> modelList)
        {
            return modelList.Select(x => new UserViewModel(x)).ToList();
        }

        public async Task Login(LoginViewModel vm)
        {
            await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, true, false);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task Register(RegisterViewModel vm)
        {
            var user = new RegisterViewModel().ConvertViewModel(vm);
            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, vm.Role);
            }
        }

        public async Task<ProfileViewModel> GetUserProfileById(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            var vm = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                About = user.About,
                Email = user.Email,
                UserName = user.UserName,
                ProfilePictureUrl = user.ProfilePictureUrl
            };
            return vm;
        }

        public async Task UpdateProfile(ProfileViewModel vm)
        {
            var userById = await _userManager.FindByNameAsync(vm.UserName);
            userById.FirstName = vm.FirstName;
            userById.LastName = vm.LastName;
            userById.About = vm.About;
            userById.ProfilePictureUrl = vm.ProfilePictureUrl;
            await _userManager.UpdateAsync(userById);
        }

        public async Task ChangeStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.Status = !user.Status;
            await _userManager.UpdateAsync(user);
        }

        public async Task ResetPassword(ResetPasswordViewModel vm)
        {
            var user = await _userManager.FindByIdAsync(vm.Id);
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, resetToken, newPassword: vm.Password);
        }
    }
}

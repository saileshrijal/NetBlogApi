using Microsoft.AspNetCore.Http;
using NetBlog.Models;
using System.ComponentModel.DataAnnotations;

namespace NetBlog.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? About { get; set; }

        public string? Email { get; set; }

        public string? UserName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public IFormFile? ProfilePicture { get; set; }

        public ProfileViewModel()
        {

        }

        public ProfileViewModel(ApplicationUser model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            About = model.About;
            Email = model.Email;
            UserName = model.UserName;
            ProfilePictureUrl = model.ProfilePictureUrl;
        }

        public ApplicationUser ConvertViewModel(ProfileViewModel model)
        {
            return new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                About = model.About,
                Email = model.Email,
                UserName = model.UserName,
                ProfilePictureUrl = model.ProfilePictureUrl,
            };
        }
    }
}

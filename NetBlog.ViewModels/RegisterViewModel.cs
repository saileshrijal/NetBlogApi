using NetBlog.Models;
using System.ComponentModel.DataAnnotations;

namespace NetBlog.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        public string? Role { get; set; }

        public RegisterViewModel()
        {
        }

        public RegisterViewModel(ApplicationUser model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            UserName = model.UserName;
            Email = model.Email;
        }

        public ApplicationUser ConvertViewModel(RegisterViewModel model)
        {
            return new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };
        }
    }
}

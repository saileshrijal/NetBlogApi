using NetBlog.Models;
using System.ComponentModel.DataAnnotations;

namespace NetBlog.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public LoginViewModel() { }

        public LoginViewModel(ApplicationUser model)
        {
            UserName = model.UserName;
        }

        public ApplicationUser ConvertViewModel(LoginViewModel model)
        {
            return new ApplicationUser
            {
                UserName = model.UserName
            };
        }

    }
}

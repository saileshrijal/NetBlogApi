using NetBlog.Models;

namespace NetBlog.ViewModels
{
    public class UserViewModel
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? About { get; set; }
        public bool Status { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public string? FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public UserViewModel() { }

        public UserViewModel(ApplicationUser model)
        {
            UserId = model.Id;
            FirstName = model.FirstName;
            LastName = model.LastName;
            About = model.About;
            Status = model.Status;
            ProfilePictureUrl = model.ProfilePictureUrl;
            Email = model.Email;
            UserName = model.UserName;

        }
        public ApplicationUser ConvertViewModel(UserViewModel model)
        {
            return new ApplicationUser
            {
                Id = model.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                About = model.About,
                ProfilePictureUrl = model.ProfilePictureUrl,
                Email = model.Email,
                UserName = model.UserName,
                Status = model.Status,
            };
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace NetBlog.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? About { get; set; }
        public bool Status { get; set; }
        public string? ProfilePictureUrl { get; set; }

        //Navigation Properies
        public List<Category>? Categories { get; set; }
        public List<Post>? Posts { get; set; }
    }
}

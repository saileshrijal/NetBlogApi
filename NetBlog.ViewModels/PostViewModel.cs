using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBlog.Models;
using System.ComponentModel.DataAnnotations;

namespace NetBlog.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? ThumbnailUrl { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public bool Status { get; set; }
        public bool IsBanner { get; set; }
        public string? Slug { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        //navigation properties
        public List<PostCategory>? PostCategories { get; set; }

        public PostViewModel()
        {
        }

        public PostViewModel(Post model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            ShortDescription = model.ShortDescription;
            Slug = model.Slug;
            CreatedDate = model.CreatedDate;
            ThumbnailUrl = model.ThumbnailUrl;
            Status = model.Status;
            IsBanner = model.IsBanner;
            User = model.User;
            UserId = model.UserId;
            PostCategories = model.PostCategories;
        }

        public Post ConvertViewModel(PostViewModel model)
        {
            return new Post
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                Slug = model.Slug,
                CreatedDate = model.CreatedDate,
                ThumbnailUrl = model.ThumbnailUrl,
                Status = model.Status,
                IsBanner = model.IsBanner,
                User = model.User,
                UserId = model.UserId,
                PostCategories = model.PostCategories
            };
        }

    }
}

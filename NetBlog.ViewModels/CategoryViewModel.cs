using NetBlog.Models;
using System.ComponentModel.DataAnnotations;

namespace NetBlog.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? Slug { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public CategoryViewModel()
        {
        }

        public CategoryViewModel(Category model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            CreatedDate = model.CreatedDate;
            UserId = model.UserId;
            User = model.User;
            Slug = model.Slug;
        }

        public Category ConvertViewModel(CategoryViewModel model)
        {
            return new Category
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                CreatedDate = model.CreatedDate,
                UserId = model.UserId,
                User = model.User,
                Slug = model.Slug
            };
        }
    }

}

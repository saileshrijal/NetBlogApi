using Microsoft.AspNetCore.Http;
using NetBlog.Models;

namespace NetBlog.ViewModels
{
    public class PageViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? Slug { get; set; }
        public string? ThumbnailUrl { get; set; }
        public IFormFile? Thumbnail { get; set; }

        public PageViewModel()
        {

        }
        public PageViewModel(Page model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            ShortDescription = model.ShortDescription;
            CreatedDate = model.CreatedDate;
            Slug = model.Slug;
            ThumbnailUrl = model.thumbnailUrl;
        }
        public Page ConvertViewModel(PageViewModel model)
        {
            return new Page
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                CreatedDate = model.CreatedDate,
                Slug = model.Slug,
                thumbnailUrl = model.ThumbnailUrl
            };
        }
    }
}

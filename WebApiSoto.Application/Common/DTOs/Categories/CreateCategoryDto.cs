

namespace WebApiSoto.Application.Common.DTOs.Categories
{
    public class CreateCategoryDto
    {
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
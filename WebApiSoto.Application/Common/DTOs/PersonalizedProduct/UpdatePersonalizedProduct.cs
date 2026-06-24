using System.ComponentModel.DataAnnotations;

namespace WebApiSoto.Application.Common.DTOs.PersonalizedProduct
{
    public class UpdatePersonalizedProductDto
    {
        public string? Description { get; set; }

        [Required]
        public List<CreatePersonalizationDetailDto> PersonalizationDetails { get; set; } = new();
    }
}
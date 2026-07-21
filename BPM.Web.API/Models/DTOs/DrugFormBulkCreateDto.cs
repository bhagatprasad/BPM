using System.ComponentModel.DataAnnotations;

namespace BPM.Web.API.Models.DTOs
{
    public class DrugFormBulkCreateDto
    {
        [Required]
        public List<CreateDrugFormDto> Forms { get; set; } = new List<CreateDrugFormDto>();
    }
}

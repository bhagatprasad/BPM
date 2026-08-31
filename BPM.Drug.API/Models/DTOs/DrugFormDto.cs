using System.ComponentModel.DataAnnotations;

namespace BPM.Web.Drug.API.Models.DTOs
{
    public class DrugFormDto
    {
        // CREATE DRUG FORM DTO

        public class CreateDrugFormDto
        {
            [Required]
            [MaxLength(20)]
            public string FormCode { get; set; } = string.Empty;

            [Required]
            [MaxLength(100)]
            public string FormName { get; set; } = string.Empty;

            [MaxLength(50)]
            public string? FormType { get; set; }
        }


        // BULK CREATE DRUG FORM DTO

        public class DrugFormBulkCreateDto
        {
            [Required]
            public List<CreateDrugFormDto> Forms { get; set; } = new List<CreateDrugFormDto>();
        }


        // RESPONSE DRUG FORM DTO

        public class ResponseDrugFormDto
        {
            public Guid FormId { get; set; }

            public string FormCode { get; set; } = string.Empty;

            public string FormName { get; set; } = string.Empty;

            public string? FormType { get; set; }

            public bool IsActive { get; set; }

            public DateTime CreatedOn { get; set; }

            public DateTime? ModifiedOn { get; set; }

            public int DrugCount { get; set; }

            public string DisplayName => $"{FormCode} - {FormName}";
        }


        // UPDATE DRUG FORM DTO

        public class UpdateDrugFormDto
        {
            [Required]
            public Guid FormId { get; set; }

            [Required]
            [MaxLength(20)]
            public string FormCode { get; set; } = string.Empty;

            [Required]
            [MaxLength(100)]
            public string FormName { get; set; } = string.Empty;

            [MaxLength(50)]
            public string? FormType { get; set; }

            public bool IsActive { get; set; } = true;
        }


        // FILTER DRUG FORM DTO

        public class DrugFormFilterDto
        {
            public string? FormCode { get; set; }

            public string? FormName { get; set; }

            public string? FormType { get; set; }

            public bool? IsActive { get; set; }

            public bool? HasDrugs { get; set; }

            public int? Page { get; set; } = 1;

            public int? PageSize { get; set; } = 10;

            public string? SortBy { get; set; }

            public bool SortDescending { get; set; }
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace BPM.Web.Drug.API.Models.DTOs
{
    public class DrugUomDto
    {
        // =========================
        // CREATE DTO
        // =========================

        public class CreateDrugUomDto
        {
            [Required]
            public Guid DrugId { get; set; }

            [Required]
            [MaxLength(20)]
            public string UomCode { get; set; } = string.Empty;

            [Required]
            [MaxLength(100)]
            public string UomName { get; set; } = string.Empty;

            [Required]
            [MaxLength(30)]
            public string UomType { get; set; } = string.Empty;


            // PARENT UOM
            public Guid? ParentUomId { get; set; }


            // QUANTITY / CONVERSION
            public int? QuantityPerParent { get; set; }

            public decimal ConversionFactor { get; set; } = 1;


            // FLAGS
            public bool IsBaseUnit { get; set; } = false;

            public bool IsPurchaseUom { get; set; } = false;

            public bool IsSalesUom { get; set; } = true;

            public bool IsInventoryUom { get; set; } = true;


            // DISPLAY
            public int DisplayOrder { get; set; } = 1;

            [MaxLength(250)]
            public string? Remarks { get; set; }
        }


        // =========================
        // UPDATE DTO
        // =========================

        public class UpdateDrugUomDto
        {
            [Required]
            public Guid UomId { get; set; }

            [Required]
            public Guid DrugId { get; set; }

            [Required]
            [MaxLength(20)]
            public string UomCode { get; set; } = string.Empty;

            [Required]
            [MaxLength(100)]
            public string UomName { get; set; } = string.Empty;

            [Required]
            [MaxLength(30)]
            public string UomType { get; set; } = string.Empty;


            // PARENT UOM
            public Guid? ParentUomId { get; set; }


            // QUANTITY / CONVERSION
            public int? QuantityPerParent { get; set; }

            public decimal ConversionFactor { get; set; }


            // FLAGS
            public bool IsBaseUnit { get; set; }

            public bool IsPurchaseUom { get; set; }

            public bool IsSalesUom { get; set; }

            public bool IsInventoryUom { get; set; }


            // DISPLAY
            public int DisplayOrder { get; set; }

            [MaxLength(250)]
            public string? Remarks { get; set; }


            // STATUS
            public bool IsActive { get; set; }
        }


        // =========================
        // RESPONSE DTO
        // =========================

        public class ResponseDrugUomDto
        {
            public Guid UomId { get; set; }

            public Guid DrugId { get; set; }

            public string? DrugCode { get; set; }

            public string? DrugName { get; set; }


            // UOM DETAILS
            public string UomCode { get; set; } = string.Empty;

            public string UomName { get; set; } = string.Empty;

            public string UomType { get; set; } = string.Empty;


            // PARENT UOM
            public Guid? ParentUomId { get; set; }

            public string? ParentUomCode { get; set; }

            public string? ParentUomName { get; set; }


            // QUANTITY / CONVERSION
            public int? QuantityPerParent { get; set; }

            public decimal ConversionFactor { get; set; }


            // FLAGS
            public bool IsBaseUnit { get; set; }

            public bool IsPurchaseUom { get; set; }

            public bool IsSalesUom { get; set; }

            public bool IsInventoryUom { get; set; }


            // DISPLAY
            public int DisplayOrder { get; set; }

            public string? Remarks { get; set; }


            // STATUS
            public bool IsActive { get; set; }


            // AUDIT
            public Guid? CreatedBy { get; set; }

            public DateTime? CreatedOn { get; set; }

            public Guid? ModifiedBy { get; set; }

            public DateTime? ModifiedOn { get; set; }
        }
    }
}
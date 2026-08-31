using System.ComponentModel.DataAnnotations;

namespace BPM.Web.Drug.API.Models.DTOs
{
    public class DrugPackagingDto
    {
        // =========================
        // CREATE DTO
        // =========================

        public class CreateDrugPackagingDto
        {
            [Required]
            public Guid DrugId { get; set; }

            [Required]
            public Guid PackageUomId { get; set; }

            [Required]
            public Guid ContainsUomId { get; set; }

            [Required]
            public int Quantity { get; set; }

            [Required]
            public int TotalUnits { get; set; }

            [Required]
            public decimal UnitPrice { get; set; }

            [Required]
            public decimal PackagePrice { get; set; }

            [MaxLength(100)]
            public string? Barcode { get; set; }

            public decimal? GrossWeight { get; set; }

            public decimal? NetWeight { get; set; }

            public decimal? Length { get; set; }

            public decimal? Width { get; set; }

            public decimal? Height { get; set; }
        }


        // =========================
        // UPDATE DTO
        // =========================

        public class UpdateDrugPackagingDto
        {
            [Required]
            public Guid PackagingId { get; set; }

            [Required]
            public Guid DrugId { get; set; }

            [Required]
            public Guid PackageUomId { get; set; }

            [Required]
            public Guid ContainsUomId { get; set; }

            [Required]
            public int Quantity { get; set; }

            [Required]
            public int TotalUnits { get; set; }

            [Required]
            public decimal UnitPrice { get; set; }

            [Required]
            public decimal PackagePrice { get; set; }

            [MaxLength(100)]
            public string? Barcode { get; set; }

            public decimal? GrossWeight { get; set; }

            public decimal? NetWeight { get; set; }

            public decimal? Length { get; set; }

            public decimal? Width { get; set; }

            public decimal? Height { get; set; }

            public bool IsActive { get; set; }
        }


        // =========================
        // RESPONSE DTO
        // =========================

        public class ResponseDrugPackagingDto
        {
            public Guid PackagingId { get; set; }


            // DRUG
            public Guid DrugId { get; set; }

            public string? DrugCode { get; set; }

            public string? DrugName { get; set; }


            // PACKAGE UOM
            public Guid PackageUomId { get; set; }

            public string? PackageUomCode { get; set; }

            public string? PackageUomName { get; set; }


            // CONTAINS UOM
            public Guid ContainsUomId { get; set; }

            public string? ContainsUomCode { get; set; }

            public string? ContainsUomName { get; set; }


            // QUANTITY
            public int Quantity { get; set; }

            public int TotalUnits { get; set; }


            // PRICING
            public decimal UnitPrice { get; set; }

            public decimal PackagePrice { get; set; }


            // BARCODE
            public string? Barcode { get; set; }


            // WEIGHT
            public decimal? GrossWeight { get; set; }

            public decimal? NetWeight { get; set; }


            // DIMENSIONS
            public decimal? Length { get; set; }

            public decimal? Width { get; set; }

            public decimal? Height { get; set; }


            // STATUS
            public bool IsActive { get; set; }


            // AUDIT
            public Guid? CreatedBy { get; set; }

            public DateTime CreatedOn { get; set; }
        }

        public class DrugPackagingFilterDto
        {
            public Guid? DrugId { get; set; }
            public Guid? PackageUomId { get; set; }
            public Guid? ContainsUomId { get; set; }
            public string? Barcode { get; set; }
            public decimal? MinPrice { get; set; }
            public decimal? MaxPrice { get; set; }
            public bool? IsActive { get; set; }
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
        }
    }
}
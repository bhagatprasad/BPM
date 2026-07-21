namespace BPM.Web.API.Models.DTOs
{
    public class DrugUomDto
    {
        public Guid UomId { get; set; }
        public Guid DrugId { get; set; }
        public string UomCode { get; set; } = string.Empty;
        public string UomName { get; set; } = string.Empty;
        public string UomType { get; set; } = string.Empty;
        public Guid? ParentUomId { get; set; }
        public int? QuantityPerParent { get; set; }
        public decimal ConversionFactor { get; set; }
        public bool IsBaseUnit { get; set; }
        public bool IsPurchaseUom { get; set; }
        public bool IsSalesUom { get; set; }
        public bool IsInventoryUom { get; set; }
        public int DisplayOrder { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? DrugName { get; set; } // Optional: for display purposes
        public string? ParentUomName { get; set; } // Optional: for display purposes
    }
}
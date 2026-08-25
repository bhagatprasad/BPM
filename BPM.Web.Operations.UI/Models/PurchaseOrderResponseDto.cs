using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Models
{
    public class PurchaseOrderResponseDto
    {
        public Guid Id { get; set; }

        public string PONumber { get; set; } = string.Empty;

        public Guid SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;

        public Guid DealerId { get; set; }
        public Guid DistributorId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string? PaymentTerms { get; set; }

        public string? DeliveryTerms { get; set; }

        public string? Remarks { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DealerDto? Dealer { get; set; }
        public bool IsOlderThan7Days { get; set; }
        public List<PurchaseOrderItemResponseDto> PurchaseOrderItemResponse { get; set; }
    }
}

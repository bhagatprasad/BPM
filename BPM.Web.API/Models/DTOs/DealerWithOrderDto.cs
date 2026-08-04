using BPM.Web.API.Models.DTOs.PurchaseOrder;

namespace BPM.Web.API.Models.DTOs
{
    public class DealerWithPuchaseOrderDto
    {
        public DealerDto Dealer { get; set; } = new DealerDto();
        public List<PurchaseOrderResponseDto> purchaseOrderResponseDtos { get; set; }= new List<PurchaseOrderResponseDto>();
    }
}

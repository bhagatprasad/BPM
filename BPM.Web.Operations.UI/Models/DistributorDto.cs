using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Models
{
    public class DistributorDto
    {
        public Guid Id { get; set; }
        public string DistributorCode { get; set; }
        public string DistributorName { get; set; }
        public string RegistrationNumber { get; set; }
        public string DrugLicenseNumber { get; set; }
        public string GSTNumber { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AlternatePhone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Website { get; set; }
        public Guid? WarehouseId { get; set; }
        public bool IsActive { get; set; }
    }
}

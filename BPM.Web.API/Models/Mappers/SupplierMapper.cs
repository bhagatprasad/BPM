using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class SupplierMapper
    {
        public static SupplierDto ToDto(this Supplier supplier)
        {
            return new SupplierDto
            {
                Id = supplier.SupplierId,
                SupplierCode = supplier.SupplierCode,
                SupplierName = supplier.SupplierName,
                ContactPerson = supplier.ContactPerson,
                Email = supplier.Email,
                Phone = supplier.Phone,
                AlternatePhone = supplier.AlternatePhone,
                GSTNumber = supplier.GSTNumber,
                DrugLicenseNumber = supplier.DrugLicenseNumber,
                AddressLine1 = supplier.AddressLine1,
                AddressLine2 = supplier.AddressLine2,
                City = supplier.City,
                State = supplier.State,
                Country = supplier.Country,
                PostalCode = supplier.PostalCode,
                Website = supplier.Website,
                IsActive = supplier.IsActive,
                CreatedOn = supplier.CreatedOn,
                ModifiedOn = supplier.ModifiedOn
            };
        }

        public static List<SupplierDto> ToDto(this IEnumerable<Supplier> suppliers)
        {
            return suppliers.Select(s => s.ToDto()).ToList();
        }

        public static Supplier ToEntity(this CreateSupplierDto dto)
        {
            return new Supplier
            {
                SupplierId = Guid.NewGuid(),
                SupplierCode = dto.SupplierCode,
                SupplierName = dto.SupplierName,
                ContactPerson = dto.ContactPerson,
                Email = dto.Email,
                Phone = dto.Phone,
                AlternatePhone = dto.AlternatePhone,
                GSTNumber = dto.GSTNumber,
                DrugLicenseNumber = dto.DrugLicenseNumber,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                Website = dto.Website,
                IsActive = dto.IsActive,
                CreatedOn = DateTime.UtcNow
            };
        }

        public static void MapToEntity(this UpdateSupplierDto dto, Supplier supplier)
        {
            supplier.SupplierCode = dto.SupplierCode;
            supplier.SupplierName = dto.SupplierName;
            supplier.ContactPerson = dto.ContactPerson;
            supplier.Email = dto.Email;
            supplier.Phone = dto.Phone;
            supplier.AlternatePhone = dto.AlternatePhone;
            supplier.GSTNumber = dto.GSTNumber;
            supplier.DrugLicenseNumber = dto.DrugLicenseNumber;
            supplier.AddressLine1 = dto.AddressLine1;
            supplier.AddressLine2 = dto.AddressLine2;
            supplier.City = dto.City;
            supplier.State = dto.State;
            supplier.Country = dto.Country;
            supplier.PostalCode = dto.PostalCode;
            supplier.Website = dto.Website;
            supplier.IsActive = dto.IsActive;
            supplier.ModifiedOn = DateTime.UtcNow;
        }

        public static Supplier ToEntity(this UpdateSupplierDto dto)
        {
            return new Supplier
            {
                SupplierId = dto.Id,
                SupplierCode = dto.SupplierCode,
                SupplierName = dto.SupplierName,
                ContactPerson = dto.ContactPerson,
                Email = dto.Email,
                Phone = dto.Phone,
                AlternatePhone = dto.AlternatePhone,
                GSTNumber = dto.GSTNumber,
                DrugLicenseNumber = dto.DrugLicenseNumber,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                Website = dto.Website,
                IsActive = dto.IsActive,
                ModifiedOn = DateTime.UtcNow
            };
        }
    }
}

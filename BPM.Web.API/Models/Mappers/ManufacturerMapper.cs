using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class ManufacturerMapper
    {
        public static ManufacturerDto ToDto(this Manufacturer manufacturer)
        {
            return new ManufacturerDto
            {
                Id = manufacturer.Id,
                ManufacturerCode = manufacturer.ManufacturerCode,
                ManufacturerName = manufacturer.ManufacturerName,
                ContactPerson = manufacturer.ContactPerson,
                Email = manufacturer.Email,
                Phone = manufacturer.Phone,
                AddressLine1 = manufacturer.AddressLine1,
                AddressLine2 = manufacturer.AddressLine2,
                City = manufacturer.City,
                State = manufacturer.State,
                Country = manufacturer.Country,
                PostalCode = manufacturer.PostalCode,
                Website = manufacturer.Website,
                IsActive = manufacturer.IsActive
            };
        }

        public static List<ManufacturerDto> ToDto(this IEnumerable<Manufacturer> manufacturers)
        {
            return manufacturers.Select(m => m.ToDto()).ToList();
        }

        public static Manufacturer ToEntity(this CreateManufacturerDto dto)
        {
            return new Manufacturer
            {
                Id = Guid.NewGuid(),
                ManufacturerCode = dto.ManufacturerCode,
                ManufacturerName = dto.ManufacturerName,
                ContactPerson = dto.ContactPerson,
                Email = dto.Email,
                Phone = dto.Phone,
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

        public static Manufacturer ToEntity(this UpdateManufacturerDto dto)
        {
            return new Manufacturer
            {
                Id = dto.Id,
                ManufacturerCode = dto.ManufacturerCode,
                ManufacturerName = dto.ManufacturerName,
                ContactPerson = dto.ContactPerson,
                Email = dto.Email,
                Phone = dto.Phone,
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

        public static void MapToEntity(this UpdateManufacturerDto dto, Manufacturer manufacturer)
        {
            manufacturer.ManufacturerCode = dto.ManufacturerCode;
            manufacturer.ManufacturerName = dto.ManufacturerName;
            manufacturer.ContactPerson = dto.ContactPerson;
            manufacturer.Email = dto.Email;
            manufacturer.Phone = dto.Phone;
            manufacturer.AddressLine1 = dto.AddressLine1;
            manufacturer.AddressLine2 = dto.AddressLine2;
            manufacturer.City = dto.City;
            manufacturer.State = dto.State;
            manufacturer.Country = dto.Country;
            manufacturer.PostalCode = dto.PostalCode;
            manufacturer.Website = dto.Website;
            manufacturer.IsActive = dto.IsActive;
            manufacturer.ModifiedOn = DateTime.UtcNow;
        }
    }
}

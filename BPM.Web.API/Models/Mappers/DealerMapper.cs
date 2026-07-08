using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class DealerMapper
    {
        public static DealerDto ToDto(this Dealer dealer)
        {
            return new DealerDto
            {
                Id = dealer.Id,
                DealershipName = dealer.DealershipName,
                RegistrationNumber = dealer.RegistrationNumber,
                TradeLicenseNumber = dealer.TradeLicenseNumber,
                GSTNumber = dealer.GSTNumber,
                ContactPerson = dealer.ContactPerson,
                Email = dealer.Email,
                Phone = dealer.Phone,
                AlternatePhone = dealer.AlternatePhone,
                AddressLine1 = dealer.AddressLine1,
                AddressLine2 = dealer.AddressLine2,
                City = dealer.City,
                State = dealer.State,
                Country = dealer.Country,
                PostalCode = dealer.PostalCode,
                Website = dealer.Website,
                IsActive = dealer.IsActive
            };
        }

        public static List<DealerDto> ToDto(this IEnumerable<Dealer> dealers)
        {
            return dealers.Select(d => d.ToDto()).ToList();
        }

        public static Dealer ToEntity(this CreateDealerDto dto)
        {
            return new Dealer
            {
                Id = Guid.NewGuid(),
                DealershipName = dto.DealershipName,
                RegistrationNumber = dto.RegistrationNumber,
                TradeLicenseNumber = dto.TradeLicenseNumber,
                GSTNumber = dto.GSTNumber,
                ContactPerson = dto.ContactPerson,
                Email = dto.Email,
                Phone = dto.Phone,
                AlternatePhone = dto.AlternatePhone,
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

        public static void MapToEntity(this UpdateDealerDto dto, Dealer dealer)
        {
            dealer.DealershipName = dto.DealershipName;
            dealer.RegistrationNumber = dto.RegistrationNumber;
            dealer.TradeLicenseNumber = dto.TradeLicenseNumber;
            dealer.GSTNumber = dto.GSTNumber;
            dealer.ContactPerson = dto.ContactPerson;
            dealer.Email = dto.Email;
            dealer.Phone = dto.Phone;
            dealer.AlternatePhone = dto.AlternatePhone;
            dealer.AddressLine1 = dto.AddressLine1;
            dealer.AddressLine2 = dto.AddressLine2;
            dealer.City = dto.City;
            dealer.State = dto.State;
            dealer.Country = dto.Country;
            dealer.PostalCode = dto.PostalCode;
            dealer.Website = dto.Website;
            dealer.IsActive = dto.IsActive;
            dealer.ModifiedOn = DateTime.UtcNow;
        }

        public static Dealer ToEntity(this UpdateDealerDto dto)
        {
            return new Dealer
            {
                Id = dto.Id,
                DealershipName = dto.DealershipName,
                RegistrationNumber = dto.RegistrationNumber,
                TradeLicenseNumber = dto.TradeLicenseNumber,
                GSTNumber = dto.GSTNumber,
                ContactPerson = dto.ContactPerson,
                Email = dto.Email,
                Phone = dto.Phone,
                AlternatePhone = dto.AlternatePhone,
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

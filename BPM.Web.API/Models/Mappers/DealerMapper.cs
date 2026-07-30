using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Models.Mappers
{
    public static class DealerMapper
    {
        public static DealerDto ToDto(this Dealer dealer)
        {
            DealerDto dealerDto = new DealerDto
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

            return dealerDto;
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

        public static Dealer ToUpdateDealerEntity(this DealerUpdatedDto dealerDto, Dealer dbDealer)
        {
            dbDealer.RegistrationNumber = dealerDto.RegistrationNumber;
            dbDealer.DealershipName = dealerDto.DealershipName;
            dbDealer.ContactPerson = dealerDto.ContactPerson;
            dbDealer.Email = dealerDto.Email;
            dbDealer.Phone = dealerDto.Phone;
            dbDealer.AlternatePhone = dealerDto.AlternatePhone;
            dbDealer.AddressLine1 = dealerDto.AddressLine1;
            dbDealer.AddressLine2 = dealerDto.AddressLine2;
            dbDealer.City = dealerDto.City;
            dbDealer.State = dealerDto.State;
            dbDealer.Country = dealerDto.Country;
            dbDealer.PostalCode = dealerDto.PostalCode;
            dbDealer.GSTNumber = dealerDto.GSTNumber;
            dbDealer.TradeLicenseNumber = dealerDto.TradeLicenseNumber;
            dbDealer.Website = dealerDto.Website;
            dbDealer.ModifiedBy = dealerDto.ModifiedBy;
            dbDealer.ModifiedOn = DateTime.Now;
            return dbDealer;
        }
    }
}

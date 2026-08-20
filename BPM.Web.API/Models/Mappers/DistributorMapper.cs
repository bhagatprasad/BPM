using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BPM.Web.API.Models.Mappers
{
    public static class DistributorMapper
    {
        public static Distributor ToEntity(this CreateDistributorDto createDistributorDto)
        {
            return new Distributor
            {
                DistributorCode = createDistributorDto.DistributorCode,
                DistributorName = createDistributorDto.DistributorName,
                RegistrationNumber = createDistributorDto.RegistrationNumber,
                DrugLicenseNumber = createDistributorDto.DrugLicenseNumber,
                GSTNumber = createDistributorDto.GSTNumber,
                ContactPerson = createDistributorDto.ContactPerson,
                Email = createDistributorDto.Email,
                Phone = createDistributorDto.Phone,
                AlternatePhone = createDistributorDto.AlternatePhone,
                AddressLine1 = createDistributorDto.AddressLine1,
                AddressLine2 = createDistributorDto.AddressLine2,
                City = createDistributorDto.City,
                State = createDistributorDto.State,
                Country = createDistributorDto.Country,
                PostalCode = createDistributorDto.PostalCode,
                Website = createDistributorDto.Website,
                WarehouseId = createDistributorDto.WarehouseId,
                IsActive = createDistributorDto.IsActive
            };
        }

        public static Distributor ToEntity(this UpdateDistributorDto distributorDto, Distributor distributor)
        {

            {
                distributor.DistributorName = distributorDto.DistributorName;
                distributor.RegistrationNumber = distributorDto.RegistrationNumber;
                distributor.DrugLicenseNumber = distributorDto.DrugLicenseNumber;
                distributor.GSTNumber = distributorDto.GSTNumber;
                distributor.ContactPerson = distributorDto.ContactPerson;
                distributor.Email = distributorDto.Email;
                distributor.Phone = distributorDto.Phone;
                distributor.AlternatePhone = distributorDto.AlternatePhone;
                distributor.AddressLine1 = distributorDto.AddressLine1;
                distributor.AddressLine2 = distributorDto.AddressLine2;
                distributor.City = distributorDto.City;
                distributor.State = distributorDto.State;
                distributor.Country = distributorDto.Country;
                distributor.PostalCode = distributorDto.PostalCode;
                distributor.Website = distributorDto.Website;
                distributor.WarehouseId = distributorDto.WarehouseId;
                distributor.IsActive = distributorDto.IsActive;
                return distributor;
            }
        }
        public static DistributorDto ToDto(this Distributor distributor)
        {
            return new DistributorDto
            {
                DistributorId = distributor.DistributorId,
                DistributorCode = distributor.DistributorCode,
                DistributorName = distributor.DistributorName,
                RegistrationNumber = distributor.RegistrationNumber,
                DrugLicenseNumber = distributor.DrugLicenseNumber,
                GSTNumber = distributor.GSTNumber,
                ContactPerson = distributor.ContactPerson,
                Email = distributor.Email,
                Phone = distributor.Phone,
                AlternatePhone = distributor.AlternatePhone,
                AddressLine1 = distributor.AddressLine1,
                AddressLine2 = distributor.AddressLine2,
                City = distributor.City,
                State = distributor.State,
                Country = distributor.Country,
                PostalCode = distributor.PostalCode,
                Website = distributor.Website,
                WarehouseId = distributor.WarehouseId,
                IsActive = distributor.IsActive,
                CreatedBy = distributor.CreatedBy,
                CreatedOn = distributor.CreatedOn,
                ModifiedBy = distributor.ModifiedBy,
                ModifiedOn = distributor.ModifiedOn
            };
        }
        public static List<DistributorDto> ToDto(this IEnumerable<Distributor> distributors)
        {
            if (distributors == null)
                return new List<DistributorDto>();

            return distributors.Select(d => d.ToDto()).ToList();
        }


        public static UserCreateDto ToUserCreateDtoFromDistiutor(this Distributor distributor, List<RoleDto> roles)
        {
            UserCreateDto userCreateDto = new UserCreateDto();

            var adminRole = roles.Where(x => x.Name == "Administrator").FirstOrDefault();

            userCreateDto.IsActive = true;
            userCreateDto.DistributorId = distributor.DistributorId;
            userCreateDto.Phone = distributor.Phone;
            userCreateDto.Email = distributor.Email;
            userCreateDto.FirstName = distributor.DistributorName;
            userCreateDto.LastName = distributor.DistributorName;
            userCreateDto.Password = "Admin@2026";
            userCreateDto.RoleId = adminRole.Id;
            return userCreateDto;
        }

        public static WarehouseCreateDto ToWarehouseCreateDtoFromDistributor(this Distributor distributor)
        {
            return new WarehouseCreateDto()
            {
                AddressLine1 = distributor.AddressLine1,
                AddressLine2 = distributor.AddressLine2,
                City = distributor.City,
                Country = distributor.Country,
                DistributorId = distributor.DistributorId,
                CreatedBy = distributor.CreatedBy,
                PostalCode = distributor.PostalCode,
                State = distributor.State,
                WarehouseCode = distributor.DistributorCode,
                WarehouseName = distributor.DistributorName
            };
        }
    }
}

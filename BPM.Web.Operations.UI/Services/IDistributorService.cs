using BPM.Web.Operations.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Services
{
    public interface IDistributorService
    {
        Task<DistributorDto> InsertDistributorAsync(CreateDistributorDto distributorDto);
        Task<DistributorDto> GetDistributorByIdAsync(Guid distributorId);
        Task<List<DistributorDto>> GetDistributorListAsync();
        Task<DistributorDto> UpdateDistributorAsync(Guid distributorId, UpdateDistributorDto updateDistributor);
        Task<bool> DeleteDistributorById(Guid distributorId);
    }
}

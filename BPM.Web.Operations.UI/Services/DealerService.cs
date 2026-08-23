using BPM.Web.Operations.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Services
{
    public class DealerService : IDealerService
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public DealerService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<List<DealerDto>> GetAllDealersAsync()
        {
            return await _repositoryFactory.SendAsync<List<DealerDto>>
            (
                HttpMethod.Get, "Dealer/getalldealers"
            );
        }
    }
}

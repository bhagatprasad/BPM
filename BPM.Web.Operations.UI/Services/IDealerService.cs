using BPM.Web.Operations.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Services
{
    public interface IDealerService
    {
        Task<List<DealerDto>> GetAllDealersAsync();
    }
}

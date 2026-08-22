using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Services
{
    public interface IRepositoryFactory
    {
        Task<TResponse> SendAsync<TResponse>(HttpMethod method, string uri);
        Task<TResponse> SendAsync<TRequest, TResponse>(HttpMethod method, string uri, TRequest entity = default);
    }
}

namespace BPM.Web.Distributor.UI.Services
{
    public interface IRepositoryFactory
    {
        Task<TResponse> SendAsync<TResponse>(HttpMethod method, string uri);
        Task<TResponse> SendAsync<TRequest, TResponse>(HttpMethod method, string uri, TRequest entity = default);
    }
}

using Microsoft.Extensions.Options;

namespace BPM.Web.Distributor.UI.Helpers
{
    public class TokenAuthorizationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BPMConfig _bpmConfig;

        public TokenAuthorizationHttpClientHandler(
            IHttpContextAccessor httpContextAccessor,
            IOptions<BPMConfig> bpmConfig)
        {
            _httpContextAccessor = httpContextAccessor;
            _bpmConfig = bpmConfig.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?
                .Session.GetString("AccessToken");

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
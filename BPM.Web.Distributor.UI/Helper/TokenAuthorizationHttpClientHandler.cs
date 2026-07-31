using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace BPM.Web.Distributor.UI.Helpers
{
    public class TokenAuthorizationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly BPMConfig _config;

        public TokenAuthorizationHttpClientHandler(
            IHttpContextAccessor contextAccessor,
            IOptions<BPMConfig> config)
        {
            _contextAccessor = contextAccessor;

            _config = config.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var jwt = _contextAccessor.HttpContext?
                .Session
                .GetString("JwtToken");

            if (!string.IsNullOrWhiteSpace(jwt))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", jwt);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
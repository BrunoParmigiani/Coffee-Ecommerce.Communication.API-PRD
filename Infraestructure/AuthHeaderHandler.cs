using System.Net.Http.Headers;

namespace Coffee_Ecommerce.Communication.API.Infraestructure
{
    public sealed class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IConfiguration _configuration;

        public AuthHeaderHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string key = _configuration.GetValue<string>("ApiKey")!;

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
            
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
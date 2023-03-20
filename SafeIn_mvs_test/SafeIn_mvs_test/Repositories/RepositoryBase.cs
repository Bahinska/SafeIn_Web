using Flurl.Http;
using Flurl.Http.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SafeIn_mvs_test.Repositories
{
    public class UntrustedCertClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
        }
    }
    public class RepositoryBase
    {
        protected readonly IFlurlClient _flurlClient;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public RepositoryBase(IFlurlClientFactory flurlClientFactory,
                                IHttpContextAccessor httpContextAccessor)
        {
            FlurlHttp.Configure(settings => settings.HttpClientFactory = new UntrustedCertClientFactory());
            _httpContextAccessor = httpContextAccessor;
            _flurlClient = flurlClientFactory.Get("https://safeinapisecondaccount.azurewebsites.net");

            _flurlClient.BeforeCall(flurlCall =>
            {
                var token = _httpContextAccessor.HttpContext
                           .Request.Cookies[Constants.XAsseccToken];

                if (!string.IsNullOrEmpty(token))
                {
                    flurlCall.HttpRequestMessage.SetHeader("Authorization", $"Bearer {token}");
                }
                else
                {
                    flurlCall.HttpRequestMessage.SetHeader("Authorization", string.Empty);
                }

                
            });
            
        }
    }
}

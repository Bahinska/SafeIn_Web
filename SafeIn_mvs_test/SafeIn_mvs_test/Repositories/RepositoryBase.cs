using Flurl.Http;
using Flurl.Http.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SafeIn_mvs_test.Models;
using Segment;
using System.Text.Json;
using System.Text;

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
                                IHttpContextAccessor httpContextAccessor
                                )
        {
            FlurlHttp.Configure(settings => settings.HttpClientFactory = new UntrustedCertClientFactory());
            _httpContextAccessor = httpContextAccessor;
            _flurlClient = flurlClientFactory.Get("https://safeinapisecondaccount.azurewebsites.net");

            _flurlClient.BeforeCall(async flurlCall =>
            {
                var accessToken = _httpContextAccessor.HttpContext
                           .Request.Cookies[Constants.XAsseccToken];
                if (accessToken != null)
                {   //check if token not expired
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                    client.BaseAddress = new Uri("https://safeinapisecondaccount.azurewebsites.net/Auth/");
                    var res = await client.GetAsync("tokenValidate");
                    
                    if(!res.IsSuccessStatusCode)//if not refresh it
                    { 
                        var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies[Constants.XRefreshToken];
                        var oldTokens = new Tokens()
                        {
                            accessToken = accessToken,
                            refreshToken = refreshToken
                        };
                        client.DefaultRequestHeaders.Remove("Authorization");
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                        //refresh token
                        var response = await client.PostAsJsonAsync("refresh", oldTokens);
                        var newTokens = await response.Content.ReadFromJsonAsync<Tokens>();

                        //set new ones to cookies
                        _httpContextAccessor.HttpContext.Response.Cookies.Append(Constants.XAsseccToken, newTokens.accessToken);
                        _httpContextAccessor.HttpContext.Response.Cookies.Append(Constants.XRefreshToken, newTokens.refreshToken);
                        accessToken = newTokens.accessToken;
                    }
                }
                if (!string.IsNullOrEmpty(accessToken))
                {
                    flurlCall.HttpRequestMessage.SetHeader("Authorization", $"Bearer {accessToken}");
                }
                else
                {
                    flurlCall.HttpRequestMessage.SetHeader("Authorization", string.Empty);
                }

                
            });
            
        }
    }
}

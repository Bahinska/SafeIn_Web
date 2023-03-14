using Microsoft.AspNetCore.Identity;
using SafeIn_mvs_test.Models;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using System.Text.Json;
using ZXing;

namespace SafeIn_mvs_test.Repositories
{
    public class UserManagementRepository : RepositoryBase, IUserManagementRepository
    {
        public UserManagementRepository(IFlurlClientFactory flurlClientFactory, IHttpContextAccessor httpContextAccessor) : base(flurlClientFactory, httpContextAccessor)
        {
        }

        public async Task<Tokens> RefreshTokensAsync(Tokens oldTokens)
        {
            return await _flurlClient.Request("/Auth/refresh").PostJsonAsync(oldTokens).ReceiveJson<Tokens>();
        }
        public async Task<IFlurlResponse> LoginAsync(UserLogin user)
        {
            return await _flurlClient.Request("/Auth/login").PostJsonAsync(user);
        }

        public async Task<IFlurlResponse> GetTokenInfoAsync(string token)
        {
            return await _flurlClient.Request("/api/Employee/information").GetAsync();
        }
    }
}

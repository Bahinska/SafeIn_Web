using Flurl.Http;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.Repositories;
using System.Web.Helpers;
namespace SafeIn_mvs_test.Services
{
    public class UserManagementService : IUserManagementService
    {
        public readonly IUserManagementRepository _userManagementRepository;

        public UserManagementService(IUserManagementRepository userManagementRepository)
        {
           _userManagementRepository = userManagementRepository;
        }

        public async Task<UserInfo> GetTokenInfoAsync(string token)
        {
            var response = await _userManagementRepository.GetTokenInfoAsync(token);
            return await response.GetJsonAsync<UserInfo>();
        }

        public async Task<Tokens> LoginAsync(UserLogin user)
        {
            var response = await _userManagementRepository.LoginAsync(user);
            return await response.GetJsonAsync<Tokens>();
        }

        public async Task LogoutAsync(RevokeToken tokenToRevoke)
        {
            await _userManagementRepository.LogoutAsync(tokenToRevoke);
        }

        public async Task<Tokens> RefreshTokensAsync(Tokens oldTokens)
        {
            return await _userManagementRepository.RefreshTokensAsync(oldTokens);
        }

    }
}

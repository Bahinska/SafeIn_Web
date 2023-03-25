using Flurl.Http;
using NuGet.Packaging.Signing;
using SafeIn_mvs_test.Models;
using System.Drawing;

namespace SafeIn_mvs_test.Services
{
    public interface IUserManagementService
    {
        Task<Tokens> LoginAsync(UserLogin user);
        Task<Tokens> RefreshTokensAsync(Tokens oldTokens);
        Task<UserInfo> GetTokenInfoAsync(string token);
        Task LogoutAsync(RevokeToken tokenToRevoke);
        Task EditAsync(EditModel user);

    }
}

using Flurl.Http;
using SafeIn_mvs_test.Models;

namespace SafeIn_mvs_test.Repositories
{
    public interface IUserManagementRepository
    {
        Task<IFlurlResponse> LoginAsync(UserLogin user);
        Task<Tokens> RefreshTokensAsync(Tokens oldTokens);
        Task<IFlurlResponse> GetTokenInfoAsync(string token);
    }   
}

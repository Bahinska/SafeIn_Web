using Flurl.Http;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.ViewModels;

namespace SafeIn_mvs_test.Services
{
    public interface IAdminService
    {
        Task<List<UserInfo>> GetAdminsAsync();
        Task<IFlurlResponse> CreateAdminAsync(CreateAdminViewModel admin);
    }
}

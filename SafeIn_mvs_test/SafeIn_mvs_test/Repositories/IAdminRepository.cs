using Flurl.Http;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.ViewModels;

namespace SafeIn_mvs_test.Repositories
{
    public interface IAdminRepository
    {
        Task<IFlurlResponse> GetAdminsAsync();
        Task<IFlurlResponse> CreateAdminAsync(CreateAdminViewModel admin);
    }
}

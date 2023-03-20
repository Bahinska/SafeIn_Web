using Flurl.Http;
using Flurl.Http.Configuration;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.ViewModels;

namespace SafeIn_mvs_test.Repositories
{
    public class AdminRepository : RepositoryBase, IAdminRepository
    {
        public AdminRepository(IFlurlClientFactory flurlClientFactory,
                        IHttpContextAccessor httpContextAccessor) :
                   base(flurlClientFactory, httpContextAccessor)
        {
        }
        public async Task<IFlurlResponse> CreateAdminAsync(CreateAdminViewModel admin)
        {
            return await _flurlClient.Request("/api/SuperAdmin/admin").PostJsonAsync(admin);
        }

        public async Task DeleteAdminAsync(string email)
        {
            await _flurlClient.Request("/api/SuperAdmin/admin").SetQueryParam("email", email).DeleteAsync();
        }

        public async Task<IFlurlResponse> GetAdminsAsync()
        {
            return await _flurlClient.Request("/api/SuperAdmin/admins").GetAsync();
        }
    }
}

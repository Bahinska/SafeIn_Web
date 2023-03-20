using Flurl.Http;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.Repositories;
using SafeIn_mvs_test.ViewModels;

namespace SafeIn_mvs_test.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminsRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminsRepository = adminRepository;
        }

        public async Task<IFlurlResponse> CreateAdminAsync(CreateAdminViewModel admin)
        {
            return await _adminsRepository.CreateAdminAsync(admin);
        }

        public async Task DeleteAdminAsync(string email)
        {
            await _adminsRepository.DeleteAdminAsync(email);
        }

        public async Task<List<UserInfo>> GetAdminsAsync()
        {
           var response = await _adminsRepository.GetAdminsAsync();
           return await response.GetJsonAsync<List<UserInfo>>();
        }
    }
}

using SafeIn_mvs_test.Models;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Policy;
using System.Net;
using SafeIn_mvs_test.ViewModels;

namespace SafeIn_mvs_test.Repositories
{
    public class EmployeeRepository : RepositoryBase, IEmployeeRepository
    {
    
        public EmployeeRepository(IFlurlClientFactory flurlClientFactory,
                                IHttpContextAccessor httpContextAccessor):
                           base(flurlClientFactory, httpContextAccessor)    
        {
        }
        public async Task<List<UserInfo>> GetEmployeesAsync()
        {
            return await _flurlClient.Request("/api/Admin/employees").GetJsonAsync<List<UserInfo>>();
        }
        public async Task DeleteEmployeeAsync(string email)
        {
            await _flurlClient.Request("/api/Admin/employee").SetQueryParam("email", email).DeleteAsync();
        }

        public async Task<IFlurlResponse> CreateEmployeeAsync(CreateEmployeeViewModel employee)
        {
            return await _flurlClient.Request("/api/Admin/employee").PostJsonAsync(employee);
        }
    }
}

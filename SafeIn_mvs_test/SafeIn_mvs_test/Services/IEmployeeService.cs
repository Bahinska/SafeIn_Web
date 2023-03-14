using Flurl.Http;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.ViewModels;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SafeIn_mvs_test.Services
{
    public interface IEmployeeService
    {
        Task<List<UserInfo>> GetEmployeesAsync();
        Task<IFlurlResponse> CreateEmployeeAsync(CreateEmployeeViewModel employee);
    }
}

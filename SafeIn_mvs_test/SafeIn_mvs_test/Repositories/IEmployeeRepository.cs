using Flurl.Http;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.ViewModels;
using System.Threading.Tasks;

namespace SafeIn_mvs_test.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<UserInfo>> GetEmployeesAsync();
        Task<IFlurlResponse> CreateEmployeeAsync(CreateEmployeeViewModel employee);
        Task DeleteEmployeeAsync(string email);
    }
}

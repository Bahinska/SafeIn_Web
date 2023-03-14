using Flurl.Http;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.Repositories;
using SafeIn_mvs_test.ViewModels;
using System.Threading.Tasks;

namespace SafeIn_mvs_test.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository usersRepository)
        {
            _employeeRepository = usersRepository;
        }

        public async Task<IFlurlResponse> CreateEmployeeAsync(CreateEmployeeViewModel employee)
        {
            return await _employeeRepository.CreateEmployeeAsync(employee);
        }

        public async Task<List<UserInfo>> GetEmployeesAsync()
        {
            return await _employeeRepository.GetEmployeesAsync();
        }
    }
}

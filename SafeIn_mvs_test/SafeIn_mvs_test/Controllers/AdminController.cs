using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.Services;
using SafeIn_mvs_test.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;

namespace SafeIn_mvs_test.Controllers
{
    public class AdminController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IStringLocalizer _localizer;

        public AdminController(IEmployeeService employeeService, IStringLocalizer<UserManagementController> localizer)
        {
            _employeeService = employeeService;
            _localizer = localizer;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new AllEmployeeViewModel
            {
                Employees = await _employeeService.GetEmployeesAsync()
            };
            return View(viewModel);
        }
        public IActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        [ActionName("CreateEmployeeAsync")]
        public async Task<IActionResult> CreateEmployeeAsync(CreateEmployeeViewModel viewModel)
        {
            TempData.Clear();
            try
            {
                var result = await _employeeService.CreateEmployeeAsync(viewModel);
                TempData["SuccessMessageEmployeeCreate"] = JsonConvert.SerializeObject(_localizer["SuccessMessageEmployeeCreate"]);
            }
            catch (FlurlHttpException ex)
            {
                TempData["CreateEmployeeError"] = JsonConvert.SerializeObject(_localizer["CreateEmployeeError"]);
                return RedirectToAction("CreateEmployee", "Admin");
            }
            return RedirectToAction("CreateEmployee", "Admin");
        }
    }
}

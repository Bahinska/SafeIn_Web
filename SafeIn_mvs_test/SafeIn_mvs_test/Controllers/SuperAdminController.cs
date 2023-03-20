using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.Services;
using SafeIn_mvs_test.ViewModels;
using System.Net;

namespace SafeIn_mvs_test.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IStringLocalizer _localizer;

        public SuperAdminController(IAdminService adminService, IStringLocalizer<UserManagementController> localizer)
        {
            _adminService = adminService;
            _localizer = localizer;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new AllAdminsViewModel
            {
                Admins = await _adminService.GetAdminsAsync()
                
            };
            return  View(viewModel);
        }
        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [ActionName("CreateAdminAsync")]
        public async Task<IActionResult> CreateAdminAsync(CreateAdminViewModel viewModel)
        {
            TempData.Clear();
            try
            {
                var result = await _adminService.CreateAdminAsync(viewModel);
                TempData["SuccessMessageAdminCreate"] = JsonConvert.SerializeObject(_localizer["SuccessMessageAdminCreate"]);
            }
            catch (FlurlHttpException ex)
            {
                TempData["CreateAdminError"] = JsonConvert.SerializeObject(_localizer["CreateAdminError"]);
                return RedirectToAction("CreateAdmin", "SuperAdmin");
            }
            return RedirectToAction("CreateAdmin", "SuperAdmin");
        }

        [HttpGet]
        [ActionName("EditAdminAsync")]
        public IActionResult EditAdminAsync(string email)
        {
            var a = 5;
            return View(a);
        }
        [HttpGet]
        [ActionName("DeleteAdminAsync")]
        public async Task<IActionResult> DeleteAdminAsync(string email)
        {
            await _adminService.DeleteAdminAsync(email);
            return RedirectToAction("Index", "SuperAdmin");
        }
    }
}


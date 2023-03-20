using Flurl.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.Services;
using SafeIn_mvs_test.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using static QRCoder.PayloadGenerator;
using ZXing.Aztec.Internal;

namespace SafeIn_mvs_test.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IStringLocalizer _localizer;

        public UserManagementController(IUserManagementService userManagementService, IStringLocalizer<UserManagementController> localizer)
        {
            _userManagementService = userManagementService;
            _localizer = localizer;
        }
        public IActionResult Login()
        {
            Response.Cookies.Delete(Constants.XAsseccToken);
            Response.Cookies.Delete(Constants.XAsseccToken);
            return View();
        }

        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30)});

            return LocalRedirect(returnUrl);
        }
        
        public IActionResult Logout()
        {
            _userManagementService.LogoutAsync(new RevokeToken() {refreshToken = Request.Cookies[Constants.XRefreshToken]});
            Response.Cookies.Delete(Constants.XAsseccToken);
            Response.Cookies.Delete(Constants.XRefreshToken);
            return View("Login");
        }

        [HttpPost]
        [ActionName("LoginPostAsync")]
        public async Task<IActionResult> LoginPostAsync(LoginViewModel viewModel)
        {
            var userLogin = new UserLogin
            {
                email = viewModel.Email,
                password = viewModel.Password
            };
            try
            {
                var tokens = await _userManagementService.LoginAsync(userLogin);
                Response.Cookies.Append(Constants.XAsseccToken, tokens.accessToken);
                Response.Cookies.Append(Constants.XRefreshToken, tokens.refreshToken);
                return Redirect(GetTokenInfo(tokens.accessToken));
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    TempData["errorLoginMessage"] = JsonConvert.SerializeObject( _localizer["LoginError"]);
                    return RedirectToAction("Login", "UserManagement");
                }
            }
            return RedirectToAction("Login", "UserManagement");
        }

        public IActionResult Redirect(UserInfo info)
        {
            if (info.role == "SuperAdmin") return RedirectToAction("Index", "SuperAdmin");
            if (info.role == "Admin") return  RedirectToAction("Index", "Admin");
            else
            {
                EmployeeViewModel employee = new EmployeeViewModel
                {
                    email = info.email,
                    name = info.name,
                    company = info.company
                };
                return RedirectToAction("Index", "Employee", employee);
            }
        }
        public UserInfo GetTokenInfo(string token)
        {
            var tokenInfo = new Dictionary<string, string>();
            var handler = new JwtSecurityTokenHandler();
            var tokenData = handler.ReadJwtToken(token);
            var claims = tokenData.Claims.ToList();
            foreach (var claim in claims)
            {
                tokenInfo.Add(claim.Type, claim.Value);
            }
            var info = new UserInfo()
            {
                email = tokenInfo["Email"],
                name = tokenInfo["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
                role = tokenInfo["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
                company = tokenInfo["Company"]
            };
            return info;
        }
    }
}

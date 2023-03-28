using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;
using QRCoder;
using SafeIn_mvs_test.ViewModels;
using System.Drawing;
using ZXing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ZXing.QrCode.Internal;
using Microsoft.AspNetCore.Components.Forms;
using Segment.Model;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.Repositories;
using SafeIn_mvs_test.Services;
using SafeIn_mvs_test.Helper;

namespace SafeIn_mvs_test.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUserManagementService _userManagementService;

        public EmployeeController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }
        public IActionResult Index()
        {
            var info = Helper.Helper.GetTokenInfo(HttpContext.Request.Cookies[Constants.XAsseccToken]);
            EmployeeViewModel employee = new EmployeeViewModel
            {
                email = info.email,
                name = info.userName,
                company = info.company
            };
            return View(employee);
        }
        public async Task<IActionResult> QrCode()
        {
            return View();
        }
        public async Task<IActionResult> GenerateQRCode()
        {
            var oldTokens = new Tokens()
            {
                accessToken = HttpContext.Request.Cookies[Constants.XAsseccToken],
                refreshToken = HttpContext.Request.Cookies[Constants.XRefreshToken]
            };
            var newTokens = await _userManagementService.RefreshTokensAsync(oldTokens);
            RenewCookies(newTokens);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(newTokens.accessToken, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qRCode.GetGraphic(20);
            return File(qrCodeBytes, "image/png");   
        }
        public void RenewCookies(Tokens newTokens)
        {
            Response.Cookies.Delete(Constants.XAsseccToken);
            Response.Cookies.Delete(Constants.XRefreshToken);

            Response.Cookies.Append(Constants.XAsseccToken, newTokens.accessToken);
            Response.Cookies.Append(Constants.XRefreshToken, newTokens.refreshToken);
        }
    }
}

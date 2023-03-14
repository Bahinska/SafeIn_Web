using System.ComponentModel.DataAnnotations;

namespace SafeIn_mvs_test.ViewModels
{
    public class CreateAdminViewModel
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}

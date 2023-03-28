using System.ComponentModel.DataAnnotations;

namespace SafeIn_mvs_test.ViewModels
{
    public class CreateEmployeeViewModel
    {
        public string email { get; set; }
        public string userName { get; set; }

        public string password { get; set; }
        public string confirmPassword { get; set; }
    }

}

using System.ComponentModel.DataAnnotations;

namespace CookieAuthentication.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "用户名不能为空")]
        public string Account { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

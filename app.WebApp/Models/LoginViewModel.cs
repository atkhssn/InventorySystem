using System.ComponentModel.DataAnnotations;
namespace app.WebApp.Models
{   public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string IncorrectInput { get; set; }
        public string ReturnUrl { get; set; }
    }
}

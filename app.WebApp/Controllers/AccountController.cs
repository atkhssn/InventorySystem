using app.Infrastructure.Auth;
using app.Services.UserpermissionsService;
using app.Services.UserServices;
using app.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace app.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserServices UserServices;
        private readonly IUserpermissionServices userpermission;
        private SignInManager<ApplicationUser> signInManager;
        public AccountController(IUserServices UserServices,
            IUserpermissionServices userpermission,
            SignInManager<ApplicationUser> signInManager
            )
        {
            this.signInManager = signInManager;

            this.UserServices = UserServices;
            this.userpermission = userpermission;
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            LoginViewModel model = new LoginViewModel();
            return await Task.Run(() => View(model));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await UserServices.GetByUser(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return await Task.Run(() => View(model));

            }
            var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                var getitem = await userpermission.GetAllMenuItemRecort(user.Id);
                HttpContext.Session.SetString("Username", user.UserName.ToString());
                var arry = JsonSerializer.Serialize(getitem);
                HttpContext.Session.SetString("ArrayData", arry);
                return await Task.Run(() => Redirect("/Admin/Index"));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Password is not verified");
                return await Task.Run(() => View(model));
            }

        }

        public async Task<IActionResult> AccessDenied()
        {

            return await Task.Run(() => View());
        }

        public async Task<IActionResult> Logout()
        {
            if (signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
                HttpContext.Session.Clear();
                return await Task.Run(() => Redirect("Login"));
            }
            return await Task.Run(() => Redirect("Login"));
        }
    }
}

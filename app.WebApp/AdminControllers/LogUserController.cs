using app.EntityModel.CoreModel;
using app.Services.MainMenuService;
using app.Services.RolesServises;
using app.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace app.WebApp.AdminControllers
{
    
    public class LogUserController : Controller
    {
        private readonly IUserServices userServices;   
        private readonly IRoleService _rolesServices;
        public LogUserController(IUserServices userServices,IRoleService _rolesServices)
        {
            this.userServices = userServices;
            this._rolesServices = _rolesServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            UserViewModel model = new UserViewModel();
            model = await userServices.GetAllRecort();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddRecort()
        {           
            UserViewModel model = new UserViewModel();
            ViewBag.Recort = new SelectList(_rolesServices.GetAllAsync().Select(s => new { Id = s.Name, Name = s.Name }), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecort(UserViewModel model)
        {
            var result = await userServices.AddUser(model);
            if (result == 1)
            {
                UserViewModel model2 = new UserViewModel();
                ViewBag.Recort = new SelectList(_rolesServices.GetAllAsync().Select(s => new { Id = s.Name, Name = s.Name }), "Id", "Name");
                ModelState.AddModelError(string.Empty, "Same Email already exists!");
                return View(model2);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRecort(string id)
        {
            UserViewModel viewModel = new UserViewModel();
            ViewBag.Recort = new SelectList(_rolesServices.GetAllAsync().Select(s => new { Id = s.Name, Name = s.Name }), "Id", "Name");
            viewModel = await userServices.GetByUserId(id);
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRecort(UserViewModel model)
        {
            var result = await userServices.UpdateUser(model);
            if (result == 1)
            {
                UserViewModel model2 = new UserViewModel();
                ViewBag.Recort = new SelectList(_rolesServices.GetAllAsync().Select(s => new { Id = s.Name, Name = s.Name }), "Id", "Name");
                ModelState.AddModelError(string.Empty, "Same Email already exists!");
                return View(model2);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await userServices.SoftDelete(id);
            return RedirectToAction("Index");
        }
    }
}

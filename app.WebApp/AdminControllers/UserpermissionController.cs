using app.EntityModel.CoreModel;
using app.Services.MainMenuService;
using app.Services.MenuItemService;
using app.Services.UserpermissionsService;
using app.Services.UserpermissionsServices;
using app.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class UserpermissionController : Controller
    {
        private readonly IUserpermissionServices userpermission;
        private readonly IMainMenuService mainmenu;
        private readonly IUserServices userServices;
        public UserpermissionController(IUserpermissionServices userpermission, IMainMenuService mainmenu, IUserServices userServices)
        {
            this.userpermission = userpermission;
            this.mainmenu = mainmenu;
            this.userServices = userServices;
        }

        [HttpGet]
        public async Task<IActionResult> AddPermission(string id)
        {
            ViewBag.Recort = await userServices.GetAllRecort();
            var result = await userpermission.GetAllRecort(id);
            result.UserId = id; 
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePermission(long id, string userid)
        {
            var result = await userpermission.AddRecort(id, userid);
            return Json(result);
        }
    }
}

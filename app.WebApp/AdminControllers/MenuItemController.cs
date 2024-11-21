using app.EntityModel.CoreModel;
using app.Services.MainMenuService;
using app.Services.MenuItemService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService MenuItem;
        private readonly IMainMenuService mainmenu;
        public MenuItemController(IMenuItemService MenuItem, IMainMenuService mainmenu)
        {
            this.MenuItem = MenuItem;
            this.mainmenu = mainmenu;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
           MenuItemViewModel menuItemViewModel = new MenuItemViewModel();
            menuItemViewModel = await MenuItem.GetAllRecort();
            return View(menuItemViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> AddRecort()
        {
            MenuItemViewModel menuItemViewModel = new MenuItemViewModel();
            ViewBag.Recort = new SelectList((await mainmenu.GetAllRecort()).Select(s => new { Id = s.Id, Name = s.Name }), "Id", "Name");
            return View(menuItemViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddRecort(MenuItemViewModel model)
        {
            var result = await MenuItem.AddRecort(model);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Same Name Action Controller already exists!");
                MenuItemViewModel menuItemViewModel = new MenuItemViewModel();
                ViewBag.Recort = new SelectList((await mainmenu.GetAllRecort()).Select(s => new { Id = s.Id, Name = s.Name }), "Id", "Name");
                return View(menuItemViewModel);
            }
 
        }
        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var res = await MenuItem.DeleteRecort(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateRecort(long id)
        {
            MenuItemViewModel menuItemViewModel = new MenuItemViewModel();
            ViewBag.Recort = new SelectList((await mainmenu.GetAllRecort()).Select(s => new { Id = s.Id, Name = s.Name }), "Id", "Name");
            menuItemViewModel = await MenuItem.GetByRecort(id);
            return View(menuItemViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRecort(MenuItemViewModel viewModel)
        {
            var result = await MenuItem.UpdateRecort(viewModel);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Same Name Action Controller already exists!");
                MenuItemViewModel menuItemViewModel = new MenuItemViewModel();
                ViewBag.Recort = new SelectList((await mainmenu.GetAllRecort()).Select(s => new { Id = s.Id, Name = s.Name }), "Id", "Name");
                return View(menuItemViewModel);
            }
        }



    }
}

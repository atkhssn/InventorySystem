using app.Services.DashboardServices;
using app.Services.UserpermissionsServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDashboard_Services dashboard_Services;
        public AdminController(IHttpContextAccessor _httpContextAccessor, IDashboard_Services dashboard_Services)
        {
            this._httpContextAccessor = _httpContextAccessor;
            this.dashboard_Services = dashboard_Services;
        }
        public async Task<IActionResult> Index()
        {
            var res =await dashboard_Services.dashbordViewModel();
            //string username = HttpContext.Session.GetString("Username");
            //var serializedArrayFromSession = HttpContext.Session.GetString("ArrayData");
            //if (serializedArrayFromSession != null)
            //{
            //    var retrievedArray = JsonSerializer.Deserialize<MenuPermissionViewModel>(serializedArrayFromSession);
            //    // Now 'retrievedArray' contains the array of strings
            //}
            return View(res);
        }
    }
}

using app.Infrastructure.Auth;
using app.Services.UserpermissionsServices;
using System.Text.Json;

namespace app.WebApp.Handlers
{
    public class AssigeMenus : IAssigeMenus
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWorkContext workContext;
        public AssigeMenus(IHttpContextAccessor _httpContextAccessor, IWorkContext workContext)
        {
            this._httpContextAccessor = _httpContextAccessor;
            this.workContext = workContext;
        }
        public async Task<MenuPermissionViewModel> menulist()
        {
            MenuPermissionViewModel model = new MenuPermissionViewModel();
            string username = _httpContextAccessor.HttpContext.Session.GetString("Username");
            var serializedArrayFromSession = _httpContextAccessor.HttpContext.Session.GetString("ArrayData");
            var res = await workContext.CurrentUserAsync();
            if (serializedArrayFromSession != null)
            {
                model = JsonSerializer.Deserialize<MenuPermissionViewModel>(serializedArrayFromSession);             
            }
            return model;   
        }
    }
}

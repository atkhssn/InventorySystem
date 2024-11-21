using app.Services.UserpermissionsServices;

namespace app.WebApp.Handlers
{
    public interface IAssigeMenus
    {
        Task<MenuPermissionViewModel> menulist();
    }
}

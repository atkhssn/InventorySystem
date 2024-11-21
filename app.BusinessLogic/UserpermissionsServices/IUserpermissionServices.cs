using app.Services.MenuItemService;
using app.Services.UserpermissionsServices;
using app.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.UserpermissionsService
{
    public interface IUserpermissionServices
    {
        Task<bool> AddRecort(long Id,string UserId);
        Task<UserpermissionViewModel> GetAllRecort(string Id);
        Task<MenuPermissionViewModel> GetAllMenuItemRecort(string username);

    }
}

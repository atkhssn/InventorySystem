using app.EntityModel.CoreModel;
using app.Services.MainMenuService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.MenuItemService
{
    public interface IMenuItemService
    {
        Task<bool> AddRecort(MenuItemViewModel model);
        Task<MenuItemViewModel> GetAllRecort();
        Task<bool> UpdateRecort(MenuItemViewModel model);
        Task<bool> DeleteRecort(long Id);
        Task<MenuItemViewModel> GetByRecort(long Id);
    }
}

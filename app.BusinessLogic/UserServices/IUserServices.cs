using app.Infrastructure.Auth;
using app.Services.MenuItemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.UserServices
{
    public interface IUserServices
    {
        Task<UserViewModel> GetAllRecort();
        Task<ApplicationUser> GetByUser(string username);
        Task<UserViewModel> GetByUserId(string username);
        Task<bool> SoftDelete(string username);
        Task<int> AddUser(UserViewModel model);
        Task<int> UpdateUser(UserViewModel model);
    
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.RolesServises
{
    public interface IRoleService
    {
        Task<RoleViewModel> GetByIdAsync(string id);
        Task<bool> AddAsync(RoleViewModel model);
        Task<bool> UpdateAsync(RoleViewModel model);
        int TotalCount();
        Task<bool> DeleteByIdAsync(string id);
        List<RoleViewModel> GetAllAsync();
    }
}

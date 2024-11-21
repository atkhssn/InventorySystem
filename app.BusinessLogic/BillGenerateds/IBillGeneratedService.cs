using app.Services.MenuItemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.BillGenerateds
{
    public interface IBillGeneratedService
    {
        Task<bool> AddRecort(BillGeneratedViewModel model);
        Task<BillGeneratedViewModel> GetAllRecort();
        Task<bool> UpdateRecort(BillGeneratedViewModel model);
        Task<bool> DeleteRecort(long Id);
        Task<BillGeneratedViewModel> GetByRecort(long Id);
    }
}

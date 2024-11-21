using app.Services.Sales_Service;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Purchase_Return_Service
{
    public interface IPurchaseReturnService
    {
        Task<long> AddPurchaseReturn(PurchaseReturnViewModel model);
        Task<long> UpdatePurchaseReturn(PurchaseReturnViewModel model);
        Task<PurchaseReturnViewModel> GetPurchaseReturn(long id);
        Task<long> AddPurchaseReturnDetalies(PurchaseReturnDetailsViewModel model);
        Task<long> DeletePurchaseReturn(long id);
        Task<long> DeletePurchaseReturnDetalies(long id);
        Task<PagedModel<PurchaseReturnViewModel>> GetPagedListAsync(int page, int pageSize, string sarchString);
    }
}

using app.Services.ProductSubCategory_Service;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.PurchaseOrder_Services
{
    public interface IPurchaseOrderServices
    {
        Task<long> AddPurchaseOrder(PurchaseOrderViewModel model);
        Task<long> UpdatePurchaseOrder(PurchaseOrderViewModel model);
        Task<PurchaseOrderViewModel> GetPurchaseOrder(long id);
        Task<long> DeletePurchaseOrderDetalies(long id);
        Task<long> AddPurchaseOrderDetalies(PurchaseOrderDetailsViewModel model);
        Task<PagedModel<PurchaseOrderViewModel>> GetPagedListAsync(int page, int pageSize, string sarchString);
    }
}

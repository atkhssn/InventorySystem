using app.Services.PurchaseOrder_Services;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.SalaesReturn_service
{
    public interface ISalesReturns
    {
        Task<long> AddSalesReturn(SalesReturnViewModel model);
        Task<long> UpdateSalesReturn(SalesReturnViewModel model);
        Task<SalesReturnViewModel> GetSalesReturn(long id);
        Task<long> DeleteSalesReturnDetalies(long id);
        Task<long> AddSalesReturnDetalies(SalesReturnDetailsViewmodel model);
        Task<PagedModel<SalesReturnViewModel>> GetPagedListAsync(int page, int pageSize, string sarchString);
    }
}

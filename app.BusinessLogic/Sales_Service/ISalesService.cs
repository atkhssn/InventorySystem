using app.Services.PurchaseOrder_Services;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Sales_Service
{
    public interface ISalesService
    {
        Task<long> AddSalesOrder(SalesViewModel model);
        Task<long> UpdateSaleOrder(SalesViewModel model);
        Task<SalesViewModel> GetSaleOrder(long id);
        Task<long> AddSaleDetalies(SalesOrderDetailsViewModel model);
        Task<long> DeleteSale(long id);
        Task<long> DeleteSaleOrderDetalies(long id);
        Task<PagedModel<SalesViewModel>> GetPagedListAsync(int page, int pageSize, string sarchString);
    }
}

using app.EntityModel.DatabaseView;
using app.Services.Product_Services;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Stock_Service
{
    public interface IStockService
    {
        Task<IEnumerable<StockView>> GetStocks();  
        Task<PagedModel<StockViewModel>> GetPagedListAsync(int page, int pageSize,DateTime fromdate,DateTime todate,  string sarchString);
    }
}

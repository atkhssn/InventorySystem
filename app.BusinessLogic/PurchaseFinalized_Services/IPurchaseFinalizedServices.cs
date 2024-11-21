using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.PurchaseFinalized_Services
{
    public interface IPurchaseFinalizedServices
    {
        Task<long> GetPurchaseFinalizedAsync(long id);
        Task<long> GetSalesFinalizedAsync(long id);
        Task<long> GetSalesReturnFinalizedAsync(long id);
        Task<long> GetPurchaseReturnFinalizedAsync(long id);
    }
}

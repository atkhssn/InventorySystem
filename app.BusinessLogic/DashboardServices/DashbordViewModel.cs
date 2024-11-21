using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.DashboardServices
{
    public class DashbordViewModel
    {
        public decimal CompanyCount { get; set; }
        public decimal CoustomerCount { get; set; }
        public decimal SupplierCount { get; set; }
        public decimal TodayPayment { get; set; }
        public decimal TodayCollaction {get; set;}
        public decimal PurchaseOrderDraft { get; set;}
        public decimal PurchaseOrderCount {get; set; }
        public decimal PurchaseOrderAmt { get; set;}  
        public decimal SalesCount {get; set; }
        public decimal SalesAmt { get; set;} 
        public decimal CollactionAmt { get; set; }
        public decimal PaymentAmt { get; set;}
        public int UserType { get; set; }
    }
}

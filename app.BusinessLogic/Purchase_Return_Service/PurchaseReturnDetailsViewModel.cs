using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Purchase_Return_Service
{
    public class PurchaseReturnDetailsViewModel:BaseViewModel
    {
        public long PurchaseReturnId { set; get; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal ReturnQty { get; set; }
        public decimal PurchaseReturnRate { get; set; }
        public decimal PurchaseReturnAmount { get; set; }
    }
}

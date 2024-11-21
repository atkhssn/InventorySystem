using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.PurchaseOrder_Services
{
    public class PurchaseOrderDetailsViewModel
    {
        public long Id { get; set; }
        public long PurchaseOrderId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal PurchaseQty { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal PurchaseAmount { get; set; }
        public string PackSize { get; set; }
    }
}
